using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_System.ViewModels
{
    public class MenuItemListingViewModel
    {
        public List<MenuItem> MenuItems { get; set; }

    }

    public class EditMenuItemViewModel
    {
        public int ID { get; set; }
        public string MenuName { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Double Price { get; set; }
        public string ImageURL { get; set; }
    }

    public class NewMenuItemViewModel
    {
        public string MenuName { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Double Price { get; set; }
        public string ImageURL { get; set; }
    }
}