using System;
using System.Collections.Generic;
using Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Context;

public partial class DomainDbContext : DbContext
{
    public DomainDbContext()
    {
    }

    public DomainDbContext(DbContextOptions<DomainDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<T_BUSINESS_MODEL> T_BUSINESS_MODELs { get; set; }

    public virtual DbSet<T_BUSINESS_MODEL_PROPERTY> T_BUSINESS_MODEL_PROPERTies { get; set; }

    public virtual DbSet<T_PROJECT> T_PROJECTs { get; set; }

    public virtual DbSet<T_REQUIREMENT> T_REQUIREMENTs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=homeserver2;database=DomainDrivenDesigner;uid=sdlfly2000;password=sdl@1215;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<T_BUSINESS_MODEL>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_T_BUSINESS_MODEL_ID");

            entity.ToTable("T_BUSINESS_MODEL");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.NAME).HasMaxLength(255);

            entity.HasOne(d => d.REQUIREMENT).WithMany(p => p.T_BUSINESS_MODELs)
                .HasForeignKey(d => d.REQUIREMENT_ID)
                .HasConstraintName("FK_T_BUSINESS_MODEL_T_REQUIREMENT");
        });

        modelBuilder.Entity<T_BUSINESS_MODEL_PROPERTY>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_T_BUSINESS_MODEL_PROPERTY_ID");

            entity.ToTable("T_BUSINESS_MODEL_PROPERTY");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.NAME).HasMaxLength(255);

            entity.HasOne(d => d.BUSINESS_MODEL).WithMany(p => p.T_BUSINESS_MODEL_PROPERTies)
                .HasForeignKey(d => d.BUSINESS_MODEL_ID)
                .HasConstraintName("FK_T_BUSINESS_MODEL_PROPERTY_T_BUSINESS_MODEL");
        });

        modelBuilder.Entity<T_PROJECT>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_T_PROJECT_ID");

            entity.ToTable("T_PROJECT");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.DESCRIPTION).HasMaxLength(255);
            entity.Property(e => e.NAME).HasMaxLength(100);
        });

        modelBuilder.Entity<T_REQUIREMENT>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_T_REQUIREMENT_ID");

            entity.ToTable("T_REQUIREMENT");

            entity.Property(e => e.ID).ValueGeneratedNever();

            entity.HasOne(d => d.PROJECT).WithMany(p => p.T_REQUIREMENTs)
                .HasForeignKey(d => d.PROJECT_ID)
                .HasConstraintName("FK_T_REQUIREMENT_T_PROJECT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
