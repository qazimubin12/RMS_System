using Newtonsoft.Json;
using OfficeOpenXml;
using RMS_System.Entities;
using RMS_System.Services;
using RMS_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Mvc;

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
                if (roles[0] == "Kitchen Staff")
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
                list.Add(new OrderWiseData { OrderCount = ordercount, CashRevenue = cash, CardRevenue = card, TotalRevenue = total, OrderDate = DateTime.Parse(item.ToString()) });
            }
            model.OrderWiseData = list;


            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Revenue Report");
            var Config = ConfigurationServices.Instance.GetConfig();
            ws.Cells["A1"].Value = Config.HotelName;

            ws.Cells["A4"].Value = "Date";
            ws.Cells["B4"].Value = "Number of Sessions";
            ws.Cells["C4"].Value = "Cash Revenue";
            ws.Cells["D4"].Value = "Card Revenue";
            ws.Cells["E4"].Value = "Total Revenue";

            int rowStart = 4;

            foreach (var item in list)
            {
                if(item.CardRevenue.ToString() != "0")
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));
                }
                else if(item.CashRevenue.ToString() != "0")
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.OrderDate;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.OrderCount;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.CashRevenue;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.CardRevenue;
                ws.Cells[string.Format("E{0}", rowStart)].Value =  item.TotalRevenue;

                rowStart++;
            }
            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
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
            list.Add(new OrderWiseData { OrderCount = ordercount, CashRevenue = cash, CardRevenue = card, TotalRevenue = total, OrderDate = DateTime.Now });
            model.OrderWiseData = list;
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
                list.Add(new OrderWiseData { OrderCount = ordercount, CashRevenue = cash, CardRevenue = card, TotalRevenue = total, OrderDate = DateTime.Parse(item.ToString()) });
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

                model.Orders = OrderServices.Instance.GetOrdersInKitchen();

                return View(model);
            }
        }

        public ActionResult KitchenOrderDashboard(KitchenDashboardViewModel model)
        {
            model.Orders = OrderServices.Instance.GetOrdersInKitchen();
            model.Entries = TableEntryServices.Instance.GetNonDispatchedTableEntries();
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
                if (model.SelectedTableName.TableStatus == "Waiting for Billing")
                {
                    return RedirectToAction("WaiterApp", "Home");
                }
                Session["Table"] = model.SelectedTableName.ID;
                Session["TableName"] = model.SelectedTableName.TableName;
                model.WaiterName = UsersService.Instance.GetUserName(Session["UserName"].ToString());
                model.MenuItmsCategories = MenuItemServices.Instance.GetAllCategories();
                model.MenuItems = MenuItemServices.Instance.GetMenuItems(null, Category);
                model.OrderedQuantity = 0;
                if (Category == null)
                {
                    return View("GoToFoodEntry", model);
                }
                else
                {
                    return View("GoToFoodEntry", model);
                }
            }

        }

        [HttpGet]
        public ActionResult ShowCart()
        {
            FoodEntryViewModel model = new FoodEntryViewModel();
            model.Entries = TableEntryServices.Instance.GetTableEntries(Session["TableName"].ToString());
            model.TableStatus = TableServices.Instance.GetTableStatus(int.Parse(Session["Table"].ToString()));
            model.SelectedTableName = int.Parse(Session["Table"].ToString());
            model.SessionStatus = TableServices.Instance.GetTableSessionStatus(model.SelectedTableName);
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
            model.SessionStatus = TableServices.Instance.GetTableSessionStatus(model.SelectedTableName);

            model.SelectedTableName = int.Parse(Session["Table"].ToString());


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
            model.SessionStatus = TableServices.Instance.GetTableSessionStatus(model.SelectedTableName);

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
            model.Entries = TableEntryServices.Instance.GetTableEntries(Session["TableName"].ToString());
            model.TableStatus = TableServices.Instance.GetTableStatus(int.Parse(Session["Table"].ToString()));
            model.SessionStatus = TableServices.Instance.GetTableSessionStatus(model.SelectedTableName);


            var order = new Order();
            order.TableName = Session["TableName"].ToString();
            order.WaiterName = Session["UserName"].ToString();
            order.GrandTotal = grandtotal;
            order.OrderedItems = OrderedItems.Count;
            order.ItemsServed = 0;
            order.OrderDate = DateTime.Now;
            order.PaymentStatus = "Order Placed";
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