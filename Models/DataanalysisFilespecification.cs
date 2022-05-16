using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Data_Validation_Tool.Models
{
    [Table("dataanalysis_filespecification")]
    public partial class DataanalysisFilespecification
    {
        public DataanalysisFilespecification()
        {
            DataanalysisFilespecificationcolumns = new HashSet<DataanalysisFilespecificationcolumn>();
            DataanalysisValidationrequests = new HashSet<DataanalysisValidationrequest>();
        }

        [Key]
        [Column(TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(45)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }

        [InverseProperty(nameof(DataanalysisFilespecificationcolumn.FileSpecification))]
        public virtual ICollection<DataanalysisFilespecificationcolumn> DataanalysisFilespecificationcolumns { get; set; }
        [InverseProperty(nameof(DataanalysisValidationrequest.FileSpecification))]
        public virtual ICollection<DataanalysisValidationrequest> DataanalysisValidationrequests { get; set; }
    }
}
