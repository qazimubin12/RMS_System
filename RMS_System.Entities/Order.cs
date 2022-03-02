using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Entities
{
    public class Order : BaseEntity
    {
        public string TableName { get; set; }

        public string WaiterName { get; set; }

        public Double GrandTotal { get; set; }

        public int ItemsServed { get; set; }
        public int OrderedItems { get; set; }
        public Double DiscountPercentage { get; set; }
        public Double Discount { get; set; }
    }
}
