﻿using RMS_System.Database;
using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Services
{
    public class MenuItemServices
    {
        #region Singleton
        public static MenuItemServices Instance
        {
            get
            {
                if (instance == null) instance = new MenuItemServices();
                return instance;
            }
        }
        private static MenuItemServices instance { get; set; }
        private MenuItemServices()
        {
        }
        #endregion

        public MenuItem GetMenuItem(int ID)
        {
            using (var context = new RMContext())
            {
                return context.MenuItems.Find(ID);
            }
        }

        public List<MenuItem> GetMenuItems(string search = null)
        {
            using (var context = new RMContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.MenuItems.Where(p => p.MenuName != null && p.MenuName.ToLower()
                        .Contains(search.ToLower()))
                        .OrderBy(x => x.MenuName)
                        .ToList();
                }
                else
                {
                    return context.MenuItems
                        .OrderBy(x => x.MenuName)
                        .ToList();
                }
            }
        }

        public void SaveMenuItem(MenuItem item)
        {
            using (var context = new RMContext())
            {
                context.MenuItems.Add(item);
                context.SaveChanges();
            }
        }

        public void UpdateMenuItems(MenuItem MenuItems)
        {
            using (var context = new RMContext())
            {
                context.Entry(MenuItems).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteMenuItems(int ID)
        {
            using (var context = new RMContext())
            {

                var MenuItems = context.MenuItems.Find(ID);
                context.MenuItems.Remove(MenuItems);
                context.SaveChanges();
            }
        }

    }
}
