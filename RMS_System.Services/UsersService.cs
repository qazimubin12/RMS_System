using RMS_System.Database;
using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace RMS_System.Services
{
    public class UsersService
    {
        #region Singleton
        public static UsersService Instance
        {
            get
            {
                if (instance == null) instance = new UsersService();
                return instance;
            }
        }
        private static UsersService instance { get; set; }
        private UsersService()
        {
        }
        #endregion

        public User GetUser(int ID)
        {
            using (var context = new RMContext())
            {
                return context.Users.Find(ID);
            }
        }

        public User GetUserForLogin(string UserName, string Password)
        {
            try
            {
                using (var context = new RMContext())
                {
                    return context.Users.FirstOrDefault(x => x.UserName == UserName && x.Password == Password);
                }
            }

            catch (Exception ex)
            {

                throw;
            }
        }

        public User GetUserName(string UserName)
        {
            using (var context = new RMContext())
            {
                return context.Users.FirstOrDefault(x => x.UserName == UserName);
            }
        }

        public int GetUserID(string UserName)
        {
            using (var context = new RMContext())
            {
                return context.Users.Where(x => x.UserName == UserName).Select(x => x.ID).FirstOrDefault();
            }
        }

        public List<User> GetAllUsers(string search =null)
        {
            using (var context = new RMContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Users.Where(p => p.UserName != null && p.UserName.ToLower()
                        .Contains(search.ToLower()))
                        .OrderBy(x => x.UserName)
                        .ToList();
                }
                else
                {
                    return context.Users
                        .OrderBy(x => x.UserName)
                        .ToList();
                }
            }
        }

        public void SaveUser(User user)
        {
            using (var context = new RMContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public List<Role> GetAllRoles()
        {
            using (var context = new RMContext())
            {
                var result = context.Roles.ToList();
                return result;  
            }
        }

        public List<string> GetAllRolesInString()
        {
            using (var context = new RMContext())
            {
                var result = context.Roles.Select(x=>x.Roles).ToList();
                return result;
            }
        }
        public void UpdateUser(User user)
        {
            using (var context = new RMContext())
            {
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteUser(int ID)
        {
            using (var context = new RMContext())
            {

                var User = context.Users.Find(ID);
                context.Users.Remove(User);
                context.SaveChanges();
            }
        }
    }
}
