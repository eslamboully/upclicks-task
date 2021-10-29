using System.Threading.Tasks;
using API.Dtos;
using API.Entities;

namespace API.Interfaces
{
    public interface IDailyAuthenticationsService
    {
        Task<DailyAuthenticationDto> GetDailyAuthenticationsStatus(string userEmail);
        Task<DailyAuthenticationDto> AddDailyAuthenticationForUser(string userEmail);
    }
}