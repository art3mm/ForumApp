using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public int ProfileId { get; set; }

        public string NickName { get; set; }

        public bool IsOnline { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string About { get; set; }

        public ICollection<int> MessagesIds { get; set; }
    }
}
