using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRN221.Application;

public partial class FucarRentingManagementContext : DbContext
{
    public FucarRentingManagementContext()
    {
    }

    public FucarRentingManagementContext(DbContextOptions<FucarRentingManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CarInformation> CarInformations { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<RentingDetail> RentingDetails { get; set; }

    public virtual DbSet<RentingTransaction> RentingTransactions { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=FUCarRentingManagement;User Id=sa;Password=12345;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarInformation>(entity =>
        {
            entity.HasKey(e => e.CarId);

            entity.ToTable("CarInformation");

            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.CarDescription).HasMaxLength(220);
            entity.Property(e => e.CarName).HasMaxLength(50);
            entity.Property(e => e.CarRentingPricePerDay).HasColumnType("money");
            entity.Property(e => e.FuelType).HasMaxLength(20);
            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.CarInformations)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("FK_CarInformation_Manufacturer");

            entity.HasOne(d => d.Supplier).WithMany(p => p.CarInformations)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_CarInformation_Supplier");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D1053426468AC9").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerBirthday).HasColumnType("date");
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Telephone).HasMaxLength(12);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.ToTable("Manufacturer");

            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.ManufacturerCountry)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ManufacturerName).HasMaxLength(50);
        });

        modelBuilder.Entity<RentingDetail>(entity =>
        {
            entity.HasKey(e => new { e.RentingTransactionId, e.CarId });

            entity.ToTable("RentingDetail");

            entity.Property(e => e.RentingTransactionId).HasColumnName("RentingTransactionID");
            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Car).WithMany(p => p.RentingDetails)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK_RentingDetail_CarInformation");

            entity.HasOne(d => d.RentingTransaction).WithMany(p => p.RentingDetails)
                .HasForeignKey(d => d.RentingTransactionId)
                .HasConstraintName("FK_RentingDetail_RentingTransaction");
        });

        modelBuilder.Entity<RentingTransaction>(entity =>
        {
            entity.HasKey(e => e.RentingTransationId);

            entity.ToTable("RentingTransaction");

            entity.Property(e => e.RentingTransationId)
                .ValueGeneratedNever()
                .HasColumnName("RentingTransationID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.RentingDate).HasColumnType("date");
            entity.Property(e => e.TotalPrice).HasColumnType("money");

            entity.HasOne(d => d.Customer).WithMany(p => p.RentingTransactions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_RentingTransaction_Customer");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier");

            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.SupplierAddress).HasMaxLength(80);
            entity.Property(e => e.SupplierDescription).HasMaxLength(250);
            entity.Property(e => e.SupplierName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
