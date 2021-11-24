using System;

namespace SampleWebApi.Dtos
{
    public class SendSmsDto
    {
        public DateTime SentOn { get; set; }
        public string GsmNumber { get; set; }
        public string Message { get; set; }
    }
}
