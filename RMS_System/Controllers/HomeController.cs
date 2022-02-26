using RMS_System.Entities;
using RMS_System.Services;
using RMS_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult AdminDashboard()
        {
            return View();
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

                model.Orders = OrderServices.Instance.GetOrders();
                model.Entries = TableEntryServices.Instance.GetTableEntries();
                
                return View(model);
            }
        }

        public ActionResult KitchenOrderDashboard(KitchenDashboardViewModel model)
        {
            model.Orders = OrderServices.Instance.GetOrders();
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

        public ActionResult BillingDashboard()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GoToFoodEntry(int ID)
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
                model.MenuItems = MenuItemServices.Instance.GetMenuItems();
                model.OrderedQuantity = 0;
                return View("GoToFoodEntry", model);
            }
           
        }

        [HttpGet]
        public ActionResult ShowCart()
        {
            FoodEntryViewModel model = new FoodEntryViewModel();
            model.Entries = TableEntryServices.Instance.GetTableEntries(Session["TableName"].ToString());
            model.TableStatus = TableServices.Instance.GetTableStatus(int.Parse(Session["Table"].ToString()));
            model.SelectedTableName = int.Parse(Session["Table"].ToString());
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
           

            var order = new Order();
            order.TableName = Session["TableName"].ToString();
            order.WaiterName = Session["UserName"].ToString();
            order.GrandTotal = grandtotal;
            order.OrderedItems = OrderedItems.Count;
            order.ItemsServed = 0;
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
            TableServices.Instance.UpdateTableInfo(ActualCompleteTable, ItemsServed);

            int FinalItemsServed = TableServices.Instance.GetItemsServed(Table);
            int FinalItemsOrdered = TableServices.Instance.GetItemsOrdered(Table);

            if(FinalItemsOrdered == FinalItemsServed)
            {
                TableServices.Instance.UpdateTableInfo(ActualCompleteTable, "Order Delivered", "Active");
            }


            int ItemsServedForOrder = OrderServices.Instance.GetItemsServedForOrder(Table);
            ++ItemsServedForOrder;
            var ActualCompleteOrder = OrderServices.Instance.GetOrderByTable(Table);


            OrderServices.Instance.UpdateOrderInfo(ActualCompleteOrder, ItemsServedForOrder);


            KitchenDashboardViewModel model = new KitchenDashboardViewModel();
            model.Orders = OrderServices.Instance.GetOrders();
            model.Entries = TableEntryServices.Instance.GetNonDispatchedTableEntries();
           
            return PartialView("KitchenOrderDashboard", model);

        }


        public ActionResult CloseSession(int ID) //table ID
        {
            var Table = TableServices.Instance.GetTable(ID);
            TableServices.Instance.UpdateTableInfo(Table,"Waiting for Billing", "Non Active");
            return RedirectToAction("WaiterApp", "Home");
        }








    }
}