using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Data_Validation_Tool.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data_Validation_Tool.Models;
using Data_Validation_Tool.DTOs;

namespace Data_Validation_Tool.Controllers
{
    [Authorize]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IS3FileUpload _service;
        public UploadController(IS3FileUpload service)
        {
            _service = service;
        }
        [HttpPut]
        [AllowAnonymous]
        [RequestSizeLimit(10000000000)]
        [Route("Upload")]
        public async Task<IActionResult> AddFile([FromForm]Parms parms)
        {
            try
            {
                var response = await _service.AddFileAsync(parms);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
