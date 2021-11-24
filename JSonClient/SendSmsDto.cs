using System;
using System.Collections.Generic;
using System.Text;

namespace JSonClient
{
    public class SendSmsDto
    {
        public string GsmNumber { get; set; }
        public string Message { get; set; }
        public DateTime SentOn { get; set; } = DateTime.Now;
    }
}
