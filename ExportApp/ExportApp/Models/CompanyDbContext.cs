using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExportApp.Models;

public partial class CompanyDbContext : DbContext
{
    public CompanyDbContext()
    {
    }

    public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=RITESH;Initial Catalog=CompanyDB;Integrated Security=True;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.MaritalStatus).HasMaxLength(50);
            entity.Property(e => e.Pincode).HasColumnName("PINCode");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.Pincode).HasColumnName("PINCode");
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.State).HasMaxLength(50).IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.ProductPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__48CFD27E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
