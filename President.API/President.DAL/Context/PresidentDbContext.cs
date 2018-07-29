using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using President.DAL.Entities;

namespace President.DAL.Context
{
    public class PresidentDbContext : IdentityDbContext<User>
    {
        public PresidentDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
