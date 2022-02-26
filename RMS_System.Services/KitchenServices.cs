using RMS_System.Database;
using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Services
{
    public class KitchenServices
    {
        #region Singleton
        public static KitchenServices Instance
        {
            get
            {
                if (instance == null) instance = new KitchenServices();
                return instance;
            }
        }
        private static KitchenServices instance { get; set; }
        private KitchenServices()
        {
        }
        #endregion

        public List<TableEntry> GetAllEntries()
        {
            using (var context = new RMContext())
            {
                return context.TableEntries.ToList();
            }
        }


     

        

    }
}
