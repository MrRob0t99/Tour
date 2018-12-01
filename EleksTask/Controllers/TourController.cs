using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EleksTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourServer.Dto;
using TourServer.Models;
using TourServer.ServicesInterface;

namespace TourServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CreateTourDto createTourDto)
        {
            var response = await _tourService.CreateAsync(createTourDto);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("file/{tourId}")]
        public async Task<IActionResult> UploadFile([FromRoute]int tourId, IFormFileCollection files)
        {
            var response = await _tourService.UploadFile(tourId,files);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTour([FromQuery]GetToursRequestDto requestDto)
        {
            var response = await _tourService.GetAllTour(requestDto);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTourById([FromRoute]int id)
        {
            var response = await _tourService.GetTourById(id);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTour([FromRoute] int id)
        {
            var response = await _tourService.DeleteTour(id);
            if (response.Error != null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}