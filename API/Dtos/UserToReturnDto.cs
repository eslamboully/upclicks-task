using System.Collections.Generic;

namespace API.Dtos
{
    public class UserToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<DailyAuthenticationDto> DailyAuthentications { get; set; }
    }
}