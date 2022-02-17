using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS_System.Entities
{
    public class UserRoleMapping :BaseEntity
    {
        public virtual int UserID { get; set; }
        public virtual int RoleID { get; set; }
    }
}
