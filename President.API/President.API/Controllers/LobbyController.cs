using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using President.API.Helpers;
using President.API.Hubs;
using President.BLL.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace President.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    public class LobbyController : Controller
    {
        private readonly IRelationshipService relatioshipService;
        private readonly IMapper mapper;
        private readonly string userId;

        public LobbyController(
            IRelationshipService _relatioshipService,
            IMapper _mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            relatioshipService = _relatioshipService;
            mapper = _mapper;

            ClaimsPrincipal caller = httpContextAccessor.HttpContext.User;
            userId = relatioshipService.GetUser(caller);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult GetOnlineFriends()
        {
            List<string> friends = relatioshipService.GetFriends(userId).Select(user => user.UserName).ToList();

            if (friends.Count() < 3)
            {
                return BadRequest(Errors.AddErrorToModelState("not_enough_friends", "You have to have at least 3 friends to play!", ModelState));
            }

            var onlineUsers = OnlineHub.UserList.Values.ToHashSet();
            var onlineFriends = onlineUsers.Intersect(friends);

            if (onlineFriends.Count() < 3)
            {
                return BadRequest(Errors.AddErrorToModelState("not_enough_friends", "Not enough friends are online", ModelState));
            }

            return Ok(onlineFriends);
        }
    }
}
