using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Data_Validation_Tool.Models
{
    [Table("dataanalysis_user")]
    public partial class DataanalysisUser
    {
        public DataanalysisUser()
        {
            DataanalysisValidationrequests = new HashSet<DataanalysisValidationrequest>();
        }

        [Key]
        [Column(TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(45)]
        public string UserName { get; set; }
        [StringLength(45)]
        public string Password { get; set; }

        [InverseProperty(nameof(DataanalysisValidationrequest.RequestedByNavigation))]
        public virtual ICollection<DataanalysisValidationrequest> DataanalysisValidationrequests { get; set; }
    }
}
