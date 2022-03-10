using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_System.ViewModels
{
    public class KitchenDashboardViewModel
    {
        public List<TableEntry> Entries { get; set; }
        public List<Order> Orders { get; set; }
        public string Role { get; set; }
    }
}