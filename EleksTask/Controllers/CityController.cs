using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourServer.Dto;
using TourServer.ServicesInterface;

namespace TourServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;


        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityRequestDto cityRequestDto)
        {
            var response = await _cityService.CreateCity(cityRequestDto);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity([FromRoute] int id)
        {

            var response = await _cityService.GetCity(id);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
