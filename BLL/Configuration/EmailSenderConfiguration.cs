using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Configuration
{
    public class EmailSenderConfiguration
    {
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string Password { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
    }
}
