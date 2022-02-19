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
