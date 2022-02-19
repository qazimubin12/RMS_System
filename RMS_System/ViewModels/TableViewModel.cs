using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_System.ViewModels
{
    public class TableViewModelListingInAdmin
    {
        public List<Table> Tables { get; set; }
    }

    public class EditTableViewModel
    {
        public int ID { get; set; }
        public List<Table>Tables { get; set; }
        public string TableName { get; set; }
        public string FloorName { get; set; }
        public int Seats { get; set; }
        public int? OrderItems { get; set; }
        public int? ItemsServed { get; set; }
        public string ServedBy { get; set; }
        public string TableStatus { get; set; }
        public string SessionStatus { get; set; }
    }

    public class NewTableViewModel
    {
        public string TableName { get; set; }
        public string FloorName { get; set; }
        public int Seats { get; set; }
        public int? OrderItems { get; set; }
        public int? ItemsServed { get; set; }
        public string ServedBy { get; set; }
        public string TableStatus { get; set; }
        public string SessionStatus { get; set; }
    }
}