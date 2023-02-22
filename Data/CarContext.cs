using System;
using System.Collections.Generic;
using CarRentalApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalApplication.Data
{
    public class CarContext : DbContext
    {
        public CarContext(DbContextOptions<CarContext> options) : base(options)
        {
        }

        public DbSet<Car> Car { get; set; }        
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema("Car");

            modelBuilder.Entity<Car>().ToTable("Car");
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<Payment>().ToTable("Payment");
            modelBuilder.Entity<Category>().ToTable("Category");
        }

        internal static Task<string> ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
