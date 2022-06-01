using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_Validation_Tool.DTOs
{
    public class DataanalysisValidationrequestDTO
    {
        public int FileSpecificationId { get; set; }
        public int FileType { get; set; }
        public string NotificationEmail { get; set; }
    }
}
