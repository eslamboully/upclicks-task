using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public UsersController(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserToReturnDto>>> GetAllUsers()
        {
            var users = await _context.Users.Include(c => c.DailyAuthentications)
                .Where(u => u.Role == "employee").ToListAsync();

            return _mapper.Map<List<User>, List<UserToReturnDto>>(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserToReturnDto>> GetUserDetails(int id)
        {
            var user = await _context.Users.Include(c => c.DailyAuthentications)
                .FirstOrDefaultAsync(u => u.Id == id);

            return _mapper.Map<User, UserToReturnDto>(user);
        }
    }
}