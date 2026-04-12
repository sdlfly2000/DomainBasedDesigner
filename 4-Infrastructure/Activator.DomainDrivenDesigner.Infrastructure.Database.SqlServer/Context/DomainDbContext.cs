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

    public virtual DbSet<T_PROJECT> T_PROJECTs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<T_PROJECT>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_T_PROJECT_ID");

            entity.ToTable("T_PROJECT");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.DESCRIPTION).HasMaxLength(255);
            entity.Property(e => e.NAME).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
