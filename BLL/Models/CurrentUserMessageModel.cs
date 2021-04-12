using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class CurrentUserMessageModel
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public DateTime CreateDate { get; set; }

        public string TopicTitle { get; set; }

        public string Text { get; set; }
    }
}
