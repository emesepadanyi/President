using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using President.API.ViewModels;
using President.DAL.Context;
using President.DAL.Entities;
using System.Threading.Tasks;
using President.API.Helpers;

namespace President.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly PresidentDbContext _presidentDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<User> userManager, IMapper mapper, PresidentDbContext presidentDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _presidentDbContext = presidentDbContext;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<User>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _presidentDbContext.PlayerStatistics.AddAsync(new PlayerStatistics() { UserId = userIdentity.Id, GamesPlayed = 0, SumPointsEarned = 0, TimesWon = 0});

            await _presidentDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }
    }
}
