using ExportApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ExportApp.Context
{
    public class CompanyDBContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Customer> Orders { get; set; }

        public CompanyDBContext(DbContextOptions<CompanyDBContext> options) : base(options)
        {

        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //    optionsBuilder.UseSqlServer("Data Source=RITESH;Initial Catalog=CompanyDB;Integrated Security=True;Trust Server Certificate=True;");

        //    // base.OnConfiguring(optionsBuilder);
        // }
    }
}
