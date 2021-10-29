using System;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class DailyAuthenticationsService : IDailyAuthenticationsService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public DailyAuthenticationsService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DailyAuthenticationDto> GetDailyAuthenticationsStatus(string userEmail)
        {
            var user = await GetUserAsync(userEmail);
            var userDailyAuthentication = await _context.DailyAuthentications
                .FirstOrDefaultAsync(c => c.UserId == user.Id && c.Day.Date == DateTime.Now.Date);

            return _mapper.Map<DailyAuthentication, DailyAuthenticationDto>(userDailyAuthentication);
        }

        public async Task<DailyAuthenticationDto> AddDailyAuthenticationForUser(string userEmail)
        {
            var user = await GetUserAsync(userEmail);
            var userDailyAuthentication = await GetDailyAuthenticationsStatus(userEmail);

            if (userDailyAuthentication != null) // The employee loggedin in the morning or not
            {
                userDailyAuthentication.EndAt = DateTimeOffset.Now; // Change the employee logout
                await _context.SaveChangesAsync();
                
                return userDailyAuthentication;
            }
            else
            {
                var dailyAuthentication = new DailyAuthentication // login date is added automatically
                {
                    Day = DateTime.Now,
                    UserId = user.Id,
                };
                
                await _context.DailyAuthentications.AddAsync(dailyAuthentication);
                await _context.SaveChangesAsync();
                
                return _mapper.Map<DailyAuthentication, DailyAuthenticationDto>(dailyAuthentication);
            }
        }

        private async Task<User> GetUserAsync(string userEmail)
        {
            return await _context.Users
                .FirstOrDefaultAsync(c => c.Email == userEmail);
        }
    }
}