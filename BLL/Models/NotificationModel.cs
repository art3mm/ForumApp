using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class NotificationModel
    {
        public string EmailAdress { get; set; }
        public string EmailSubject { get; set; }
        public string RecipientName { get; set; }
        public string EmailText { get; set; }
    }
}
