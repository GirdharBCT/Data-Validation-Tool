using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data_Validation_Tool.DTOs;
using Microsoft.AspNetCore.Http;

namespace Data_Validation_Tool.Models
{
    public class Parms
    {
        public IFormFile formFile { get; set; }
        public DataanalysisValidationrequestDTO dVDtoRequest { get; set; }

    }
}
