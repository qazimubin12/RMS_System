using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Entities
{
    public class CancelledOrders:BaseEntity
    {
        public string ItemName { get; set; }
        public DateTime CancelledDate { get; set; }

        public string WaiterName { get; set; }
        public string TableName { get; set; }
    }
}
