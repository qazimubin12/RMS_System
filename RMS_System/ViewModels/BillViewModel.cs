using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_System.ViewModels
{
    public class BillViewModel
    {
        public List<TableEntry> TableEntries { get; set; }
        public List<Order> Order { get; set; }
        public Configuration configuration { get; set; }
    }
}