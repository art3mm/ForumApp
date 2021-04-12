using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class TopicModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ICollection<string> MessagesIds { get; set; }
    }
}
