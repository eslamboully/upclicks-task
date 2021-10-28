using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IDailyAuthenticationsService
    {
        Task<DailyAuthentication> GetDailyAuthenticationsStatus(string userEmail);
        Task<bool> AddDailyAuthenticationForUser(string userEmail);
    }
}