using ClothesWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace ClothesWeb.DBContext
{
    public class ClothesDbContext : DbContext
    {
        public ClothesDbContext():base("name=clothesEntities")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DetailBIll> DetailBs { get; set; }
        public virtual DbSet<ImageProduct> ImageProducts { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }


    }
}