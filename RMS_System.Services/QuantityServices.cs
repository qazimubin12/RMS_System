using RMS_System.Database;
using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Services
{
    public class QuantityServices
    {
        #region Singleton
        public static QuantityServices Instance
        {
            get
            {
                if (instance == null) instance = new QuantityServices();
                return instance;
            }
        }
        private static QuantityServices instance { get; set; }
        private QuantityServices()
        {
        }
        #endregion
        public List<MenuItem> GetMenuItemForQuantity(int ID)
        {
            using (var context = new RMContext())
            {
                List<MenuItem> list = context.MenuItems.Where(p => p.ID == ID).ToList();
                return list;
            }
        }
    }
}
