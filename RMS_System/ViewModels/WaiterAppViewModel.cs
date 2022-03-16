using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_System.ViewModels
{
    public class WaiterAppViewModel
    {
        public List<Table> Tables { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }

    public class TableEntryForFoodViewModel
    {
        public Table SelectedTableName { get; set; }
        public User WaiterName { get; set; }
        public List<string> MenuItmsCategories { get; set; }
        public List<MenuItem> MenuItems { get; set; }

        public int OrderedQuantity { get; set; }

        public MenuItem MenuItem { get; set; }
    }


    public class FoodEntryViewModel
    {

        public MenuItem MenuItem { get; set; }
        public int Quantity { get; set; }
        public int SelectedTableName { get; set; }
        public int ProductTotal { get; set; }
        public List<TableEntry> Entries { get; set; }
        public string TableStatus { get; set; }
        public Table Table { get; set; }
        public bool OrderSatisfied { get; set; }

        public string SessionStatus { get; set; }

    }
}