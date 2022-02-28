using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Entities
{
    public class Configuration : BaseEntity
    {
        public Double CGST { get; set; }
        public Double SGST { get; set; }
        public string ImageURL { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }

    }
}
