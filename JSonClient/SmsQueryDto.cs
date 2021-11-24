using System;

namespace JSonClient
{
    public class SmsQueryDto
    {
        public string GsmNumber { get; set; } = null;
        public DateTime? StartTime { get; set; } = null;
        public DateTime? EndTime { get; set; } = null;
    }
}
