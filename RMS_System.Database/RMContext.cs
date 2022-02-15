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
        public RMContext() : base("RMS_Connection")
        {

        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
