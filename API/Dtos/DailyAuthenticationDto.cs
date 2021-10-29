using System;

namespace API.Dtos
{
    public class DailyAuthenticationDto
    {
        public DateTimeOffset StartAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset EndAt { get; set; }
        public DateTime Day { get; set; }
    }
}