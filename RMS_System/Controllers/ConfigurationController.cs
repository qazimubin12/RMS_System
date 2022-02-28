using RMS_System.Entities;
using RMS_System.Services;
using RMS_System.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS_System.Controllers
{
    public class ConfigurationController : Controller
    {
        // GET: Configuration
        public ActionResult Index()
        {
            ConfigurationViewModel model = new ConfigurationViewModel();
            var config = ConfigurationServices.Instance.GetConfig();
            if (config == null)
            {
                var configuration = new Configuration();
                configuration.CGST = 2.5;
                configuration.SGST = 2.5;
                configuration.HotelName = "Shadab Hotel";
                configuration.HotelAddress = "Please Edit Me";
                ConfigurationServices.Instance.SaveConfig(configuration);
            }
            else
            {
                model.Configuration = ConfigurationServices.Instance.GetConfig();
            }
            return View(model);
        }



        [HttpGet]
        public ActionResult Edit()
        {
            EditConfigurationViewModel  model = new EditConfigurationViewModel();
            var config = ConfigurationServices.Instance.GetConfig();
            if(config != null)
            {
                model.ID = config.ID;
                model.CGST = config.CGST;
                model.SGST = config.CGST;
                model.HotelName = config.HotelName;
                model.HotelAddress = config.HotelAddress;
                model.ImageURL = config.ImageURL;
            }
            else
            {
                model.ID = 0;
                model.CGST = 0;
                model.SGST = 0;
                model.HotelName = "";
                model.HotelAddress = "";
                model.ImageURL = "";
            }
           
            return View("Edit", model);
        }


        [HttpPost]
        public ActionResult Edit(EditConfigurationViewModel model)
        {
            var config = new Configuration();
            config.ID = model.ID;
            config.CGST = model.CGST;
            config.SGST = model.SGST;
            config.HotelName = model.HotelName;
            config.HotelAddress = model.HotelAddress;
            config.ImageURL = model.ImageURL;
            ConfigurationServices.Instance.UpdateConfig(config);
            return RedirectToAction("Index", "Configuration");
        }





    }
}