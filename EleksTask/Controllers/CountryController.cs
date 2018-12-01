using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourServer.Dto;
using TourServer.ServicesInterface;

namespace TourServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody]CreateCountryRequestDto dto)
        {
            if (dto.Name == null)
            {
                return BadRequest();
            }
            var response = await _countryService.CreateCountry(dto.Name);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCountry()
        {
            var response = await _countryService.GetCountry();
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
