using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Data_Validation_Tool.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Data_Validation_Tool.Data;
using Data_Validation_Tool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Data_Validation_Tool.DTOs;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Data_Validation_Tool.Services
{
    public class S3FileUpload : IS3FileUpload
    {
        private static IAmazonS3 _s3Client;
        private readonly prd_phyndContext _context;
        private readonly IMapper _mapper;
        //private IConfiguration _configuration;
        private const string bucketName = "demo-dvt-glue-bucket/input";
        public S3FileUpload(IAmazonS3 s3Client,prd_phyndContext context,IMapper mapper)
        {
            _s3Client = s3Client;
            _context = context;
            _mapper = mapper;
            //_configuration = configuration;
        }
        public async Task<S3ApiResponse> AddFileAsync(Parms parms)
        {
            List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();

            if (parms.formFile == null || parms.formFile.Length == 0)
            {
                return new S3ApiResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = "File is null or empty"
                };
            }

            //Making entry in dataanalysis_validationrequest table
            var dVRequest = _mapper.Map<DataanalysisValidationrequest>(parms.dVDtoRequest);
            dVRequest.RequestedBy = 1;
            dVRequest.ValidationStatusId = 1;
            dVRequest.RequestedDate = DateTime.Now;
            await _context.DataanalysisValidationrequests.AddAsync(dVRequest);
            await _context.SaveChangesAsync();
            var validationRequest = await _context.DataanalysisValidationrequests.OrderByDescending(x=>x.Id).FirstAsync();
            //_context.DataanalysisValidationrequests.Add(dVRequest);
            //_context.SaveChanges();
            //var validationRequest = dVRequest;

            //----

            InitiateMultipartUploadRequest initiateRequest = new InitiateMultipartUploadRequest
            {
                BucketName = bucketName,
                Key = parms.formFile.FileName
            };

            initiateRequest.Metadata.Add("validationRequestId", validationRequest.Id.ToString()); //Adding Validation request id in metadata

            InitiateMultipartUploadResponse initResponse =
                await _s3Client.InitiateMultipartUploadAsync(initiateRequest);

            //File Operation
            byte[] fileBytes = new Byte[parms.formFile.Length];
            var path = Path.Combine(Directory.GetCurrentDirectory(), parms.formFile.FileName);
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                await parms.formFile.CopyToAsync(stream);
            }
            long contentLength = new FileInfo(parms.formFile.FileName).Length;
            long partSize = 30 * (long)Math.Pow(2, 20);

            //AWS Official Low-level API Implementation for Multipart Upload
            try
            {
                long filePosition = 0;
                for (int i = 1; filePosition < contentLength; i++)
                {
                    UploadPartRequest uploadRequest = new UploadPartRequest
                    {
                        BucketName = bucketName,
                        Key = parms.formFile.FileName,
                        UploadId = initResponse.UploadId,
                        PartNumber = i,
                        PartSize = partSize,
                        FilePosition = filePosition,
                        FilePath = parms.formFile.FileName
                    };

                    uploadRequest.StreamTransferProgress +=
                        new EventHandler<StreamTransferProgressArgs>(UploadPartProgress);

                    uploadResponses.Add(await _s3Client.UploadPartAsync(uploadRequest));

                    filePosition += partSize;
                }

                CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = parms.formFile.FileName,
                    UploadId = initResponse.UploadId
                };

                completeRequest.AddPartETags(uploadResponses);

                CompleteMultipartUploadResponse completeUploadResponse =
                    await _s3Client.CompleteMultipartUploadAsync(completeRequest);

                //Writing file location in dataanalysis_validationrequest table
                int dVRequestId = validationRequest.Id;
                DataanalysisValidationrequest dVRequestObj = await _context.DataanalysisValidationrequests.OrderByDescending(x=>x.Id).FirstOrDefaultAsync(i => i.Id == dVRequestId);
                dVRequestObj.Url = completeUploadResponse.Location;
                dVRequestObj.Size = parms.formFile.Length;
                await _context.SaveChangesAsync();
                //-------------------------

                return new S3ApiResponse
                {
                    Status = HttpStatusCode.OK,
                    Message = "File uploaded to S3",
                    Location = completeUploadResponse.Location,
                    ETag = completeUploadResponse.ETag,
                };

            }
            catch (AmazonS3Exception e)
            {
                return new S3ApiResponse
                {
                    Status = e.StatusCode,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("An AmazonS3Exception was thrown: {0}", e.Message);

                AbortMultipartUploadRequest abortMPURequest = new AbortMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = parms.formFile.FileName,
                    UploadId = initResponse.UploadId
                };
                await _s3Client.AbortMultipartUploadAsync(abortMPURequest);
            }

            return new S3ApiResponse
            {
                Status = HttpStatusCode.InternalServerError,
                Message = "Error. Failed to upload File"
            };
        }

        public static void UploadPartProgress(object sender, StreamTransferProgressArgs e)
        {
            Console.WriteLine("{0}/{1}, {2}% done", e.TransferredBytes, e.TotalBytes, e.PercentDone);
        }
    }
}
