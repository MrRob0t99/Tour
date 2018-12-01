using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourServer.Dto;
using TourServer.ServicesInterface;

namespace TourServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody]HotelDto hotelDto)
        {
            var response = await _hotelService.CreateHotel(hotelDto);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotels([FromRoute]int id)
        {
            var response = await _hotelService.GetHotels(id);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}