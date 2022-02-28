using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_System.ViewModels
{
    public class ConfigurationViewModel
    {
        public Configuration Configuration { get; set; }
    }


    public class EditConfigurationViewModel
    {
        public Double CGST { get; set; }
        public Double SGST { get; set; }
        public string ImageURL { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public int ID { get; set; }
    }



}