using System;

namespace API.Entities
{
    public class DailyAuthentication
    {
        public int Id { get; set; }
        public DateTimeOffset StartAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset EndAt { get; set; }
        public DateTime Day { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}