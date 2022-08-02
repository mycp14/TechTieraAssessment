using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.Entities;
using WebApp.Data.Model_Configuration;

namespace WebApp.Data.Context
{
    public class WebAppContext : DbContext
    {
        public WebAppContext(DbContextOptions<WebAppContext> options) : base(options)
        {
        }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Table Configuration
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.Seed();
            //base.OnModelCreating(modelBuilder);
            #endregion
        }
    }
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
        }
    }
}
