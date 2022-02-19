using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Entities
{
    public class Table:BaseEntity
    {
        public string TableName { get; set; }
        public int Seats { get; set; }
        public int? OrderItems { get; set; }
        public int? ItemsServed { get; set; }
        public string ServedBy { get; set; }
        public string TableStatus { get; set; }
        public string SessionStatus { get; set; }
    }
}
