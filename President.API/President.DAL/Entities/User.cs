using Microsoft.AspNetCore.Identity;

namespace President.DAL.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
    }
}
