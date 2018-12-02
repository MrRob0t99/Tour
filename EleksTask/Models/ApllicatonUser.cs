using Microsoft.AspNetCore.Identity;
using TourServer.Models;

namespace EleksTask.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

}
