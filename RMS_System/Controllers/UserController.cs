using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMS_System.Entities;
using RMS_System.Services;
using RMS_System.ViewModels;
namespace RMS_System.Controllers
{
    public class UserController : Controller
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
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserListing(string search)
        {
            CheckRole();
            if (role == false)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                UserListingViewModel model = new UserListingViewModel();
                model.Users = UsersService.Instance.GetAllUsers(search);
                return View("UserListing", model);
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
                var roles = UsersService.Instance.GetAllRoles();
                return View(roles);
            }
        }

        [HttpPost]
        public ActionResult Create(NewUserViewModel model)
        {
            CheckRole();
            if (role == false)
            {
                return RedirectToAction("Login", "Login");

            }
            else
            {
                var newUser = new User();
                newUser.UserName = model.UserName;
                newUser.Email = model.Email;
                newUser.Role = model.User_Role.Roles;
                newUser.Password = model.Password;
                newUser.ImageURL = model.ImageURL;
                UsersService.Instance.SaveUser(newUser);
                return RedirectToAction("UserListing","User");
            }
        }
    }
}