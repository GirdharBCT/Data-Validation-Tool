using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Data_Validation_Tool.Models
{
    [Table("dataanalysis_filespecificationcolumn")]
    [Index(nameof(FileSpecificationId), Name = "FK.FileSpecificationId_idx")]
    public partial class DataanalysisFilespecificationcolumn
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Id { get; set; }
        [Column(TypeName = "int(11)")]
        public int? FileSpecificationId { get; set; }
        [StringLength(45)]
        public string Name { get; set; }
        [StringLength(45)]
        public string InternalName { get; set; }
        [StringLength(45)]
        public string Order { get; set; }

        [ForeignKey(nameof(FileSpecificationId))]
        [InverseProperty(nameof(DataanalysisFilespecification.DataanalysisFilespecificationcolumns))]
        public virtual DataanalysisFilespecification FileSpecification { get; set; }
    }
}
