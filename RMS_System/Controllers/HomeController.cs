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

        bool WaiterRole = false;
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminDashboard()
        {
            return View();
        }

        public ActionResult KitchenDashboard()
        {
            return View();
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
                model.WaiterName = UsersService.Instance.GetUserName(Session["UserName"].ToString());
                model.MenuItmsCategories = MenuItemServices.Instance.GetAllCategories();
                model.MenuItems = MenuItemServices.Instance.GetMenuItems();
                model.OrderedQuantity = 0;
                return View("GoToFoodEntry", model);

            }
           
        }

        [HttpGet]
        public ActionResult TableFoodEntry(int ID, int Quantity)
        {
            FoodEntryViewModel model = new FoodEntryViewModel();
            model.MenuItem = MenuItemServices.Instance.GetMenuItem(ID);
            model.Quantity = Quantity;
            return PartialView();
        }


    }
}