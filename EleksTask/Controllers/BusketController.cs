using System.Threading.Tasks;
using EleksTask;
using EleksTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TourServer.Dto;
using TourServer.ServicesInterface;

namespace TourServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusketController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBusketService _busketService;
        public BusketController(ApplicationContext context, UserManager<ApplicationUser> userManager,IBusketService busketService)
        {
            _busketService = busketService;
            _userManager = userManager;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody]AddProductToBuseketDto dto)
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;

            var response = await _busketService.AddProduct(dto, userId);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBusket()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;

            var response = await _busketService.GetBusket(userId);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;

            var response = await _busketService.GetCount( userId);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{tourId}")]
        public async Task<IActionResult> DeleteTour([FromRoute]int tourId)
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;

            var response = await _busketService.DeleteTour(tourId, userId);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
