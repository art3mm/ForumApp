using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities
{
    public class Topic
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
