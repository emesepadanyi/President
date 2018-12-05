using Microsoft.AspNetCore.Identity;

namespace President.DAL.Entities
{
    public class User : IdentityUser
    {
        public string PictureUrl { get; set; }
    }
}
