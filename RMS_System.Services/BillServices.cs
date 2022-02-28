using RMS_System.Database;
using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Services
{
    public class BillServices
    {
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
