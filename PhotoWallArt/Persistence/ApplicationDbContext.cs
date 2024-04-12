
using Domain.Entities;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Orders;

namespace Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

            // Rename table
            modelBuilder.Entity<ApplicationUser>()
                .ToTable(nameof(ApplicationUser));
            modelBuilder.Entity<Role>()
                .ToTable(nameof(Role));
            modelBuilder.Entity<IdentityUserRole<Guid>>()
                .ToTable("ApplicationUserRole");
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        
    }
}
