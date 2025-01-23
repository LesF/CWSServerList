using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CWSServerList.Models;

public partial class ClinicalIntranetAlphaContext : DbContext
{
    DbContextOptions<ClinicalIntranetAlphaContext>? _options;

    public ClinicalIntranetAlphaContext()
    {
    }

    public ClinicalIntranetAlphaContext(DbContextOptions<ClinicalIntranetAlphaContext> options)
        : base(options)
    {
        _options = options;
    }

    public virtual DbSet<Cwsenvironment> Cwsenvironments { get; set; }

    public virtual DbSet<Cwsserver> Cwsservers { get; set; }

    public virtual DbSet<CwsserverPurpose> CwsserverPurposes { get; set; }

    public virtual DbSet<Cwsversion> Cwsversions { get; set; }

    public virtual DbSet<UserPref> UserPrefs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Use a default connection string if none is provided
            optionsBuilder.UseSqlServer("Server=your_server;Database=your_database;User Id=your_user;Password=your_password;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cwsenvironment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CWSEnvironment");

            entity.Property(e => e.Environment).HasMaxLength(40);
            entity.Property(e => e.EnvironmentCode).HasMaxLength(10);
            entity.Property(e => e.Nlburl)
                .HasMaxLength(200)
                .HasColumnName("NLBURL");
            entity.Property(e => e.Version).HasMaxLength(50);
        });

        modelBuilder.Entity<Cwsserver>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CWSServer");

            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.Cwsurl)
                .HasMaxLength(200)
                .HasColumnName("CWSURL");
            entity.Property(e => e.EnvironmentCode).HasMaxLength(10);
            entity.Property(e => e.Gateway).HasMaxLength(16);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(16)
                .HasColumnName("IPAddress");
            entity.Property(e => e.Prefix).HasMaxLength(20);
            entity.Property(e => e.ServerName).HasMaxLength(50);
            entity.Property(e => e.ServiceAccount).HasMaxLength(50);
            entity.Property(e => e.TypeCode).HasMaxLength(10);
            entity.Property(e => e.Vmgroup).HasColumnName("VMGroup");
        });

        modelBuilder.Entity<CwsserverPurpose>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CWSServerPurpose");

            entity.Property(e => e.ServerTypeName).HasMaxLength(50);
            entity.Property(e => e.SortKey).HasMaxLength(5);
            entity.Property(e => e.TypeCode).HasMaxLength(10);
        });

        modelBuilder.Entity<Cwsversion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CWSVersion");

            entity.Property(e => e.VersionName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserPref>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.EnvironmentCode).HasMaxLength(10);
            entity.Property(e => e.QuickLinks).HasMaxLength(250);
            entity.Property(e => e.TypeCode).HasMaxLength(10);
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.VersionId).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
