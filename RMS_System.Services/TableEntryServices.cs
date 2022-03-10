using RMS_System.Database;
using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Services
{
    public class TableEntryServices
    {
        #region Singleton
        public static TableEntryServices Instance
        {
            get
            {
                if (instance == null) instance = new TableEntryServices();
                return instance;
            }
        }
        private static TableEntryServices instance { get; set; }
        private TableEntryServices()
        {
        }
        #endregion
        public void SaveTableEntry(TableEntry entry)
        {
            using (var context = new RMContext())
            {
                context.TableEntries.Add(entry);
                context.SaveChanges();
          
            }
        }

        [Obsolete]
        public int GetNoFoodServed(DateTime date, string ItemName)
        {
            DateTime date2 = DateTime.Parse(date.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                var result = context.TableEntries.Where(x => EntityFunctions.TruncateTime(x.OrderedTime) == date2 && x.FoodItem == ItemName).Count();
                return result;

            }
        }


        [Obsolete]
        public int GetNoFoodServedReport(DateTime startDate,DateTime endDate, string ItemName)
        {
            DateTime date1 = DateTime.Parse(startDate.ToString("yyyy-MM-dd"));
            DateTime date2 = DateTime.Parse(endDate.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                var result = context.TableEntries.Where(x => EntityFunctions.TruncateTime(x.OrderedTime) >= date1 && EntityFunctions.TruncateTime(x.OrderedTime) <= date2 && x.FoodItem == ItemName).Count();
                return result;

            }
        }


        [Obsolete]
        public int GetNoFoodServedForReport(DateTime date, string ItemName)
        {
            DateTime date2 = DateTime.Parse(date.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                var result = context.TableEntries.Where(x => EntityFunctions.TruncateTime(x.OrderedTime) == date2 && x.FoodItem == ItemName).Count();
                return result;

            }
        }


        [Obsolete]
        public IQueryable<TableEntry> GetNoFoodServed(DateTime date)
        {
            DateTime date2 = DateTime.Parse(date.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                var result = context.TableEntries.Where(x => EntityFunctions.TruncateTime(x.OrderedTime) == date2).Select(x => new { x.FoodItem, x.ProductTotal }).ToList();
                return (IQueryable<TableEntry>)result;

            }
        }






        public List<TableEntry> GetTableEntries()
        {
            using (var context = new RMContext())
            {
                return context.TableEntries.ToList();
            }
        }

        [Obsolete]
        public List<TableEntry> GetTableEntries(DateTime date)
        {
            DateTime date2 = DateTime.Parse(date.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                return context.TableEntries.Where(x => EntityFunctions.TruncateTime(x.OrderedTime) == date2).Take(5).ToList();
            }
        }

        [Obsolete]
        public List<string> GetTableEntriesDishWise(DateTime date)
        {
            DateTime date2 = DateTime.Parse(date.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                return context.TableEntries.Where(x => EntityFunctions.TruncateTime(x.OrderedTime) == date2).Select(x=>x.FoodItem).Distinct().ToList();
            }
        }


        [Obsolete]
        public List<string> GetTableEntriesDishWiseReport(DateTime startDate, DateTime endDate)
        {
            DateTime date1 = DateTime.Parse(startDate.ToString("yyyy-MM-dd"));
            DateTime date2 = DateTime.Parse(endDate.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                return context.TableEntries.Where(x => EntityFunctions.TruncateTime(x.OrderedTime) >= date1 && EntityFunctions.TruncateTime(x.OrderedTime) <= date2).Select(x => x.FoodItem).Distinct().ToList();
            }
        }

        [Obsolete]
        public List<string> GetTableEntriesDishWiseForReport(DateTime startDate, DateTime endDate)
        {
            DateTime date1 = DateTime.Parse(startDate.ToString("yyyy-MM-dd"));
            DateTime date2 = DateTime.Parse(endDate.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                return context.TableEntries.Where(x => EntityFunctions.TruncateTime(x.OrderedTime) >= date1 && EntityFunctions.TruncateTime(x.OrderedTime) <= date2).Select(x => x.FoodItem).Distinct().ToList();
            }
        }


        public List<TableEntry> GetNonDispatchedTableEntries()
        {
            using (var context = new RMContext())
            {
                return context.TableEntries.Where(x=>x.FoodDispatchedStatus == "Not Yet").ToList();
            }
        }


        public List<TableEntry> GetTableEntriesForBilling(string TableName, int OrderID)
        {
            using (var context = new RMContext())
            {
                var list = (from c in context.TableEntries join o in context.Bills on c.ID equals o.EntriesID  where c.TableName == TableName where o.EntriesID == c.ID  where o.OrderID == OrderID   select c).ToList();
                return list;
                //return context.TableEntries.Where(x => x.TableName == TableName).Join(context.Bills).ToList();
            }
        }


      

        public string GetTableNameFromEntryID(int ID)
        {
            using (var context = new RMContext())
            {
                return context.TableEntries.Where(x => x.ID == ID).Select(x => x.TableName).FirstOrDefault();
            }
        }

        public TableEntry GetTableEntry(int ID)
        {
            using (var context = new RMContext())
            {
                return context.TableEntries.Find(ID);
            }
        }

        public List<TableEntry> GetTableEntries(string TableName)
        {
            using (var context = new RMContext())
            {
                return context.TableEntries.Where(x => x.TableName == TableName && x.BillingDone == false).ToList();
            }
        }


        public void UpdateTableEntryStatus(TableEntry entry, string Status)
        {

            entry.FoodDispatchedStatus = Status;

            using (var context = new RMContext())
            {
                context.TableEntries.Attach(entry);
                context.Entry(entry).Property(x => x.FoodDispatchedStatus).IsModified = true;
                context.SaveChanges();
            }
        }


        public void UpdateTableEntryStatus(TableEntry entry, bool BillingDone)
        {
            
            entry.BillingDone = BillingDone;
          
            using (var context = new RMContext())
            {
                context.TableEntries.Attach(entry);
                context.Entry(entry).Property(x => x.BillingDone).IsModified = true;
                context.SaveChanges();
            }
        }

       



        public void UpdateTableEntry(TableEntry entry)
        {
            using (var context = new RMContext())
            {
                context.Entry(entry).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteTableEntry(int ID)
        {
            using (var context = new RMContext())
            {

                var entry = context.TableEntries.Find(ID);
                context.TableEntries.Remove(entry);
                context.SaveChanges();
            }
        }
    }
}
