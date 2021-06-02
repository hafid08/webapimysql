using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loyalto
{
    public partial class LoyaltoContext : DbContext
    {
        public LoyaltoContext() :
            base()
        {
            OnCreated();

        }
        public LoyaltoContext(DbContextOptions<LoyaltoContext> options) :
            base(options)
        {
            OnCreated();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.Options.Extensions.OfType<RelationalOptionsExtension>().Any(ext => !string.IsNullOrEmpty(ext.ConnectionString) || ext.Connection != null))
                CustomizeConfiguration(ref optionsBuilder);
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseMySQL("server=localhost;database=library;user=user;password=password");
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);
        public DbSet<Pengguna> Pengguna { get; set; }
        public DbSet<Valuta> Valuta { get; set; }
        public DbSet<Toqen> Toqen { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Valuta>(entity =>
            {
                entity.HasKey(e => e.vid);
                entity.Property(e => e.vcode).IsRequired();
                entity.Property(e => e.vname).IsRequired();
                entity.Property(e => e.vprice).IsRequired();
            });
            modelBuilder.Entity<Pengguna>(entity =>
            {
                entity.HasKey(e => e.pid);
                entity.Property(e => e.puser).IsRequired();
                entity.Property(e => e.ppass).IsRequired();
                entity.Property(e => e.pstatus).IsRequired();
            });
            modelBuilder.Entity<Toqen>(entity =>
            {
                entity.HasKey(e => e.pid);
                entity.Property(e => e.token).IsRequired();
                entity.Property(e => e.ttime).IsRequired();
                entity.Property(e => e.texpired).IsRequired();
            });

        }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }
}
