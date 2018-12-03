using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EleksTask;
using EleksTask.Dto;
using EleksTask.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TourServer.Dto;
using TourServer.Models;
using TourServer.ServicesInterface;

namespace TourServer.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IEmailService emailService, IConfiguration configuration,
            RoleManager<IdentityRole> roleManager, ApplicationContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<Response<LogInResponseDto>> LogInAsync(LogInDto logInDto)
        {
            var response = new Response<LogInResponseDto>();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == logInDto.UserName && u.EmailConfirmed);
            if (user != null && await _userManager.CheckPasswordAsync(user, logInDto.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claim = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("Roles",roles[0]),
                };

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddMonths(1),
                    claims: claim,
                    signingCredentials: new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("SecretKey").Value)),
                            SecurityAlgorithms.HmacSha256)
                );

                var handler = new JwtSecurityTokenHandler();

                var dto = new LogInResponseDto()
                {
                    Token = handler.WriteToken(token),
                    Exparation = token.ValidTo
                };
                response.Data = dto;
                return response;
            }
            response.Error = new Error("Username or/and password not correct");
            return response;
        }

        public async Task<Response<string>> Registration(RegistrationDto registrationDto)
        {

            var response = new Response<string>();

            if (_context.Users.Any(u => u.UserName == registrationDto.UserName))
            {
                response.Error = new Error($"User with email {registrationDto.Email} already exist. Please sign in");
                return response;
            }

            if (_context.Users.Any(u => u.UserName == registrationDto.UserName))
            {
                response.Error = new Error($"User with username {registrationDto.UserName} already exist");
                return response;
            }



            var user = new ApplicationUser()
            {
                Email = registrationDto.Email,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                UserName = registrationDto.UserName
            };
            if (!await _context.Users.AnyAsync())
            {
                registrationDto.Role = Role.Admin;
            }

            if (!await _roleManager.RoleExistsAsync(registrationDto.Role.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(registrationDto.Role.ToString()));
            }

            var result = await _userManager.CreateAsync(user, registrationDto.Password);
            if (result.Errors != null && result.Errors.Any())
            {
                response.Error =new Error(result.Errors.Select(e => e.Description).Aggregate((a, b) => a + b));
                return response;
            }
            await _userManager.AddToRoleAsync(user, registrationDto.Role.ToString());

            var token = new EmailToken()
            {
                UserId = user.Id,
                Token = Guid.NewGuid()
            };

            await _context.EmailTokens.AddAsync(token);
            await _context.SaveChangesAsync();
            await _emailService.SendConfirmLetter(token.Token.ToString(), user.Id, user.Email);

            response.Data = user.Id;
            return response;
        }

        public async Task<Response<bool>> ConfirmEmail(Guid token, [FromQuery] string userId)
        {
            var response = new Response<bool>();
            var tok = await _context.EmailTokens.AsNoTracking().FirstOrDefaultAsync(t => t.UserId == userId && t.Token == token);
            if (tok == null)
            {
                response.Error =new Error("Not found");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.EmailConfirmed = true;

            _context.EmailTokens.Remove(tok);
            await _context.SaveChangesAsync();
            response.Data = true;
            return response;
        }
    }
}
