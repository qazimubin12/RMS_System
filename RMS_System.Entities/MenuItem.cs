using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Entities
{
    public class MenuItem : BaseEntity
    {
        public string MenuName { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Double Price { get; set; }
        public string ImageURL { get; set; }
        public int? OrderedQuantity { get; set; }
    }
}
