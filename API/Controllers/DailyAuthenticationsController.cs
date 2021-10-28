using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
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
        public async Task<ActionResult<DailyAuthentication>> UserDailyAuthenticationStatus()
        {
            return await _dailyAuthenticationsService.GetDailyAuthenticationsStatus(ClaimTypes.Email);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddDailyAuthentication()
        {
            return await _dailyAuthenticationsService.AddDailyAuthenticationForUser(ClaimTypes.Email);
        }
    }
}