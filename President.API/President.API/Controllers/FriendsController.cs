using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using President.API.Dtos;
using President.BLL.Services;
using President.DAL.Context;
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

        public FriendsController(IRelationshipService _relatioshipService, IMapper _mapper, IHttpContextAccessor httpContextAccessor)
        {
            relatioshipService = _relatioshipService;
            mapper = _mapper;

            ClaimsPrincipal caller = httpContextAccessor.HttpContext.User;
            userId = relatioshipService.GetUser(caller);
        }

        // GET: friends
        [HttpGet]
        public IActionResult GetFriends()
        {
            var friends = relatioshipService.GetFriends(userId);
            var friendDtos = mapper.Map<IList<UserDto>>(friends);

            return Ok(friendDtos);
        }

        // GET: friends/Anna
        [HttpGet("{keyWord}")]
        public IActionResult FindFriends([FromRoute] string keyWord)
        {
            var users = relatioshipService.FindUsers(userId, keyWord);
            var userDtos = mapper.Map<IList<UserDto>>(users);

            return Ok(userDtos);
        }
    }
}
