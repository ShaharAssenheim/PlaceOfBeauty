using Kristin2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kristin2.Context
{
    public class MyContext : DbContext
    {
        public MyContext()
        : base("name=MyContext") { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CustomerModel>().ToTable("CustomerModels");
            modelBuilder.Entity<CalanderModel>().ToTable("Events");
            modelBuilder.Entity<ProductModel>().ToTable("Products");
        }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<ProductModel> ProductDb { get; set; }
        public DbSet<CalanderModel> Eventsdb { get; set; }
    }
}