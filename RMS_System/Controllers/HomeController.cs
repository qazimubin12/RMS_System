using Newtonsoft.Json;
using RMS_System.Entities;
using RMS_System.Services;
using RMS_System.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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
            var Orders = OrderServices.Instance.GetOrders(date);
            foreach (var item in Orders)
            {
                dataPoints.Add(new DataPoint(item.OrderDate, item.GrandTotal));
            }
            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints, _jsonSetting);
            return View(model);
         
        }

      
      

        [Obsolete]
        public ActionResult AdminDashboard(AdminViewModel model)
        {
            model.Orders = OrderServices.Instance.GetOrders(DateTime.Now);
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
                if(model.SelectedTableName.TableStatus == "Waiting for Billing")
                {
                    return RedirectToAction("WaiterApp", "Home");
                }
                Session["Table"] = model.SelectedTableName.ID;
                Session["TableName"] = model.SelectedTableName.TableName;
                model.WaiterName = UsersService.Instance.GetUserName(Session["UserName"].ToString());
                model.MenuItmsCategories = MenuItemServices.Instance.GetAllCategories();
                model.MenuItems = MenuItemServices.Instance.GetMenuItems(null,Category);
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
     

            return PartialView("TableFoodEntry",model); 
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
            var OrderedItems =  TableEntryServices.Instance.GetTableEntries(Session["TableName"].ToString());
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


            int ItemsServed =  TableServices.Instance.GetItemsServed(Table);
            ItemsServed++;
            TableServices.Instance.UpdateTableInfo(ActualCompleteTable, ItemsServed);

            int FinalItemsServed = TableServices.Instance.GetItemsServed(Table);
            int FinalItemsOrdered = TableServices.Instance.GetItemsOrdered(Table);

            if(FinalItemsOrdered == FinalItemsServed)
            {
                TableServices.Instance.UpdateTableInfo(ActualCompleteTable, "Order Delivered", "Active");
            }

            var ActualCompleteOrder = OrderServices.Instance.GetOrderOfOrderPlaced(Table);

            int ItemsServedForOrder = OrderServices.Instance.GetItemsServedForOrder(Table,ActualCompleteOrder.ID);
            int ItemsOrderedForOrder = OrderServices.Instance.GetItemsOrderedForOrder(Table,ActualCompleteOrder.ID);
            ItemsServedForOrder++;


            OrderServices.Instance.UpdateOrderInfo(ActualCompleteOrder, ItemsServedForOrder);
            if(ItemsServedForOrder == ItemsOrderedForOrder)
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
                OrderServices.Instance.UpdateOrderDiscount(Order, Discount, DiscountPercentage,UpdatedGrandTotal);
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
            TableServices.Instance.UpdateTableInfo(Table,"Waiting for Billing", "Non Active");

            var Date = DateTime.Now;
            var OrderID = OrderServices.Instance.GetOrderOfOrderDeliverted(Table.TableName);
            OrderServices.Instance.UpdateOrderInfo(OrderID, "Waiting for Billing");

            var Config =  ConfigurationServices.Instance.GetConfig();
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
            model.TableEntries = TableEntryServices.Instance.GetTableEntriesForBilling(model.Table.TableName,ID);
            model.CGST = BillServices.Instance.GetCGST(model.OrderID);
            model.SGST = BillServices.Instance.GetSGST(model.OrderID);
            return View("ShowReceipt", model);
        }



        public ActionResult CompleteOrder(int ID,string Method)
        {
            //Update Table Info and Order Payment Status
            var Order = OrderServices.Instance.GetOrder(ID);
            var Table = TableServices.Instance.GetTable(Order.TableName);
            OrderServices.Instance.UpdateOrderInfo(Order, "Payment Done",Method);
            TableServices.Instance.UpdateTableInfo(Table, "Fresh Table", 0, 0, "Non Active", null);


            BillViewModel model = new BillViewModel();
            model.TableEntries = TableEntryServices.Instance.GetTableEntries();
            model.Order = OrderServices.Instance.GetOrders();
            model.configuration = ConfigurationServices.Instance.GetConfig();
            return PartialView("BillingOrderDashboard",model);
        }

    }
}