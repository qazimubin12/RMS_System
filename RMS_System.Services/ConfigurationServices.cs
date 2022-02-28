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
    public class ConfigurationServices
    {
        #region Singleton
        public static ConfigurationServices Instance
        {
            get
            {
                if (instance == null) instance = new ConfigurationServices();
                return instance;
            }
        }
        private static ConfigurationServices instance { get; set; }
        private ConfigurationServices()
        {
        }
        #endregion

        public Configuration GetConfig(int ID)
        {
            using (var context = new RMContext())
            {
                return context.Configurations.Find(ID);
            }
        }

        public Configuration GetConfig()
        {
            using (var context = new RMContext())
            {
                return context.Configurations.FirstOrDefault();
            }
        }

        public void SaveConfig(Configuration config)
        {
            using (var context = new RMContext())
            {
                context.Configurations.Add(config);
                context.SaveChanges();
            }
        }



        public void UpdateConfig(Configuration config)
        {
            using (var context = new RMContext())
            {
                try
                {
                    context.Entry(config).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
                
            }
        }

        public void DeleteConfig(int ID)
        {
            using (var context = new RMContext())
            {

                var configuration = context.Configurations.Find(ID);
                context.Configurations.Remove(configuration);
                context.SaveChanges();
            }
        }
    }
}
