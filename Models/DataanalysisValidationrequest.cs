using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Data_Validation_Tool.Models
{
    [Table("dataanalysis_validationrequest")]
    [Index(nameof(FileSpecificationId), Name = "FK.FileSpecificationId_idx")]
    [Index(nameof(RequestedBy), Name = "FK.RequestedBy_idx")]
    [Index(nameof(ValidationStatusId), Name = "FK.ValidationStatusId_idx")]
    public partial class DataanalysisValidationrequest
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(200)]
        public string Url { get; set; }
        [Column(TypeName = "int(11)")]
        public int? RequestedBy { get; set; }
        [Column(TypeName = "int(11)")]
        public int? HealthSystemId { get; set; }
        [Column(TypeName = "int(11)")]
        public int? FileSpecificationId { get; set; }
        [Column(TypeName = "int(11)")]
        public int? FileType { get; set; }
        [StringLength(100)]
        public string NotificationEmail { get; set; }
        [Column(TypeName = "int(11)")]
        public int? ValidationStatusId { get; set; }
        public DateTime? RequestedDate { get; set; }
        public float? Size { get; set; }
        [Column(TypeName = "int(11)")]
        public int? NoOfColumns { get; set; }
        [Column("NoOFRows", TypeName = "int(11)")]
        public int? NoOfrows { get; set; }
        [StringLength(200)]
        public string Result { get; set; }

        [ForeignKey(nameof(FileSpecificationId))]
        [InverseProperty(nameof(DataanalysisFilespecification.DataanalysisValidationrequests))]
        public virtual DataanalysisFilespecification FileSpecification { get; set; }
        [ForeignKey(nameof(RequestedBy))]
        [InverseProperty(nameof(DataanalysisUser.DataanalysisValidationrequests))]
        public virtual DataanalysisUser RequestedByNavigation { get; set; }
        [ForeignKey(nameof(ValidationStatusId))]
        [InverseProperty(nameof(DataanalysisValidationstatus.DataanalysisValidationrequests))]
        public virtual DataanalysisValidationstatus ValidationStatus { get; set; }
    }
}
