using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }

        public bool IsOnline { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
