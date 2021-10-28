using System;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class DailyAuthenticationsService : IDailyAuthenticationsService
    {
        private readonly DatabaseContext _context;

        public DailyAuthenticationsService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<DailyAuthentication> GetDailyAuthenticationsStatus(string userEmail)
        {
            var user = GetUserAsync(userEmail);
            var userDailyAuthentication = await _context.DailyAuthentications
                .FirstOrDefaultAsync(c => c.UserId == user.Id && c.Day.Date == DateTime.Now.Date);

            return userDailyAuthentication;
        }

        public async Task<bool> AddDailyAuthenticationForUser(string userEmail)
        {
            var user = GetUserAsync(userEmail);
            var userDailyAuthentication = await GetDailyAuthenticationsStatus(userEmail);

            if (userDailyAuthentication != null) // The employee loggedin in the morning or not
            {
                userDailyAuthentication.EndAt = DateTimeOffset.Now; // Change the employee logout
            }
            else
            {
                var dailyAuthentication = new DailyAuthentication // login date is added automatically
                {
                    Day = DateTime.Now,
                    UserId = user.Id,
                };
                
                await _context.DailyAuthentications.AddAsync(dailyAuthentication);
            }
            
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<User> GetUserAsync(string userEmail)
        {
            return await _context.Users
                .FirstOrDefaultAsync(c => c.Email == userEmail);
        }
    }
}