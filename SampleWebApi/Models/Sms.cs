using System;

namespace SampleWebApi.Models
{
    public class Sms
    {
        public int Id { get; set; }
        public DateTime SentOn { get; set; }
        public string GsmNumber { get; set; }
        public string Message { get; set;}
    }
}
