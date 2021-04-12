using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }
        
        public string ApplicationUserId { get; set; }

        public int TopicId { get; set; }

        public DateTime CreateDate { get; set; }

        public string Text { get; set; }

        public virtual ApplicationUser  ApplicationUser { get; set; }

        public virtual Topic Topic { get; set; }

    }
}
