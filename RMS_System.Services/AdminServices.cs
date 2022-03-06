using RMS_System.Database;
using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RMS_System.Services
{
    public class AdminServices
    {
        #region Singleton
        public static AdminServices Instance
        {
            get
            {
                if (instance == null) instance = new AdminServices();
                return instance;
            }
        }
        private static AdminServices instance { get; set; }
        private AdminServices()
        {
        }
        #endregion
              
    }
}
