using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class DailyAuthenticationsController : ControllerBase
    {
        private readonly IDailyAuthenticationsService _dailyAuthenticationsService;

        public DailyAuthenticationsController(IDailyAuthenticationsService dailyAuthenticationsService)
        {
            _dailyAuthenticationsService = dailyAuthenticationsService;
        }

        [HttpGet]
        public async Task<ActionResult<DailyAuthenticationDto>> UserDailyAuthenticationStatus()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return await _dailyAuthenticationsService.GetDailyAuthenticationsStatus(email);
        }

        [HttpPost]
        public async Task<ActionResult<DailyAuthenticationDto>> AddDailyAuthentication()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return await _dailyAuthenticationsService.AddDailyAuthenticationForUser(email);
        }
    }
}