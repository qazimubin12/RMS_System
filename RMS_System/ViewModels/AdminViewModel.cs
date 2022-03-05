using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_System.ViewModels
{
    public class AdminViewModel
    {
        public List<Order> Orders { get; set; }

        public Double TotalRevenueInfoBox { get; set; }
        public Double CashRevenueInfoBox { get; set; }
        public Double CardRevenueInfoBox { get; set; }
        public int NoOfSessions { get; set; }


    }
}