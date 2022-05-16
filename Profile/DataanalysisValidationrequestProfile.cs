using Data_Validation_Tool.DTOs;
using Data_Validation_Tool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Data_Validation_Tool.Profile
{
    public class DataanalysisValidationrequestProfile : AutoMapper.Profile 
    {
        public DataanalysisValidationrequestProfile()
        {
            CreateMap<DataanalysisValidationrequest, DataanalysisValidationrequestDTO>().ReverseMap();

        }
}
    
}
