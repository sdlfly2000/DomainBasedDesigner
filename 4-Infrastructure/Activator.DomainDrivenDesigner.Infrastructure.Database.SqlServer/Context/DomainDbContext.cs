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

    public virtual DbSet<T_BUSINESS_ACTION> T_BUSINESS_ACTIONs { get; set; }

    public virtual DbSet<T_BUSINESS_CONTEXT> T_BUSINESS_CONTEXTs { get; set; }

    public virtual DbSet<T_BUSINESS_MODEL> T_BUSINESS_MODELs { get; set; }

    public virtual DbSet<T_PROJECT> T_PROJECTs { get; set; }

    public virtual DbSet<T_REQUIREMENT> T_REQUIREMENTs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<T_BUSINESS_ACTION>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_T_BUSINESS_ACTION_ID");

            entity.ToTable("T_BUSINESS_ACTION");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.NAME).HasMaxLength(255);

            entity.HasOne(d => d.CONTEXT).WithMany(p => p.T_BUSINESS_ACTIONs)
                .HasForeignKey(d => d.CONTEXT_ID)
                .HasConstraintName("FK_T_BUSINESS_ACTION_T_BUSINESS_CONTEXT");

            entity.HasOne(d => d.PARENT_BUSINESS_ACTION).WithMany(p => p.Child_BUSINESS_ACTIONs)
                .HasForeignKey(d => d.PARENT_BUSINESS_ACTION_ID)
                .HasConstraintName("FK_T_BUSINESS_ACTION_T_BUSINESS_ACTION");

            entity.HasOne(d => d.REQUIREMENT).WithMany(p => p.T_BUSINESS_ACTIONs)
                .HasForeignKey(d => d.REQUIREMENT_ID)
                .HasConstraintName("FK_T_BUSINESS_ACTION_T_REQUIREMENT");
        });

        modelBuilder.Entity<T_BUSINESS_CONTEXT>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_T_BUSINESS_CONTEXT_ID");

            entity.ToTable("T_BUSINESS_CONTEXT");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.NAME).HasMaxLength(255);

            entity.HasOne(d => d.T_PROJECT).WithMany(p => p.T_BUSINESS_CONTEXTs)
                .HasForeignKey(d => d.T_PROJECT_ID)
                .HasConstraintName("FK_T_BUSINESS_CONTEXT_T_PROJECT");
        });

        modelBuilder.Entity<T_BUSINESS_MODEL>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_T_BUSINESS_MODEL_ID");

            entity.ToTable("T_BUSINESS_MODEL");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.NAME).HasMaxLength(255);

            entity.HasOne(d => d.CONTEXT).WithMany(p => p.T_BUSINESS_MODELs)
                .HasForeignKey(d => d.CONTEXT_ID)
                .HasConstraintName("FK_T_BUSINESS_MODEL_T_BUSINESS_CONTEXT");

            entity.HasOne(d => d.REQUIREMENT).WithMany(p => p.T_BUSINESS_MODELs)
                .HasForeignKey(d => d.REQUIREMENT_ID)
                .HasConstraintName("FK_T_BUSINESS_MODEL_T_REQUIREMENT");
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
