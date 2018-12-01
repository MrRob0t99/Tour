using EleksTask.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourServer.Models;

namespace EleksTask
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Tour> Tours { get; set; }

        public DbSet<EmailToken> EmailTokens { get; set; }

        public DbSet<FileModel> FileModels { get; set; }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Busket> Buskets { get; set; }

        public DbSet<Order> Orders { get; set; }
    }


}
