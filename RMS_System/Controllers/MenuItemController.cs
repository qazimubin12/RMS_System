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
    public class MenuItemController : Controller
    {
        bool role = false;
        public void CheckRole()
        {
            role = false;
            if (Session["UserName"] == null)
            {
                role = false;
            }
            string[] roles = System.Web.Security.Roles.GetRolesForUser(Convert.ToString(Session["UserName"]));
            if (roles.Length != 0)
            {
                if (roles[0] == "Admin")
                {
                    role = true;
                }
                else
                {
                    role = false;
                }
            }
            else
            {
                role = false;
            }
        }
        // GET: MenuItem
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuListing(string search = null)
        {
            CheckRole();
            if (role == false)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                MenuItemListingViewModel model = new MenuItemListingViewModel();
                model.MenuItems = MenuItemServices.Instance.GetMenuItems(search);
                if (search == null)
                {
                    return View("MenuListing", model);
                }
                else
                {
                    return View("MenuListing", model);
                }

            }

        }


        [HttpGet]
        public ActionResult Create()
        {
            CheckRole();

            if (role == false)
            {
                return RedirectToAction("Login", "Login");

            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(NewMenuItemViewModel model)
        {
            CheckRole();
            if (role == false)
            {
                return RedirectToAction("Login", "Login");

            }
            else
            {
                var newItem = new MenuItem();
                newItem.MenuName = model.MenuName;
                newItem.CategoryName = model.CategoryName;
                newItem.Description = model.Description;
                newItem.Price = model.Price;
                newItem.ImageURL = model.ImageURL;
                MenuItemServices.Instance.SaveMenuItem(newItem);
                return RedirectToAction("MenuListing", "MenuItem");
            }
        }


        [HttpGet]
        public ActionResult Edit(int ID)
        {
            CheckRole();

            if (role == false)
            {
                return RedirectToAction("Login", "Login");

            }
            else
            {
                EditMenuItemViewModel model = new EditMenuItemViewModel();
                var menuItem = MenuItemServices.Instance.GetMenuItem(ID);
                model.ID = menuItem.ID;
                model.MenuName = menuItem.MenuName;
                model.CategoryName = menuItem.CategoryName;
                model.Description = menuItem.Description;
                model.Price = menuItem.Price;
                model.ImageURL = menuItem.ImageURL;
                return View("Edit", model);
            }

        }

        [HttpPost]
        public ActionResult Edit(EditMenuItemViewModel model)
        {
            CheckRole();

            if (role == false)
            {
                return RedirectToAction("Login", "Login");

            }
            else
            {
                var existingMenuItem = MenuItemServices.Instance.GetMenuItem(model.ID);
                existingMenuItem.MenuName = model.MenuName;
                existingMenuItem.CategoryName = model.CategoryName;
                existingMenuItem.Description = model.Description;
                existingMenuItem.Price = model.Price;
                existingMenuItem.ImageURL = model.ImageURL;
                MenuItemServices.Instance.UpdateMenuItems(existingMenuItem);
                return RedirectToAction("MenuListing", "MenuItem");
            }
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            MenuItemServices.Instance.DeleteMenuItems(ID);
            return RedirectToAction("MenuListing", "MenuItem");
        }

    }
}