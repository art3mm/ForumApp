using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class MessageModel
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        public int TopicId { get; set; }

        public DateTime CreateDate { get; set; }

        public string Text { get; set; }
    }
}
