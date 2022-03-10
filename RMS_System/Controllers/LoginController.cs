using RMS_System.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS_System.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Login(string username, string password)
        {
            var user = UsersService.Instance.GetUserForLogin(username, password);
            if (user != null)
            {
                Session["ID"] = user.ID.ToString();
                Session["UserName"] = user.UserName.ToString();
                Session["Email"] = user.Email.ToString();
                Session["Role"] = user.Role.ToString();
                if (user.Role == "Admin")
                {
                   return RedirectToAction("AdminDashboard", "Home");
                }
                else if (user.Role == "Kitchen Staff")
                {
                    return RedirectToAction("KitchenDashboard", "Home");
                }
                else if (user.Role == "Cashier")
                {
                    return RedirectToAction("BillingDashboard", "Home");
                }

                else if (user.Role == "Waiter")
                {
                    return RedirectToAction("WaiterApp", "Home");
                }
                else if(user.Role == "Kitchen Master")
                {
                    return RedirectToAction("KitchenDashboard", "Home");
                }
            }
            else
            {
                string msg = "Invalid Password or UserName";
                TempData["ErrorMessage"] = msg;
            }

            Session["ID"] = null;
            Session["UserName"] = null;
            Session["Email"] = null;
            Session["Role"] = null;
            return RedirectToAction("Index", "Login");


        }
    }
}