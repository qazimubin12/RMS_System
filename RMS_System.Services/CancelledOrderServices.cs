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
   public class CancelledOrderServices
    {

        #region Singleton
        public static CancelledOrderServices Instance
        {
            get
            {
                if (instance == null) instance = new CancelledOrderServices();
                return instance;
            }
        }
        private static CancelledOrderServices instance { get; set; }
        private CancelledOrderServices()
        {
        }
        #endregion
        public void SaveOrder(CancelledOrders cancelorder)
        {
            using (var context = new RMContext())
            {
                try
                {

                    context.CancelledOrders.Add(cancelorder);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }


            }
        }


        [Obsolete]
        public List<CancelledOrders> GetCancelledOrders(DateTime date)
        {
            DateTime date2 = DateTime.Parse(date.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                return context.CancelledOrders.Where(x => EntityFunctions.TruncateTime(x.CancelledDate) == date2).ToList();
            }
        }

        [Obsolete]
        public List<CancelledOrders> GetCancelledOrdersReport(DateTime startdate, DateTime enddate)
        {
            DateTime date1 = DateTime.Parse(startdate.ToString("yyyy-MM-dd"));
            DateTime date2 = DateTime.Parse(enddate.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                return context.CancelledOrders.Where(x => EntityFunctions.TruncateTime(x.CancelledDate) >= date1 && EntityFunctions.TruncateTime(x.CancelledDate) <= date2).ToList();
            }
        }

    }
}
