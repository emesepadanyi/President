using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using President.DAL.Context;
using President.DAL.Entities;

namespace President.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        private readonly PresidentDbContext _appDbContext;

        public DashboardController(UserManager<User> userManager, PresidentDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _appDbContext = appDbContext;
        }

        // GET api/dashboard/home
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            // retrieve the user info
            //HttpContext.User
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var user = await _appDbContext.Users.SingleAsync(dbUser => dbUser.Id == userId.Value);

            return new OkObjectResult(new
            {
                Message = "This is secure API and user data!",
                user.FirstName,
                user.LastName,
                user.PictureUrl,
                user.FacebookId
                //, customer.Location,
                //customer.Locale,
                //customer.Gender
            });
        }
    }
}
