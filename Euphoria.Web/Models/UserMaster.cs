using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Euphoria.Web.Models
{
    public class UserMaster
    {
       

        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public int roleId { get; set; }
        public virtual Role Role { get; set; }
        

    }
}