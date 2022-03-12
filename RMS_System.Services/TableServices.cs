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
    public class TableServices
    {
        #region Singleton
        public static TableServices Instance
        {
            get
            {
                if (instance == null) instance = new TableServices();
                return instance;
            }
        }
        private static TableServices instance { get; set; }
        private TableServices()
        {
        }
        #endregion

        public Table GetTable(int ID)
        {
            using (var context = new RMContext())
            {
                return context.Tables.Find(ID);
            }
        }

        public string GetTableServedBy(string TableName)
        {
            using (var context = new RMContext())
            {
                return context.Tables.Where(x => x.TableName == TableName).Select(x => x.ServedBy).FirstOrDefault();
            }
        }

        public Table GetTable(string Name)
        {
            using (var context = new RMContext())
            {
                return context.Tables.Where(x => x.TableName == Name).FirstOrDefault();
            }
        }


        public string GetTableStatus(int ID)
        {
            using (var context = new RMContext())
            {
                return context.Tables.Where(x=>x.ID == ID).Select(x=>x.TableStatus).FirstOrDefault();
            }
        }

        public string GetTableSessionStatus(int ID)
        {
            using (var context = new RMContext())
            {
                return context.Tables.Where(x => x.ID == ID).Select(x => x.SessionStatus).FirstOrDefault();
            }
        }

        public int GetItemsServed(string TableName)
        {
            using (var context = new RMContext())
            {
                int GetItemsServed = (int)context.Tables.Where(x => x.TableName == TableName).Select(x => x.ItemsServed).FirstOrDefault();
                return GetItemsServed;
            }
        }

        public int GetItemsOrdered(string TableName)
        {
            using (var context = new RMContext())
            {
                int GetItemsServed = (int)context.Tables.Where(x => x.TableName == TableName).Select(x => x.OrderItems).FirstOrDefault();
                return GetItemsServed;
            }
        }

        public Table ServingByTable(string WaiterName)
        {
            using (var context = new RMContext())
            {
                return context.Tables.Where(x => x.ServedBy == WaiterName).FirstOrDefault();
            }
        }

        public List<Table> GetTables(string search = null)
        {
            using (var context = new RMContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Tables.Where(p => p.TableName != null && p.TableName.ToLower()
                        .Contains(search.ToLower()))
                        .OrderBy(x => x.TableName)
                        .ToList();
                }
                else
                {
                    return context.Tables
                        .OrderBy(x => x.TableName)
                        .ToList();
                }
            }
        }


        public void SaveTable(Table table)
        {
            using (var context = new RMContext())
            {
                context.Tables.Add(table);
                context.SaveChanges();
            }
        }

        public void UpdateTable(Table table)
        {
            using (var context = new RMContext())
            {
                context.Entry(table).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

      

        public void UpdateTableInfo(int TableID,string Status, int OrderedItems, string SessionStatus,string ServedBy)
        {
            var table = new Table();
            table.ID = TableID;
            table.TableStatus = Status;
            table.OrderItems = OrderedItems;
            table.ItemsServed = 0;
            table.ServedBy = ServedBy;
            table.SessionStatus = SessionStatus;
            using (var context = new RMContext())
            {
                context.Tables.Attach(table);
                context.Entry(table).Property(x => x.TableStatus).IsModified = true;
                context.Entry(table).Property(x => x.OrderItems).IsModified = true;
                context.Entry(table).Property(x => x.SessionStatus).IsModified = true;
                context.Entry(table).Property(x => x.ItemsServed).IsModified = true;
                context.Entry(table).Property(x => x.ServedBy).IsModified = true;
                context.SaveChanges();
            }
        }

        public void UpdateTableInfoData(Table table, int OrderedItems)
        {
          
            table.OrderItems = OrderedItems;
           
            using (var context = new RMContext())
            {
                context.Tables.Attach(table);
                context.Entry(table).Property(x => x.OrderItems).IsModified = true;
                context.SaveChanges();
            }
        }

        public void UpdateTableInfo(Table table, string Status, int OrderedItems, int ServedItem,  string SessionStatus, string ServedBy)
        {
            table.TableStatus = Status;
            table.OrderItems = OrderedItems;
            table.ItemsServed = ServedItem;
            table.ServedBy = ServedBy;
            table.SessionStatus = SessionStatus;
            using (var context = new RMContext())
            {
                context.Tables.Attach(table);
                context.Entry(table).Property(x => x.TableStatus).IsModified = true;
                context.Entry(table).Property(x => x.OrderItems).IsModified = true;
                context.Entry(table).Property(x => x.SessionStatus).IsModified = true;
                context.Entry(table).Property(x => x.ItemsServed).IsModified = true;
                context.Entry(table).Property(x => x.ServedBy).IsModified = true;
                context.SaveChanges();
            }
        }



        public void UpdateTableInfo(Table table,  int ServedItems)
        {
            table.ItemsServed = ServedItems;
            using (var context = new RMContext())
            {
                context.Tables.Attach(table);
                context.Entry(table).Property(x => x.ItemsServed).IsModified = true;
                context.SaveChanges();
            }
        }


        public void UpdateTableInfo(Table table, string TableStatus,string SessionStatus)
        {
            try
            {
                table.SessionStatus = SessionStatus;
                table.TableStatus = TableStatus;
                using (var context = new RMContext())
                {
                    context.Tables.Attach(table);
                    context.Entry(table).Property(x => x.SessionStatus).IsModified = true;
                    context.Entry(table).Property(x => x.TableStatus).IsModified = true;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

        public void DeleteTable(int ID)
        {
            using (var context = new RMContext())
            {

                var table = context.Tables.Find(ID);
                context.Tables.Remove(table);
                context.SaveChanges();
            }
        }
    }
}
