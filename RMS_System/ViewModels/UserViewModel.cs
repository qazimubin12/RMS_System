using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_System.ViewModels
{
    public class UserListingViewModel
    {
        public List<User> Users { get; set; }
    }

    public class EditUserViewModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual string Role { get; set; }
        public string ImageURL { get; set; }

        public List<Role> Roles { get; set; }
    }

    public class NewUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual string Role { get; set; }
        public string ImageURL { get; set; }
        public Role User_Role { get; set; }
        public List<Role> Roles { get; set; }
    }
}