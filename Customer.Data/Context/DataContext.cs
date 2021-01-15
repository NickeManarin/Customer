using System;
using System.Collections.Generic;
using System.Text;
using Customers.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Customers.Data.Context
{
    public class DataContext : DbContext
    {
        //Migrations:
        //Add-Migration "Version0001" -Context DataContext -verbose -Project Customers.Data
        //Remove-Migration -Context DataContext -verbose -Project Customers.Data

        //Common SQL commands:
        //Open SQL Server Object Explorer, right click the server, click on "New query".
        //Drop Database CustomerDatabase;

        public DbSet<User> Users { get; set; }
       
        public DbSet<UserRole> UserRoles { get; set; }
        
        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Region> Regions { get; set; }
        
        public DbSet<City> Cities { get; set; }
        
        public DbSet<Classification> Classifications { get; set; }
        
        public DbSet<Gender> Genders { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif

            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Relational configurations

            //builder.Entity<UserRole>().HasMany(h => h.Users).WithOne(w => w.UserRole);
            //builder.Entity<User>().HasOne(bc => bc.UserRole).WithMany(b => b.Users);

            #endregion

            #region Seed data

            //Roles.
            var roles = new List<UserRole>
            {
                new UserRole { Id = 1, Name = "Administrator", IsAdmin = true },
                new UserRole { Id = 2, Name = "Seller", IsAdmin = false }
            };
            
            builder.Entity<UserRole>().HasData(roles);

            //Users.
            var users = new List<User>
            {
                new User { Id = 1, Email = "admin@app.com", UserRoleId = 1 },
                new User { Id = 2, Email = "seller1@app.com", UserRoleId = 2 },
                new User { Id = 3, Email = "seller2@app.com", UserRoleId = 2 }
            };

            //Creates the passwords for each user, for the sake of the example.
            //It would be better if these passwords were already hashed and not in plain sight.
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            users[0].PasswordSalt = hmac.Key;
            users[0].PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("admin@123"));
            users[0].PasswordLastUpdatedUtc = DateTime.UtcNow;
            users[1].PasswordSalt = hmac.Key;
            users[1].PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("seller@1"));
            users[1].PasswordLastUpdatedUtc = DateTime.UtcNow;
            users[2].PasswordSalt = hmac.Key;
            users[2].PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("seller@2"));
            users[2].PasswordLastUpdatedUtc = DateTime.UtcNow;

            builder.Entity<User>().HasData(users);

            //Regions.
            var regions = new List<Region>
            {
                new Region { Id = 1, Name = "Rio Grande do Sul" },
                new Region { Id = 2, Name = "São Paulo" },
                new Region { Id = 3, Name = "Curitiba" }
            };

            builder.Entity<Region>().HasData(regions);

            //Cities.
            var cities = new List<City>
            {
                new City { Id = 1, Name = "Porto Alegre", RegionId = 1 }
            };

            builder.Entity<City>().HasData(cities);

            //Classifications.
            var classifications = new List<Classification>
            {
                new Classification { Id = 1, Name = "VIP" },
                new Classification { Id = 2, Name = "Regular" },
                new Classification { Id = 3, Name = "Sporadic" }
            };

            builder.Entity<Classification>().HasData(classifications);

            //Genders.
            var genders = new List<Gender>
            {
                new Gender { Id = 1, Name = "Masculine" },
                new Gender { Id = 2, Name = "Feminine" }
            };

            builder.Entity<Gender>().HasData(genders);

            //Customers.
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Maurício", Phone = "(11) 95429999", GenderId = 1, CityId = 1, LastPurchase = new DateTime(2016, 09, 10), ClassificationId = 1, UserId = 3},
                new Customer { Id = 2, Name = "Carla", Phone = "(53) 94569999 ", GenderId = 2, CityId = 1, LastPurchase = new DateTime(2015, 10, 10), ClassificationId = 1, UserId = 2},
                new Customer { Id = 3, Name = "Maria", Phone = "(64) 94518888", GenderId = 2, CityId = 1, LastPurchase = new DateTime(2013, 10, 12), ClassificationId = 3, UserId = 2},
                new Customer { Id = 4, Name = "Douglas", Phone = "(51) 12455555 ", GenderId = 1, CityId = 1, LastPurchase = new DateTime(2016, 05, 05), ClassificationId = 2, UserId = 2 },
                new Customer { Id = 5, Name = "Marta", Phone = "(51) 45788888 ", GenderId = 2, CityId = 1, LastPurchase = new DateTime(2016, 08, 08), ClassificationId = 2, UserId = 3 }
            };

            builder.Entity<Customer>().HasData(customers);
            
            #endregion

            base.OnModelCreating(builder);
        }
    }
}