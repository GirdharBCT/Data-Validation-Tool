using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Data_Validation_Tool.Models;

#nullable disable

namespace Data_Validation_Tool.Data
{
    public partial class prd_phyndContext : DbContext
    {
        public prd_phyndContext()
        {
        }

        public prd_phyndContext(DbContextOptions<prd_phyndContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataanalysisFilespecification> DataanalysisFilespecifications { get; set; }
        public virtual DbSet<DataanalysisFilespecificationcolumn> DataanalysisFilespecificationcolumns { get; set; }
        public virtual DbSet<DataanalysisUser> DataanalysisUsers { get; set; }
        public virtual DbSet<DataanalysisValidationrequest> DataanalysisValidationrequests { get; set; }
        public virtual DbSet<DataanalysisValidationstatus> DataanalysisValidationstatuses { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseMySQL("server=localhost;uid=root;pwd=root;database=prd_phynd;SSL mode=none");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataanalysisFilespecificationcolumn>(entity =>
            {
                entity.HasOne(d => d.FileSpecification)
                    .WithMany(p => p.DataanalysisFilespecificationcolumns)
                    .HasForeignKey(d => d.FileSpecificationId)
                    .HasConstraintName("FK.FileSpecificationId");
            });

            modelBuilder.Entity<DataanalysisValidationrequest>(entity =>
            {
                entity.HasOne(d => d.FileSpecification)
                    .WithMany(p => p.DataanalysisValidationrequests)
                    .HasForeignKey(d => d.FileSpecificationId)
                    .HasConstraintName("FK.RequestFileSpecificationId");

                entity.HasOne(d => d.RequestedByNavigation)
                    .WithMany(p => p.DataanalysisValidationrequests)
                    .HasForeignKey(d => d.RequestedBy)
                    .HasConstraintName("FK.RequestedBy");

                entity.HasOne(d => d.ValidationStatus)
                    .WithMany(p => p.DataanalysisValidationrequests)
                    .HasForeignKey(d => d.ValidationStatusId)
                    .HasConstraintName("FK.ValidationStatusId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
