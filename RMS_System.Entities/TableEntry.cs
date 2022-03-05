using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Entities
{
    public class TableEntry :BaseEntity
    {
        public string FoodItem { get; set; }
        public int Quantity { get; set; }
        public int ProductTotal { get; set; }
        public string TableName { get; set; }
        public string FoodDispatchedStatus { get; set; }
        public DateTime OrderedTime { get; set; }
        public bool BillingDone { get; set; }
    }
}
