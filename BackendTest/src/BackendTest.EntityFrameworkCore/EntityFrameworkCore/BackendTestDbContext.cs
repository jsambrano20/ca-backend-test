using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using BackendTest.Authorization.Roles;
using BackendTest.Authorization.Users;
using BackendTest.MultiTenancy;
using BackendTest.Entities.Billings;
using BackendTest.Entities.Customers;
using BackendTest.Entities.Products;

namespace BackendTest.EntityFrameworkCore
{
    public class BackendTestDbContext : AbpZeroDbContext<Tenant, Role, User, BackendTestDbContext>
    {
        /* Define a DbSet for each entity of the application */
        #region Entities BackendTest
        public DbSet<Billing> Billings { get; set; }
        public DbSet<BillingLine> BillingLines { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        #endregion

        public BackendTestDbContext(DbContextOptions<BackendTestDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
