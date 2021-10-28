using System.Collections.Generic;

namespace API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } = "employee";
        public List<DailyAuthentication> DailyAuthentications { get; set; }
    }
}