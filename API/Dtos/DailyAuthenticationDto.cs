using System;

namespace API.Dtos
{
    public class DailyAuthenticationDto
    {
        public int Id { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public DateTime Day { get; set; }
    }
}