    using Newtonsoft.Json;
using RMS_System.Entities;
using RMS_System.Services;
using RMS_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS_System.Controllers
{
    public class HomeController : Controller
    {
        bool Adminrole = false;
        bool WaiterRole = false;
        bool KitchenRole = false;
        public void CheckRoleForAdmin()
        {
            Adminrole = false;
            if (Session["UserName"] == null)
            {
                Adminrole = false;
            }
            string[] roles = System.Web.Security.Roles.GetRolesForUser(Convert.ToString(Session["UserName"]));
            if (roles.Length != 0)
            {
                if (roles[0] == "Admin")
                {
                    Adminrole = true;
                }
                else
                {
                    Adminrole = false;
                }
            }
            else
            {
                Adminrole = false;
            }
        }
        public void CheckRoleForWaiter()
        {
            WaiterRole = false;
            if (Session["UserName"] == null)
            {
                WaiterRole = false;
            }
            string[] roles = System.Web.Security.Roles.GetRolesForUser(Convert.ToString(Session["UserName"]));
            if (roles.Length != 0)
            {
                if (roles[0] == "Waiter")
                {
                    WaiterRole = true;
                }
                else
                {
                    WaiterRole = false;
                }
            }
            else
            {
                WaiterRole = false;
            }
        }
        public void CheckRoleForKitchenStaff()
        {
            KitchenRole = false;
            if (Session["UserName"] == null)
            {
                KitchenRole = false;
            }
            string[] roles = System.Web.Security.Roles.GetRolesForUser(Convert.ToString(Session["UserName"]));
            if (roles.Length != 0)
            {
                if (roles[0] == "Kitchen Staff" || roles[0]=="Kitchen Master")
                {
                    KitchenRole = true;
                }
                else
                {
                    KitchenRole = false;
                }
            }
            else
            {
                KitchenRole = false;
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        [Obsolete]
        [HttpPost]
        public void Export(string date)
        {
            DateTime myStartDate = DateTime.Now;
            DateTime myEndDate = DateTime.Now;
            string[] datedata = date.Split(' ');
            string startDate = datedata[0];
            string endDate = datedata[2];
            myStartDate = DateTime.Parse(startDate);
            myEndDate = DateTime.Parse(endDate);
            AdminViewModel model = new AdminViewModel();
            var list = new List<OrderWiseData>();
            model.RevenueOrders = OrderServices.Instance.GetOrdersReportDatesOnly(myStartDate, myEndDate);
            foreach (var item in model.RevenueOrders)
            {
                int ordercount = OrderServices.Instance.NoOfSessionRegardingGivenDate(DateTime.Parse(item.ToString()));
                double cash = OrderServices.Instance.GetCashRevenueOfGivenDate(DateTime.Parse(item.ToString()));
                double card = OrderServices.Instance.GetCardRevenueOfGivenDate(DateTime.Parse(item.ToString()));
                double total = OrderServices.Instance.GetTotalRevenueOfGivenDate(DateTime.Parse(item.ToString()));
                list.Add(new OrderWiseData { OrderCount = ordercount, CashRevenue = cash, CardRevenue = card, TotalRevenue = total, OrderDate = DateTime.Parse(item.ToString()).ToShortDateString() });
            }
            model.OrderWiseData = list;

            var gv = new GridView();
            gv.DataSource = list;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=RevenueReport.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();



        }


        [Obsolete]
        [HttpPost]
        public void ExportDishWise(string date)
        {
            AdminViewModel model = new AdminViewModel();
            DateTime myStartDate = DateTime.Now;
            DateTime myEndDate = DateTime.Now;
            string[] datedata = date.Split(' ');
            string startDate = datedata[0];
            string endDate = datedata[2];
            myStartDate = DateTime.Parse(startDate);
            myEndDate = DateTime.Parse(endDate);


            var DishesName = TableEntryServices.Instance.GetTableEntriesDishWiseReport(myStartDate, myEndDate);
            var list = new List<DishWiseData>();
            foreach (var item in DishesName)
            {
                int totalcount = TableEntryServices.Instance.GetNoFoodServedReport(myStartDate, myEndDate, item);
                double MenuItemRate = MenuItemServices.Instance.GetMenuItemPrice(item);
                double revenue = totalcount * MenuItemRate;
                list.Add(new DishWiseData { ItemName = item, OrderCount = totalcount, Revenue = revenue, Date= Convert.ToString(myStartDate.ToShortDateString() +"-"+ myEndDate.ToShortDateString()) });
            }

            var gv = new GridView();
            gv.DataSource = list;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DishWiseReport.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();



        }

        [HttpPost]
        [Obsolete]
        public ActionResult AdminDashboard(DateTime date)
        {

            Session["ProvidedDate"] = date.ToShortDateString();
            AdminViewModel model = new AdminViewModel();
            model.Orders = OrderServices.Instance.GetOrders(date);
            model.TotalRevenueInfoBox = OrderServices.Instance.GetTotalRevenueOfGivenDate(date);
            model.CashRevenueInfoBox = OrderServices.Instance.GetCashRevenueOfGivenDate(date);
            model.CardRevenueInfoBox = OrderServices.Instance.GetCardRevenueOfGivenDate(date);
            model.NoOfSessions = OrderServices.Instance.NoOfSessionRegardingGivenDate(date);
            model.date = date;
            List<DataPoint> dataPoints = new List<DataPoint>();
            List<DataPoint2> dataPoint2 = new List<DataPoint2>();
            List<DataPoint3> dataPoint3 = new List<DataPoint3>();
            var Orders = OrderServices.Instance.GetTop5Orders(date);
            string tempdata = "";
            bool checkiteminlist = false;
            List<string> tempdata2 = new List<string>();
            foreach (var item in Orders)
            {
                dataPoints.Add(new DataPoint(item.OrderDate, item.GrandTotal));
                var Count = OrderServices.Instance.GetWaiterNameWithNumberOfOrderTheyServed(date, item.WaiterName);
                if (item.WaiterName == tempdata)
                {
                    continue;
                }
                else
                {
                    dataPoint2.Add(new DataPoint2(item.WaiterName, Count));
                }
                tempdata = item.WaiterName;
            }
            var TableEntries = TableEntryServices.Instance.GetTableEntries(date);
            foreach (var item in TableEntries)
            {
                var Count = TableEntryServices.Instance.GetNoFoodServed(date, item.FoodItem);
                foreach (var item2 in tempdata2)
                {
                    if (item2 == item.FoodItem)
                    {
                        checkiteminlist = true;
                        break;
                    }
                    else
                    {
                        checkiteminlist = false;
                    }
                }
                if (checkiteminlist == true)
                {
                    continue;
                }
                else
                {
                    tempdata2.Add(item.FoodItem);
                    dataPoint3.Add(new DataPoint3(item.FoodItem, Count));
                }
            }
            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints, _jsonSetting);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoint2, _jsonSetting);
            ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoint3, _jsonSetting);

            var DishesName = TableEntryServices.Instance.GetTableEntriesDishWise(date);

            var list = new List<DishWiseData>();
            foreach (var item in DishesName)
            {
                int totalcount = TableEntryServices.Instance.GetNoFoodServed(date, item);
                double MenuItemRate = MenuItemServices.Instance.GetMenuItemPrice(item);
                double revenue = totalcount * MenuItemRate;
                list.Add(new DishWiseData { ItemName = item, OrderCount = totalcount, Revenue = revenue });
            }
            model.DishWiseData = list;

            return View(model);

        }


        [HttpGet]
        [Obsolete]
        public ActionResult RevenueReport(AdminViewModel model)
        {
            model.Orders = OrderServices.Instance.GetOrders(DateTime.Now);
            List<DataPoint> dataPoints = new List<DataPoint>();
            model.TotalRevenueInfoBox = OrderServices.Instance.GetTotalRevenueOfGivenDate(DateTime.Now);
            model.CashRevenueInfoBox = OrderServices.Instance.GetCashRevenueOfGivenDate(DateTime.Now);
            model.CardRevenueInfoBox = OrderServices.Instance.GetCardRevenueOfGivenDate(DateTime.Now);
            model.NoOfSessions = OrderServices.Instance.NoOfSessionRegardingGivenDate(DateTime.Now);
            model.RevenueOrders = OrderServices.Instance.GetOrdersReportDatesOnly(DateTime.Now);
            foreach (var item in model.Orders)
            {
                dataPoints.Add(new DataPoint(item.OrderDate, item.GrandTotal));
            }
            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            ViewBag.dataPoints = JsonConvert.SerializeObject(dataPoints, _jsonSetting);

            var list = new List<OrderWiseData>();

            int ordercount = OrderServices.Instance.NoOfSessionRegardingGivenDate(DateTime.Now);
            double cash = OrderServices.Instance.GetCashRevenueOfGivenDate(DateTime.Now);
            double card = OrderServices.Instance.GetCardRevenueOfGivenDate(DateTime.Now);
            double total = OrderServices.Instance.GetTotalRevenueOfGivenDate(DateTime.Now);
            model.ProvidedDate = DateTime.Now.ToString();
            list.Add(new OrderWiseData { OrderCount = ordercount, CashRevenue = cash, CardRevenue = card, TotalRevenue = total, OrderDate = DateTime.Now.ToShortDateString() });
            model.OrderWiseData = list;
            return View(model);
        }


        [HttpGet]
        [Obsolete]
        public ActionResult DishWiseReport(AdminViewModel model)
        {
            model.Orders = OrderServices.Instance.GetOrders(DateTime.Now);
            var DishesName = TableEntryServices.Instance.GetTableEntriesDishWise(DateTime.Now);
            var list = new List<DishWiseData>();
            foreach (var item in DishesName)
            {
                int totalcount = TableEntryServices.Instance.GetNoFoodServed(DateTime.Now, item);
                double MenuItemRate = MenuItemServices.Instance.GetMenuItemPrice(item);
                double revenue = totalcount * MenuItemRate;
                list.Add(new DishWiseData { ItemName = item, OrderCount = totalcount, Revenue = revenue,StartDate = DateTime.Now.ToShortDateString(),EndDate = DateTime.Now.ToShortDateString() });
            }

            model.DishWiseData = list;
            return View(model);
        }

   




        [HttpGet]
        [Obsolete]
        public ActionResult CancelledOrders(AdminViewModel model)
        {
            model.CancelledOrders = CancelledOrderServices.Instance.GetCancelledOrders(DateTime.Now);
            return View(model);
        }


        [HttpPost]
        [Obsolete]
        public ActionResult CancelledOrders(string date)
        {
            AdminViewModel model = new AdminViewModel();
            DateTime myStartDate = DateTime.Now;
            DateTime myEndDate = DateTime.Now;
            string[] datedata = date.Split(' ');
            string startDate = datedata[0];
            string endDate = datedata[2];
            myStartDate = DateTime.Parse(startDate);
            myEndDate = DateTime.Parse(endDate);
            model.ProvidedDate = date;
            model.CancelledOrders = CancelledOrderServices.Instance.GetCancelledOrdersReport(myStartDate,myEndDate);
            return View(model);
        }

        [HttpPost]
        [Obsolete]
        public ActionResult DishWiseReport(string date)
        {
            AdminViewModel model = new AdminViewModel();
            DateTime myStartDate = DateTime.Now;
            DateTime myEndDate = DateTime.Now;
            string[] datedata = date.Split(' ');
            string startDate = datedata[0];
            string endDate = datedata[2];
            myStartDate = DateTime.Parse(startDate);
            myEndDate = DateTime.Parse(endDate);


            var DishesName = TableEntryServices.Instance.GetTableEntriesDishWiseReport(myStartDate,myEndDate);
            var list = new List<DishWiseData>();
            foreach (var item in DishesName)
            {
                int totalcount = TableEntryServices.Instance.GetNoFoodServedReport(myStartDate,myEndDate, item);
                double MenuItemRate = MenuItemServices.Instance.GetMenuItemPrice(item);
                double revenue = totalcount * MenuItemRate;
                list.Add(new DishWiseData { ItemName = item, OrderCount = totalcount, Revenue = revenue,StartDate =myStartDate.ToShortDateString(),EndDate = myEndDate.ToShortDateString() });
            }


            model.ProvidedDate = date;
            model.DishWiseData = list;
            return View(model);
        }


        [HttpPost]
        [Obsolete]
        public ActionResult RevenueReport(string date)
        {
            AdminViewModel model = new AdminViewModel();

            DateTime myStartDate = DateTime.Now;
            DateTime myEndDate = DateTime.Now;
            string[] datedata = date.Split(' ');
            string startDate = datedata[0];
            string endDate = datedata[2];
            myStartDate = DateTime.Parse(startDate);
            myEndDate = DateTime.Parse(endDate);
            model.Orders = OrderServices.Instance.GetOrdersReport(myStartDate, myEndDate);
            model.RevenueOrders = OrderServices.Instance.GetOrdersReportDatesOnly(myStartDate, myEndDate);
            model.TotalRevenueInfoBox = OrderServices.Instance.GetTotalRevenueOfGivenDateReport(myStartDate, myEndDate);
            model.CashRevenueInfoBox = OrderServices.Instance.GetCashRevenueOfGivenDateReport(myStartDate, myEndDate);
            model.CardRevenueInfoBox = OrderServices.Instance.GetCardRevenueOfGivenDateReport(myStartDate, myEndDate);
            model.NoOfSessions = OrderServices.Instance.NoOfSessionRegardingGivenDateReport(myStartDate, myEndDate);
            model.ProvidedDate = date;
            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (var item in model.Orders)
            {
                dataPoints.Add(new DataPoint(item.OrderDate, item.GrandTotal));
            }
            var list = new List<OrderWiseData>();
            foreach (var item in model.RevenueOrders)
            {
                int ordercount = OrderServices.Instance.NoOfSessionRegardingGivenDate(DateTime.Parse(item.ToString()));
                double cash = OrderServices.Instance.GetCashRevenueOfGivenDate(DateTime.Parse(item.ToString()));
                double card = OrderServices.Instance.GetCardRevenueOfGivenDate(DateTime.Parse(item.ToString()));
                double total = OrderServices.Instance.GetTotalRevenueOfGivenDate(DateTime.Parse(item.ToString()));
                list.Add(new OrderWiseData { OrderCount = ordercount, CashRevenue = cash, CardRevenue = card, TotalRevenue = total, OrderDate = DateTime.Parse(item.ToString()).ToShortDateString() });
            }
            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            ViewBag.dataPoints = JsonConvert.SerializeObject(dataPoints, _jsonSetting);
            model.OrderWiseData = list;
            return View(model);
        }


        [Obsolete]
        public ActionResult AdminDashboard(AdminViewModel model)
        {
            model.Orders = OrderServices.Instance.GetOrders(DateTime.Now);
            var DishesName = TableEntryServices.Instance.GetTableEntriesDishWise(DateTime.Now);
            var list = new List<DishWiseData>();
            model.date = DateTime.Now;
            foreach (var item in DishesName)
            {
                int totalcount = TableEntryServices.Instance.GetNoFoodServed(DateTime.Now, item);
                double MenuItemRate = MenuItemServices.Instance.GetMenuItemPrice(item);
                double revenue = totalcount * MenuItemRate;
                list.Add(new DishWiseData { ItemName = item, OrderCount = totalcount, Revenue = revenue });
            }

            model.DishWiseData = list;
            return View(model);
        }


        public ActionResult KitchenDashboard(KitchenDashboardViewModel model)
        {
            CheckRoleForKitchenStaff();
            if (KitchenRole == false)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                string[] roles = System.Web.Security.Roles.GetRolesForUser(Convert.ToString(Session["UserName"]));
                model.Orders = OrderServices.Instance.GetOrdersInKitchen();
                model.Role = roles[0];
                return View(model);
            }
        }

        public ActionResult KitchenOrderDashboard(KitchenDashboardViewModel model)
        {
            string[] roles = System.Web.Security.Roles.GetRolesForUser(Convert.ToString(Session["UserName"]));
            model.Orders = OrderServices.Instance.GetOrdersInKitchen();
            model.Entries = TableEntryServices.Instance.GetNonDispatchedTableEntries();
            model.Role = roles[0];
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult WaiterApp(WaiterAppViewModel model)
        {
            CheckRoleForWaiter();
            if (WaiterRole == false)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                model.Tables = TableServices.Instance.GetTables();
                model.MenuItems = MenuItemServices.Instance.GetMenuItems();
                return View(model);
            }
        }

        public ActionResult BillingDashboard(BillViewModel model)
        {
            model.TableEntries = TableEntryServices.Instance.GetTableEntries();
            model.Order = OrderServices.Instance.GetOrders();
            model.configuration = ConfigurationServices.Instance.GetConfig();
            
            return View(model);
        }


        public ActionResult BillingOrderDashboard(BillViewModel model)
        {
            model.TableEntries = TableEntryServices.Instance.GetTableEntries();
            model.Order = OrderServices.Instance.GetOrders();
            model.configuration = ConfigurationServices.Instance.GetConfig();
         
            return PartialView(model);
        }


        [HttpGet]
        public ActionResult GoToFoodEntry(int ID, string Category = null)
        {
            CheckRoleForWaiter();
            if (WaiterRole == false)
            {
                return RedirectToAction("Index", "Login");
            }

            else
            {

                TableEntryForFoodViewModel model = new TableEntryForFoodViewModel();
                model.SelectedTableName = TableServices.Instance.GetTable(ID);
                var SomeInfoForTable = TableServices.Instance.GetTable(ID);
                if (model.SelectedTableName.TableStatus == "Waiting for Billing")
                {
                    return RedirectToAction("WaiterApp", "Home");
                }
                Session["Table"] = model.SelectedTableName.ID;
                Session["TableName"] = model.SelectedTableName.TableName;
                model.WaiterName = UsersService.Instance.GetUserName(Session["UserName"].ToString());
                if (SomeInfoForTable.ServedBy != null)
                {
                    if (SomeInfoForTable.ServedBy != model.WaiterName.UserName)
                    {
                        return RedirectToAction("WaiterApp", "Home");

                    }
                }
                model.MenuItmsCategories = MenuItemServices.Instance.GetAllCategories();
                if (Category == null)
                {
                    model.MenuItems = MenuItemServices.Instance.GetMenuItems(null);
                }
                else
                {
                    model.MenuItems = MenuItemServices.Instance.GetMenuItems(null, Category);
                }
                model.OrderedQuantity = 0;

                return View(model);
            }

        }

        [HttpGet]
        public ActionResult ShowCart()
        {
            FoodEntryViewModel model = new FoodEntryViewModel();
            model.Entries = TableEntryServices.Instance.GetTableEntries(Session["TableName"].ToString());
            model.TableStatus = TableServices.Instance.GetTableStatus(int.Parse(Session["Table"].ToString()));
            model.SelectedTableName = int.Parse(Session["Table"].ToString());
            model.SessionStatus = TableServices.Instance.GetTableSessionStatus(Session["TableName"].ToString());
           
            var Table = TableServices.Instance.GetTable(model.SelectedTableName);
            model.Table = Table;
            if(Table.OrderItems == Table.ItemsServed)
            {
                model.OrderSatisfied = true;
            }
            else
            {
                model.OrderSatisfied = false;
            }
            return PartialView("TableFoodEntry", model);
        }


        [HttpGet]
        public ActionResult TableFoodEntry(int ID, int Quantity)
        {
            var entry = new TableEntry();
            entry.FoodItem = MenuItemServices.Instance.GetMenuItemName(ID);
            entry.Quantity = Quantity;
            entry.TableName = Session["TableName"].ToString();
            Double price = MenuItemServices.Instance.GetMenuItemPrice(ID);
            entry.ProductTotal = int.Parse(price.ToString()) * Quantity;
            entry.OrderedTime = DateTime.Now;
            TableEntryServices.Instance.SaveTableEntry(entry);

            int TABLEID = int.Parse(Session["Table"].ToString());
            FoodEntryViewModel model = new FoodEntryViewModel();
            model.Entries = TableEntryServices.Instance.GetTableEntries(Session["TableName"].ToString());
            model.TableStatus = TableServices.Instance.GetTableStatus(int.Parse(Session["Table"].ToString()));
            model.SessionStatus = TableServices.Instance.GetTableSessionStatus(Session["TableName"].ToString());
            model.SelectedTableName = int.Parse(Session["Table"].ToString());
            var Table = TableServices.Instance.GetTable(model.SelectedTableName);
            model.Table = Table;
            if (Table.OrderItems == Table.ItemsServed)
            {
                model.OrderSatisfied = true;
            }
            else
            {
                model.OrderSatisfied = false;
            }

            return PartialView("TableFoodEntry", model);
        }



        [HttpPost]
        public ActionResult Delete(int ID)
        {
            TableEntryServices.Instance.DeleteTableEntry(ID);

            FoodEntryViewModel model = new FoodEntryViewModel();
            model.Entries = TableEntryServices.Instance.GetTableEntries(Session["TableName"].ToString());
            model.SelectedTableName = int.Parse(Session["Table"].ToString());
            model.TableStatus = TableServices.Instance.GetTableStatus(int.Parse(Session["Table"].ToString()));
            model.SessionStatus = TableServices.Instance.GetTableSessionStatus(Session["TableName"].ToString());
            var Table = TableServices.Instance.GetTable(model.SelectedTableName);
            model.Table = Table;
            if (Table.OrderItems == Table.ItemsServed)
            {
                model.OrderSatisfied = true;
            }
            else
            {
                model.OrderSatisfied = false;
            }
            return PartialView("TableFoodEntry", model);
        }

        public ActionResult UpdateOrder(int ID)
        {
            Double grandtotal = 0;
            var OrderedItems = TableEntryServices.Instance.GetTableEntries(Session["TableName"].ToString());
            foreach (var item in OrderedItems)
            {
                if (item.FoodDispatchedStatus == null)
                {
                    TableEntryServices.Instance.UpdateTableEntryStatus(item, "Not Yet");
                }
                grandtotal += item.ProductTotal;
            }



            TableServices.Instance.UpdateTableInfoForUpdateOrder(ID, "Order Placed", OrderedItems.Count, "Active", Session["UserName"].ToString());
            FoodEntryViewModel model = new FoodEntryViewModel();
            var Table2 = TableServices.Instance.GetTable(Session["TableName"].ToString());
            model.Table = Table2;
            if (Table2.ItemsServed == Table2.OrderItems)
            {
                model.OrderSatisfied = true;
            }
            else
            {
                model.OrderSatisfied = false;
            }

            model.Entries = TableEntryServices.Instance.GetTableEntries(Session["TableName"].ToString());
            model.TableStatus = TableServices.Instance.GetTableStatus(int.Parse(Session["Table"].ToString()));
            model.SessionStatus = TableServices.Instance.GetTableSessionStatus(Session["TableName"].ToString());
            var Table = TableServices.Instance.GetTable(ID);


            var CompleteOrder = OrderServices.Instance.GetOrderForUpdation(Session["TableName"].ToString());
            OrderServices.Instance.UpdateOrderInfo(CompleteOrder, grandtotal, OrderedItems.Count, grandtotal,"Order Placed");
            return PartialView("TableFoodEntry", model);

        }


        public ActionResult ConfirmOrder(int ID)
        {
            Double grandtotal = 0;
            var OrderedItems = TableEntryServices.Instance.GetTableEntries(Session["TableName"].ToString());
            foreach (var item in OrderedItems)
            {
                TableEntryServices.Instance.UpdateTableEntryStatus(item, "Not Yet");
                grandtotal += item.ProductTotal;
            }



            TableServices.Instance.UpdateTableInfo(ID, "Order Placed", OrderedItems.Count, "Active", Session["UserName"].ToString());
            FoodEntryViewModel model = new FoodEntryViewModel();
            var Table2 = TableServices.Instance.GetTable(Session["TableName"].ToString());
            model.Table = Table2;
            if (Table2.ItemsServed == Table2.OrderItems)
            {
                model.OrderSatisfied = true;
            }
            else
            {
                model.OrderSatisfied = false;
            }
            model.Entries = TableEntryServices.Instance.GetTableEntries(Session["TableName"].ToString());
            model.TableStatus = TableServices.Instance.GetTableStatus(int.Parse(Session["Table"].ToString()));
            model.SessionStatus = TableServices.Instance.GetTableSessionStatus(Session["TableName"].ToString());

            var order = new Order();
            order.TableName = Session["TableName"].ToString();
            order.WaiterName = Session["UserName"].ToString();
            order.GrandTotal = grandtotal;
            order.OrderedItems = OrderedItems.Count;
            order.ItemsServed = 0;
            order.OrderDate = DateTime.Now;
            order.PaymentStatus = "Order Placed";
            order.GrossTotal = grandtotal;
            OrderServices.Instance.SaveOrder(order);
            return PartialView("TableFoodEntry", model);


        }


        public ActionResult DispatchEntry(int ID)
        {

            var Entry = TableEntryServices.Instance.GetTableEntry(ID);
            TableEntryServices.Instance.UpdateTableEntryStatus(Entry, "Dispatched");


            var Table = TableEntryServices.Instance.GetTableNameFromEntryID(ID);
            var ActualCompleteTable = TableServices.Instance.GetTable(Table);


            int ItemsServed = TableServices.Instance.GetItemsServed(Table);
            ItemsServed++;
                TableServices.Instance.UpdateTableInfo(ActualCompleteTable, ItemsServed);

            int FinalItemsServed = TableServices.Instance.GetItemsServed(Table);
            int FinalItemsOrdered = TableServices.Instance.GetItemsOrdered(Table);

            if (FinalItemsOrdered == FinalItemsServed)
            {
                TableServices.Instance.UpdateTableInfo(ActualCompleteTable, "Order Delivered", "Active");
            }

            var ActualCompleteOrder = OrderServices.Instance.GetOrderOfOrderPlaced(Table);

            int ItemsServedForOrder = OrderServices.Instance.GetItemsServedForOrder(Table, ActualCompleteOrder.ID);
            int ItemsOrderedForOrder = OrderServices.Instance.GetItemsOrderedForOrder(Table, ActualCompleteOrder.ID);
            ItemsServedForOrder++;


            OrderServices.Instance.UpdateOrderInfo(ActualCompleteOrder, ItemsServedForOrder);
            if (ItemsServedForOrder == ItemsOrderedForOrder)
            {
                OrderServices.Instance.UpdateOrderInfo(ActualCompleteOrder, "Order Delivered");
            }

            KitchenDashboardViewModel model = new KitchenDashboardViewModel();
            model.Orders = OrderServices.Instance.GetOrders();
            model.Entries = TableEntryServices.Instance.GetNonDispatchedTableEntries();

            return PartialView("KitchenOrderDashboard", model);

        }
        


        public ActionResult DeleteOrderEntries(int ID)
        {
            var Table = TableEntryServices.Instance.GetTableNameFromEntryID(ID);
            var ActualCompleteTable = TableServices.Instance.GetTable(Table);

            int FinalItemsOrdered = TableServices.Instance.GetItemsOrdered(Table);
            int FinalItemsServed = TableServices.Instance.GetItemsServed(Table);
            var ActualCompleteOrder = OrderServices.Instance.GetOrderOfOrderPlaced(Table);
            int ItemsOrderedForOrder = OrderServices.Instance.GetItemsOrderedForOrder(Table, ActualCompleteOrder.ID);
            int ItemsServedForOrder = OrderServices.Instance.GetItemsServedForOrder(Table, ActualCompleteOrder.ID);
            ItemsOrderedForOrder--;
            if (ItemsOrderedForOrder == 0 && ItemsServedForOrder == 0)
            {
                var BillListWRTToOrderID = BillServices.Instance.GetBillFor(ActualCompleteOrder.ID);
                foreach (var item in BillListWRTToOrderID)
                {
                    BillServices.Instance.DeleteBill(item.ID);
                }
                OrderServices.Instance.DeleteOrder(ActualCompleteOrder.ID);
                TableServices.Instance.UpdateTableInfo(ActualCompleteTable, "Fresh Table", 0, 0, "Non Active", null);

            }
            else
            {
                OrderServices.Instance.UpdateOrderDeletion(ActualCompleteOrder, ItemsOrderedForOrder);
                FinalItemsOrdered--;
                TableServices.Instance.UpdateTableInfoData(ActualCompleteTable, FinalItemsOrdered);

                if (FinalItemsOrdered == FinalItemsServed)
                {
                    TableServices.Instance.UpdateTableInfo(ActualCompleteTable, "Order Delivered", "Active");
                }

                int FinalItemsOrderedForOrder = OrderServices.Instance.GetItemsOrderedForOrder(Table, ActualCompleteOrder.ID);
                int FinalItemsServedForOrder = OrderServices.Instance.GetItemsServedForOrder(Table, ActualCompleteOrder.ID);

                if (FinalItemsOrderedForOrder == FinalItemsServedForOrder)
                {
                    OrderServices.Instance.UpdateOrderInfo(ActualCompleteOrder, "Order Delivered");
                }

                Double grandtotal = 0;
                var OrderedItems = TableEntryServices.Instance.GetTableEntries(ActualCompleteTable.TableName);
                foreach (var item in OrderedItems)
                {
                    grandtotal += item.ProductTotal;
                }

                OrderServices.Instance.UpdateOrderInfo(ActualCompleteOrder,grandtotal);
            }

            var cancelledOrder = new CancelledOrders();
            cancelledOrder.CancelledDate = DateTime.Now;
            cancelledOrder.ItemName = TableEntryServices.Instance.GetItemNameFromEntryID(ID); 
            cancelledOrder.TableName = Table;
            cancelledOrder.WaiterName = TableServices.Instance.GetTableServedBy(Table);
            CancelledOrderServices.Instance.SaveOrder(cancelledOrder);
            TableEntryServices.Instance.DeleteTableEntry(ID);
            KitchenDashboardViewModel model = new KitchenDashboardViewModel();
            model.Orders = OrderServices.Instance.GetOrders();
            model.Entries = TableEntryServices.Instance.GetNonDispatchedTableEntries();

            return PartialView("KitchenOrderDashboard", model);

        }
        public ActionResult UpdateDiscount(string TableName, Double Discount, Double DiscountPercentage)
        {
            if (Discount != 0)
            {
                var Order = OrderServices.Instance.GetOrderByTable(TableName);

                var BillsBasedOnOrderId = BillServices.Instance.GetBillFor(Order.ID);
                Double UpdatedGrandTotal = 0;
                foreach (var item in BillsBasedOnOrderId)
                {
                    var Entry = TableEntryServices.Instance.GetTableEntry(item.EntriesID);
                    UpdatedGrandTotal += Entry.ProductTotal;
                }

                Double CGST = BillServices.Instance.GetCGST(Order.ID);
                Double SGST = BillServices.Instance.GetSGST(Order.ID);
                UpdatedGrandTotal = UpdatedGrandTotal + CGST + SGST;
                UpdatedGrandTotal -= Discount;
                OrderServices.Instance.UpdateOrderDiscount(Order, Discount, DiscountPercentage, UpdatedGrandTotal);
            }
            else
            {
                var Order = OrderServices.Instance.GetOrderByTable(TableName);
                var BillsBasedOnOrderId = BillServices.Instance.GetBillFor(Order.ID);
                Double UpdatedGrandTotal = 0;
                foreach (var item in BillsBasedOnOrderId)
                {
                    var Entry = TableEntryServices.Instance.GetTableEntry(item.EntriesID);
                    UpdatedGrandTotal += Entry.ProductTotal;
                }
                Double CGST = BillServices.Instance.GetCGST(Order.ID);
                Double SGST = BillServices.Instance.GetSGST(Order.ID);
                UpdatedGrandTotal = UpdatedGrandTotal + CGST + SGST;
                OrderServices.Instance.UpdateOrderDiscount(Order, Discount, DiscountPercentage, UpdatedGrandTotal);

            }
            return PartialView("BillingOrderDashboard");
        }

        public ActionResult CloseSession(int ID) //table ID
        {
            var Table = TableServices.Instance.GetTable(ID);
            TableServices.Instance.UpdateTableInfo(Table, "Waiting for Billing", "Non Active");

            var Date = DateTime.Now;
            var OrderID = OrderServices.Instance.GetOrderOfOrderDeliverted(Table.TableName);
            OrderServices.Instance.UpdateOrderInfo(OrderID, "Waiting for Billing");

            var Config = ConfigurationServices.Instance.GetConfig();
            Double SGST = Config.SGST;
            Double CGST = Config.CGST;
            Double CGSTAmount = 0;
            Double SGSTAmount = 0;

            Double GrandTotal = OrderID.GrandTotal;
            CGSTAmount = GrandTotal / 100;
            CGSTAmount *= CGST;

            SGSTAmount = GrandTotal / 100;
            SGSTAmount *= SGST;

            var EntriesList = TableEntryServices.Instance.GetTableEntries(Table.TableName);
            var bill = new Bill();
            bill.OrderDate = Date;
            bill.OrderID = OrderID.ID;

            foreach (var item in EntriesList)
            {
                TableEntryServices.Instance.UpdateTableEntryStatus(item, true);
                bill.EntriesID = item.ID;
                bill.CGST = CGSTAmount;
                bill.SGST = SGSTAmount;
                BillServices.Instance.SaveBill(bill);

            }

            return RedirectToAction("WaiterApp", "Home");

        }



        public ActionResult ShowReceipt(int ID)//Order ID
        {
            ReceiptViewModel model = new ReceiptViewModel();
            model.OrderID = ID;
            model.Order = OrderServices.Instance.GetOrder(ID);
            var tablename = OrderServices.Instance.GetTableNameFromOrderID(ID);
            model.Table = TableServices.Instance.GetTable(tablename);
            model.configuration = ConfigurationServices.Instance.GetConfig();
            model.TableEntries = TableEntryServices.Instance.GetTableEntriesForBilling(model.Table.TableName, ID);
            model.CGST = BillServices.Instance.GetCGST(model.OrderID);
            model.SGST = BillServices.Instance.GetSGST(model.OrderID);
            model.GrandTotal = model.Order.GrossTotal + model.CGST + model.SGST;
            return View("ShowReceipt", model);
        }



        public ActionResult CompleteOrder(int ID, string Method)
        {
            //Update Table Info and Order Payment Status
            var Order = OrderServices.Instance.GetOrder(ID);
            var Table = TableServices.Instance.GetTable(Order.TableName);
            OrderServices.Instance.UpdateOrderInfo(Order, "Payment Done", Method);
            TableServices.Instance.UpdateTableInfo(Table, "Fresh Table", 0, 0, "Non Active", null);


            BillViewModel model = new BillViewModel();
            model.TableEntries = TableEntryServices.Instance.GetTableEntries();
            model.Order = OrderServices.Instance.GetOrders();
            model.configuration = ConfigurationServices.Instance.GetConfig();
            return PartialView("BillingOrderDashboard", model);
        }

    }
}