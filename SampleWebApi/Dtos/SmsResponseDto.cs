using System;

namespace SampleWebApi.Dtos
{
    public class SmsResponseDto
    {
        public int Id { get; set; }
        public DateTime SentOn { get; set; }
        public string GsmNumber { get; set; }
        public string Message { get; set; }
    }
}
