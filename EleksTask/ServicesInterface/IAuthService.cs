using System;
using System.Threading.Tasks;
using EleksTask.Dto;
using EleksTask.Models;
using Microsoft.AspNetCore.Mvc;
using TourServer.Dto;

namespace TourServer.ServicesInterface
{
    public interface IAuthService
    {
        Task<Response<LogInResponseDto>> LogInAsync(LogInDto logInDto);

        Task<Response<string>> Registration(RegistrationDto registrationDto);

        Task<Response<bool>> ConfirmEmail(Guid token, [FromQuery] string userId);
    }
}
