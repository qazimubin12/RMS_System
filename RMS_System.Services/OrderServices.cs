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
    public class OrderServices
    {
        #region Singleton
        public static OrderServices Instance
        {
            get
            {
                if (instance == null) instance = new OrderServices();
                return instance;
            }
        }
        private static OrderServices instance { get; set; }
        private OrderServices()
        {
        }
        #endregion
        public void SaveOrder(Order order)
        {
            using (var context = new RMContext())
            {
                try
                {
                    
                    context.Orders.Add(order);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
                

            }
        }


        public List<Order> GetOrders()
        {
            using (var context = new RMContext())
            {
                return context.Orders.ToList();
            }
        }


        public Order GetOrderByTable(string TableName)
        {
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => x.TableName == TableName).FirstOrDefault();
            }
        }

        public int GetItemsServedForOrder(string TableName)
        {
            using (var context = new RMContext())
            {
                int GetItemsServed = (int)context.Orders.Where(x => x.TableName == TableName).Select(x => x.ItemsServed).FirstOrDefault();
                return GetItemsServed;
            }
        }


        public void UpdateOrderInfo(Order order, int ServedItems)
        {
            
            order.ItemsServed = ServedItems;
            using (var context = new RMContext())
            {
                context.Orders.Attach(order);
                context.Entry(order).Property(x => x.ItemsServed).IsModified = true;
                context.SaveChanges();
            }
        }

        public void UpdateOrder(Order order)
        {
            using (var context = new RMContext())
            {
                context.Entry(order).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


      
        public void DeleteOrder(int ID)
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
