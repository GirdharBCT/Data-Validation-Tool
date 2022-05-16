using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Data_Validation_Tool.Models
{
    [Table("dataanalysis_validationstatus")]
    public partial class DataanalysisValidationstatus
    {
        public DataanalysisValidationstatus()
        {
            DataanalysisValidationrequests = new HashSet<DataanalysisValidationrequest>();
        }

        [Key]
        [Column(TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(45)]
        public string Name { get; set; }

        [InverseProperty(nameof(DataanalysisValidationrequest.ValidationStatus))]
        public virtual ICollection<DataanalysisValidationrequest> DataanalysisValidationrequests { get; set; }
    }
}
