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

        [Obsolete]
        public List<Order> GetOrders(DateTime date)
        {
            DateTime date2 = DateTime.Parse(date.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                return context.Orders.Where(x=> EntityFunctions.TruncateTime(x.OrderDate) == date2).ToList();
            }
        }

        [Obsolete]
        public List<Order> GetOrdersReport(DateTime startdate, DateTime enddate)
        {
            DateTime date1 = DateTime.Parse(startdate.ToString("yyyy-MM-dd"));
            DateTime date2 = DateTime.Parse(enddate.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) >= date1 && EntityFunctions.TruncateTime(x.OrderDate) <= date2).ToList();
            }
        }


        [Obsolete]
        public List<DateTime?> GetOrdersReportDatesOnly(DateTime startdate, DateTime enddate)
        {
            DateTime date1 = DateTime.Parse(startdate.ToString("yyyy-MM-dd"));
            DateTime date2 = DateTime.Parse(enddate.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) >= date1 && EntityFunctions.TruncateTime(x.OrderDate) <= date2).Select(x=>EntityFunctions.TruncateTime(x.OrderDate)).Distinct().ToList();
            }
        }


        [Obsolete]
        public List<DateTime?> GetOrdersReportDatesOnly(DateTime date)
        {
            DateTime date1 = DateTime.Parse(date.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) == date1 ).Select(x => EntityFunctions.TruncateTime(x.OrderDate)).Distinct().ToList();
            }
        }






        [Obsolete]
        public List<Order> GetTop5Orders(DateTime date)
        {
            DateTime date2 = DateTime.Parse(date.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) == date2).Take(5).ToList();
            }
        }






        [Obsolete]
        public int GetWaiterNameWithNumberOfOrderTheyServed(DateTime date, string WaiterName)
        {
            DateTime date2 = DateTime.Parse(date.ToString("yyyy-MM-dd"));
            using (var context = new RMContext())
            {
                var result =  context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) == date2 && x.WaiterName == WaiterName).Count();
                return result;
                
            }
        }

       







        public Order GetOrderByTable(string TableName)
        {
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => x.TableName == TableName && x.PaymentStatus == "Waiting for Billing").FirstOrDefault();
            }
        }


        public Order GetOrderOfOrderPlaced(string TableName)
        {
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => x.TableName == TableName &&x.PaymentStatus == "Order Placed").FirstOrDefault();
            }
        }


        public Order GetOrderOfOrderDeliverted(string TableName)
        {
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => x.TableName == TableName && x.PaymentStatus == "Order Delivered").FirstOrDefault();
            }
        }




        public Order GetOrder(int ID)
        {
            using (var context = new RMContext())
            {
                return context.Orders.Find(ID);
            }
        }

        public Order GetOrder(string TableName)
        {
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => x.TableName == TableName).FirstOrDefault();
            }
        }

        public Order GetOrderForUpdation(string TableName)
        {
            using (var context = new RMContext())
            {
                return context.Orders.Where(x => x.TableName == TableName && x.PaidBy == null).FirstOrDefault();
            }
        }

        public int GetItemsServedForOrder(string TableName, int ID)
        {
            using (var context = new RMContext())
            {
                int GetItemsServed = (int)context.Orders.Where(x => x.TableName == TableName && x.ID == ID).Select(x => x.ItemsServed).FirstOrDefault();
                return GetItemsServed;
            }
        }

        public int GetItemsOrderedForOrder(string TableName, int ID)
        {
            using (var context = new RMContext())
            {
                int GetItemsServed = (int)context.Orders.Where(x => x.TableName == TableName && x.ID == ID ).Select(x => x.OrderedItems).FirstOrDefault();
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

        public void UpdateOrderInfo(Order order, Double GrandTotal)
        {

            order.GrandTotal = GrandTotal;
            using (var context = new RMContext())
            {
                context.Orders.Attach(order);
                context.Entry(order).Property(x => x.GrandTotal).IsModified = true;
                context.SaveChanges();
            }
        }


        public void UpdateOrderInfo(Order order, Double GrandTotal,int OrderedItems,Double GrossTotal,string Status)
        {

            order.GrandTotal = GrandTotal;
            order.OrderedItems = OrderedItems;
            order.GrossTotal = GrossTotal;
            order.PaymentStatus = Status;
            using (var context = new RMContext())
            {
                context.Orders.Attach(order);
                context.Entry(order).Property(x => x.GrandTotal).IsModified = true;
                context.Entry(order).Property(x => x.OrderedItems).IsModified = true;
                context.Entry(order).Property(x => x.GrossTotal).IsModified = true;
                context.Entry(order).Property(x => x.GrossTotal).IsModified = true;
                context.SaveChanges();
            }
        }


        public void UpdateOrderDeletion(Order order, int OrderedItems)
        {

            order.OrderedItems = OrderedItems;
            using (var context = new RMContext())
            {
                context.Orders.Attach(order);
                context.Entry(order).Property(x => x.OrderedItems).IsModified = true;
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

                var order = context.Orders.Find(ID);
                context.Orders.Remove(order);
                context.SaveChanges();
            }
        }




        [Obsolete]
        public Double GetTotalRevenueOfGivenDateReport(DateTime startdate, DateTime enddate)
        {
            DateTime date1 = DateTime.Parse(startdate.ToString("yyyy-MM-dd"));
            DateTime date2 = DateTime.Parse(enddate.ToString("yyyy-MM-dd"));

            using (var context = new RMContext())
            {
                Double Revenue = 0;

                var data = context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) >= date1 && EntityFunctions.TruncateTime(x.OrderDate) <= date2).ToList();
                foreach (var item in data)
                {
                    Revenue += item.GrandTotal;
                }
                return Revenue;
            }
        }


        [Obsolete]
        public int NoOfSessionRegardingGivenDateReport(DateTime startdate, DateTime enddate)
        {

            DateTime date1 = DateTime.Parse(startdate.ToString("yyyy-MM-dd"));
            DateTime date2 = DateTime.Parse(enddate.ToString("yyyy-MM-dd"));
      
            using (var context = new RMContext())
            {

                var data = context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) >= date1 && EntityFunctions.TruncateTime(x.OrderDate) <= date2).ToList();

                return data.Count;
            }
        }



        [Obsolete]
        public Double GetCashRevenueOfGivenDateReport(DateTime startdate, DateTime enddate)
        {
            DateTime date1 = DateTime.Parse(startdate.ToString("yyyy-MM-dd"));
            DateTime date2 = DateTime.Parse(enddate.ToString("yyyy-MM-dd"));

            using (var context = new RMContext())
            {
                Double Revenue = 0;

                var data = context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) >= date1 && EntityFunctions.TruncateTime(x.OrderDate) <= date2 && x.PaidBy == "Cash").ToList();
                foreach (var item in data)
                {
                    Revenue += item.GrandTotal;
                }
                return Revenue;
            }
        }

        [Obsolete]
        public Double GetCardRevenueOfGivenDateReport(DateTime startdate, DateTime enddate)
        {

            DateTime date1 = DateTime.Parse(startdate.ToString("yyyy-MM-dd"));
            DateTime date2 = DateTime.Parse(enddate.ToString("yyyy-MM-dd"));

            using (var context = new RMContext())
            {
                Double Revenue = 0;

                var data = context.Orders.Where(x => EntityFunctions.TruncateTime(x.OrderDate) >= date1 && EntityFunctions.TruncateTime(x.OrderDate) <= date2 && x.PaidBy == "Card").ToList();
                foreach (var item in data)
                {
                    Revenue += item.GrandTotal;
                }
                return Revenue;
            }
        }
    }
}
