using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public static class DatabaseContextSeed
    {
        public static async Task AddAdminUserAsync(DatabaseContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                using var hmac = new HMACSHA512();
                var user = new User
                {
                    Name = "Admin User",
                    Email = "admin@admin.com",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("admin")),
                    PasswordSalt = hmac.Key,
                    Role = "admin"
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }
        }
    }
}