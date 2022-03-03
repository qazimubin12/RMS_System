using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Database
{
    public class RMContext : DbContext, IDisposable
    {
        public RMContext() : base("RMSConnectionStrings")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table>().Property(m => m.ServedBy).IsOptional();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<TableEntry> TableEntries { get; set; }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<Configuration> Configurations { get; set; }



       

    }
}
