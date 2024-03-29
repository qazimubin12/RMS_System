﻿using System;
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
                if (search == null)
                {
                    return View("UserListing", model);
                }
                else
                {
                    return View("UserListing", model);
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
                newUser.Role = model.Role;
                newUser.Password = model.Password;
                newUser.ImageURL = model.ImageURL;
                UsersService.Instance.SaveUser(newUser);
                return RedirectToAction("UserListing","User");
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
                EditUserViewModel model = new EditUserViewModel();
                var user = UsersService.Instance.GetUser(ID);
                model.ID = user.ID;
                model.UserName = user.UserName;
                model.Email = user.Email;
                model.Password = user.Password;
                model.Role = user.Role;
                model.AllRoles = UsersService.Instance.GetAllRolesInString();
                ViewBag.User_Role = model.Role;
                model.ImageURL = user.ImageURL;
                return PartialView("Edit",model);
            }

        }

        [HttpPost]
        public ActionResult Edit(EditUserViewModel model)
        {
            CheckRole();

            if (role == false)
            {
                return RedirectToAction("Login", "Login");

            }
            else
            {
                var existingUser = UsersService.Instance.GetUser(model.ID);
                existingUser.UserName = model.UserName;
                existingUser.Email = model.Email;
                existingUser.Password = model.Password;
                existingUser.Role = model.Role;
                existingUser.ImageURL = model.ImageURL;
                UsersService.Instance.UpdateUser(existingUser);
                return RedirectToAction("UserListing", "User");
            }
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            UsersService.Instance.DeleteUser(ID);
            return RedirectToAction("UserListing", "User");
        }

    }
}