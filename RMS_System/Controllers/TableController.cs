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
    public class TableController : Controller
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
        // GET: Table
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TableListing(string search = null)
        {
            CheckRole();
            if (role == false)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TableViewModelListingInAdmin model = new TableViewModelListingInAdmin();
                model.Tables = TableServices.Instance.GetTables(search);
                if (search == null)
                {
                    return View("TableListing", model);
                }
                else
                {
                    return View("TableListing", model);
                }

            }

        }

       
        [HttpPost]
        public ActionResult Create(NewTableViewModel model)
        {
            CheckRole();
            if (role == false)
            {
                return RedirectToAction("Login", "Login");

            }
            else
            {
                var newTable = new Table();
                newTable.TableName = model.TableName;
                newTable.FloorName = model.FloorName;
                newTable.Seats = model.Seats;
                newTable.TableStatus = model.TableStatus;
                TableServices.Instance.SaveTable(newTable);
                return RedirectToAction("TableListing", "Table");
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
                EditTableViewModel model = new EditTableViewModel();
                var table = TableServices.Instance.GetTable(ID);
                model.ID = table.ID;
                model.Tables = TableServices.Instance.GetTables();
                model.TableName = table.TableName;
                model.FloorName = table.FloorName;
                model.Seats = table.Seats;
                model.TableStatus = table.TableStatus;
                return PartialView("Edit", model);
            }

        }

        [HttpPost]
        public ActionResult Edit(EditTableViewModel model)
        {
            CheckRole();

            if (role == false)
            {
                return RedirectToAction("Login", "Login");

            }
            else
            {
                var existingTable = TableServices.Instance.GetTable(model.ID);
                existingTable.TableName = model.TableName;
                existingTable.FloorName = model.FloorName;
                existingTable.Seats = model.Seats;
                existingTable.TableStatus = model.TableStatus;
                TableServices.Instance.UpdateTable(existingTable);
                return RedirectToAction("TableListing", "Table");
            }
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            TableServices.Instance.DeleteTable(ID);
            return RedirectToAction("TableListing", "Table");
        }
    }
}