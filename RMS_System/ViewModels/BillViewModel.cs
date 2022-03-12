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
        public Double SGST { get; set; }
        public Double CGST { get; set; }
        public Double TotalAmount { get; set; }
    }



    public  class ReceiptViewModel
    {
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public List<TableEntry> TableEntries { get; set; }
        public Configuration configuration { get; set; }
        public Table Table { get; set; }
        public Double SGST { get; set; }
        public Double CGST { get; set; }
        public Double TotalAmount { get; set; }
    }
}