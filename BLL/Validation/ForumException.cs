using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Validation
{
    public class ForumException : Exception
    {
        public string Property { get; protected set; }

        public ForumException(string message, string property) : base(message)
        {
            this.Property = property;
        }
    }
}
