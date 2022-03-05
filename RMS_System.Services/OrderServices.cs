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

        [Obsolete]
        public Double GetTotalRevenueOfGivenDate(DateTime date)
        {
            
            using (var context = new RMContext())
            {
                Double Revenue = 0;
                
                var data = context.Orders.Where(x=> EntityFunctions.TruncateTime(x.OrderDate) == date).ToList();
                foreach (var item in data)
                {
                    Revenue += item.GrandTotal;
                }
                return Revenue;
            }
        }


        [Obsolete]
        public int NoOfSessionRegardingGivenDate(DateTime date)
        {

            using (var context = new RMContext())
            {
                
                var data = context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) == date).ToList();
               
                return data.Count;
            }
        }



        [Obsolete]
        public Double GetCashRevenueOfGivenDate(DateTime date)
        {

            using (var context = new RMContext())
            {
                Double Revenue = 0;

                var data = context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) == date && x.PaidBy == "Cash").ToList();
                foreach (var item in data)
                {
                    Revenue += item.GrandTotal;
                }
                return Revenue;
            }
        }

        [Obsolete]
        public Double GetCardRevenueOfGivenDate(DateTime date)
        {

            using (var context = new RMContext())
            {
                Double Revenue = 0;

                var data = context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) == date && x.PaidBy == "Card").ToList();
                foreach (var item in data)
                {
                    Revenue += item.GrandTotal;
                }
                return Revenue;
            }
        }
        public List<Order> GetOrders()
        {
            using (var context = new RMContext())
            {
                return context.Orders.Where(x=>x.PaymentStatus == "Waiting for Billing").ToList();
            }
        }


        public List<Order> GetOrdersInKitchen()
        {
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => x.PaymentStatus == "Order Placed").ToList();
            }
        }


        public List<Order> GetOrders(DateTime date)
        {
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => x.OrderDate >= date).ToList();
            }
        }


        public Order GetOrderByTable(string TableName)
        {
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => x.TableName == TableName).FirstOrDefault();
            }
        }

     


        public Order GetOrder(int ID)
        {
            using (var context = new RMContext())
            {
                return context.Orders.Find(ID);
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

        public int GetItemsOrderedForOrder(string TableName)
        {
            using (var context = new RMContext())
            {
                int GetItemsServed = (int)context.Orders.Where(x => x.TableName == TableName).Select(x => x.OrderedItems).FirstOrDefault();
                return GetItemsServed;
            }
        }


        public string GetTableNameFromOrderID(int ID)
        {
            using (var context = new RMContext())
            {
                string table = context.Orders.Where(x => x.ID == ID).Select(x => x.TableName).FirstOrDefault();
                return table;
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

        public void UpdateOrderInfo(Order order, string PaymentStatus, string PaidBy)
        {

            order.PaymentStatus = PaymentStatus;
            order.PaidBy = PaidBy;
            using (var context = new RMContext())
            {
                context.Orders.Attach(order);
                context.Entry(order).Property(x => x.PaymentStatus).IsModified = true;
                context.Entry(order).Property(x => x.PaidBy).IsModified = true;
                context.SaveChanges();
            }
        }



        public void UpdateOrderInfo(Order order, string PaymentStatus)
        {

            order.PaymentStatus = PaymentStatus;
            using (var context = new RMContext())
            {
                context.Orders.Attach(order);
                context.Entry(order).Property(x => x.PaymentStatus).IsModified = true;
                context.SaveChanges();
            }
        }



        public void UpdateOrderDiscount(Order order, Double Discount, Double DiscountPercentage,Double GrandTotal)
        {

            order.Discount = Discount;
            order.DiscountPercentage = DiscountPercentage;
            order.GrandTotal = GrandTotal;
            using (var context = new RMContext())
            {
                context.Orders.Attach(order);
                context.Entry(order).Property(x => x.Discount).IsModified = true;
                context.Entry(order).Property(x => x.DiscountPercentage).IsModified = true;
                context.Entry(order).Property(x => x.GrandTotal).IsModified = true;
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
