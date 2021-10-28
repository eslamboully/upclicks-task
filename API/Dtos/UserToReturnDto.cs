using System.Collections.Generic;

namespace API.Dtos
{
    public class UserToReturnDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<DailyAuthenticationDto> DailyAuthentications { get; set; }
    }
}