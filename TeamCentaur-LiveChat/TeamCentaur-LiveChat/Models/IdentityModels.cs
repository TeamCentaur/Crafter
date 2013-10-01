using Microsoft.AspNet.Identity.EntityFramework;

namespace TeamCentaur_LiveChat.Models
{
    // You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : User
    {
        public string Email { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public int Age { get; set; }

        public string City { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContextWithCustomUser<ApplicationUser>
    {
    }
}