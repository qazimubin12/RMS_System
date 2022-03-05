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


        public List<TableEntry> GetTableEntries()
        {
            using (var context = new RMContext())
            {
                return context.TableEntries.ToList();
            }
        }

        


        public List<TableEntry> GetNonDispatchedTableEntries()
        {
            using (var context = new RMContext())
            {
                return context.TableEntries.Where(x=>x.FoodDispatchedStatus == "Not Yet").ToList();
            }
        }


        public List<TableEntry> GetTableEntriesForBilling(string TableName)
        {
            using (var context = new RMContext())
            {
                var list = (from c in context.TableEntries join o in context.Bills on c.ID equals o.EntriesID  where c.TableName == TableName where o.EntriesID == c.ID  select c).ToList();
                return list;
                //return context.TableEntries.Where(x => x.TableName == TableName).Join(context.Bills).ToList();
            }
        }


        //public List<TableEntry> GetTableEntriesForWaiter(string TableName)
        //{
        //    using (var context = new RMContext())
        //    {
        //        var list = (from c in context.TableEntries join o in context.Tables on c.TableName equals o.TableName where c.TableName == TableName where o.SessionStatus == "Active"  select c).ToList();
        //        return list;
        //    }
        //}

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
