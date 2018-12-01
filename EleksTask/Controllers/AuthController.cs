using System;
using System.Threading.Tasks;
using EleksTask.Dto;
using EleksTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourServer.ServicesInterface;

namespace EleksTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> LogInAsync([FromBody] LogInDto logInDto)
        {
            var response = await _authService.LogInAsync(logInDto);
            if (response.Error!=null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationDto registrationDto)
        {
            var response = await _authService.Registration(registrationDto);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("confirmEmail")]
        public async Task<ActionResult> ConfirmEmail([FromQuery]Guid token, [FromQuery] string userId)
        {
            var response = await _authService.ConfirmEmail(token,userId);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }

}
