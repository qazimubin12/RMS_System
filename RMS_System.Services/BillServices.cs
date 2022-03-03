using RMS_System.Database;
using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Services
{
    public class BillServices
    {
        #region Singleton
        public static BillServices Instance
        {
            get
            {
                if (instance == null) instance = new BillServices();
                return instance;
            }
        }
        private static BillServices instance { get; set; }
        private BillServices()
        {
        }
        #endregion
        public Bill GetBill(int ID)
        {
            using (var context = new RMContext())
            {
                return context.Bills.Find(ID);
            }
        }

        public void SaveBill(Bill bill)
        {
            using (var context = new RMContext())
            {
                context.Bills.Add(bill);
                context.SaveChanges();
            }
        }


        public List<int> GetTableEntires(int OrderID)
        {
            using (var context = new RMContext())
            {
                return context.Bills.Where(x => x.OrderID == OrderID).Select(x => x.EntriesID).ToList();
            }
        }


        public Double GetSGST(int OrderID)
        {
            using (var context = new RMContext())
            {
                return context.Bills.Where(x => x.OrderID == OrderID).Select(x => x.SGST).FirstOrDefault();
            }
        }

        public Double GetCGST(int OrderID)
        {
            using (var context = new RMContext())
            {
                return context.Bills.Where(x => x.OrderID == OrderID).Select(x => x.CGST).FirstOrDefault();
            }
        }



        public void UpdateBill(Bill Bill)
        {
            using (var context = new RMContext())
            {
                context.Entry(Bill).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteBill(int ID)
        {
            using (var context = new RMContext())
            {

                var Bill = context.Bills.Find(ID);
                context.Bills.Remove(Bill);
                context.SaveChanges();
            }
        }

    }
}
