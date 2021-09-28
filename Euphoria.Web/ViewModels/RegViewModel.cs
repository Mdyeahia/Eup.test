using Euphoria.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Euphoria.Web.ViewModels
{
    public class RegisterModel
    {
        

        public UserMaster userMaster { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        

        public int roleId { get; set; }
        public Role Role { get; set; }
        public List<Role> roles { get; set; }

    }
    public class ListViewModel
    {
        public List<UserMaster> listuser { get; set; }
    }
    public class detailsViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }

        public int roleId { get; set; }
        public Role roleName { get; set; }
    }
    
}