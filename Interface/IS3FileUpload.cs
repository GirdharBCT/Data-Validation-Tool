using Data_Validation_Tool.DTOs;
using Data_Validation_Tool.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_Validation_Tool.Interface
{
    public interface IS3FileUpload
    {
        Task<S3ApiResponse> AddFileAsync(Parms parms);
    }
}
