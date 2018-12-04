using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using President.BLL.Dtos;
using President.BLL.Services;
using System.Collections.Generic;
using System.Security.Claims;

namespace President.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FriendsController : Controller
    {
        private IRelationshipService relatioshipService;
        private IMapper mapper;

        private readonly string userId;

        public FriendsController(IHttpContextAccessor httpContextAccessor, IRelationshipService _relatioshipService, IMapper _mapper)
        {
            relatioshipService = _relatioshipService;
            mapper = _mapper;

            ClaimsPrincipal caller = httpContextAccessor.HttpContext.User;
            userId = relatioshipService.GetUser(caller);
        }

        // GET: friends
        [HttpGet]
        public virtual IActionResult GetFriends()
        {
            var friends = relatioshipService.GetFriends(userId);
            var friendDtos = mapper.Map<IList<UserDto>>(friends);

            return Ok(friendDtos);
        }

        // GET: friends/Anna
        [HttpGet("{keyWord}")]
        public virtual IActionResult FindFriends([FromRoute] string keyWord)
        {
            var users = relatioshipService.FindUsers(userId, keyWord);
            var userDtos = mapper.Map<IList<UserDto>>(users);

            return Ok(userDtos);
        }
    }
}
