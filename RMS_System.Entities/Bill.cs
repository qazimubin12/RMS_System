using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Entities
{
    public class Bill : BaseEntity
    {
        public virtual int OrderID { get; set; }
        public virtual int EntriesID { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
