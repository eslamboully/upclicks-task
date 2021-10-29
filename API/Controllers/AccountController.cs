using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public AccountController(ITokenService tokenService, DatabaseContext context, IMapper mapper)
        {
            _tokenService = tokenService;
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("profile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> Profile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            
            var user = await _context.Users.Include(c => c.DailyAuthentications)
                .FirstOrDefaultAsync(u => u.Email == email);
            // return _mapper.Map<User, UserDto>(user);
            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.GenerateToken(user),
                Name = user.Name,
                Role = user.Role
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto userLoginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLoginDto.Email);
            
            if (user == null) return Unauthorized("Invalid Email");
            
            var isValid = IsUserPasswordValid(user, userLoginDto.Password);
            
            if (!isValid) return Unauthorized("Invalid Password");

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.GenerateToken(user),
                Name = user.Name,
                Role = user.Role
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegisterDto userRegisterDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userRegisterDto.Email);
            
            if (user != null) return BadRequest("Email Exists");
            
            if (ModelState.IsValid)
            {
                using var hmac = new HMACSHA512();

                var newEmployee = new User
                {
                    Name = userRegisterDto.Name,
                    Email = userRegisterDto.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDto.Password)),
                    PasswordSalt = hmac.Key,
                };

                await _context.Users.AddAsync(newEmployee);
                await _context.SaveChangesAsync();

                return new UserDto
                {
                    Email = newEmployee.Email,
                    Token = _tokenService.GenerateToken(newEmployee),
                    Name = newEmployee.Name,
                    Role = newEmployee.Role
                };
            }

            return BadRequest("Something occurs");
        }

        private bool IsUserPasswordValid(User user, string password)
        {
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computePasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computePasswordHash.Length; i++)
            {
                if (computePasswordHash[i] != user.PasswordHash[i]) return false;
            }
            return true;
        }
    }
}