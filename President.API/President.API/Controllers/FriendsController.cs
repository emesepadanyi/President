using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using President.API.Dtos;
using President.BLL.Services;
using President.DAL.Context;
using President.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace President.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FriendsController : Controller
    {
        private IRelationshipService relatioshipService;
        private IMapper mapper;

        private readonly User user;
        private readonly PresidentDbContext presidentDbContext;

        public FriendsController(
            IRelationshipService _relatioshipService,
            IMapper _mapper,
            IHttpContextAccessor httpContextAccessor,
            PresidentDbContext _presidentDbContext)
        {
            relatioshipService = _relatioshipService;
            mapper = _mapper;
            presidentDbContext = _presidentDbContext;

            ClaimsPrincipal caller = httpContextAccessor.HttpContext.User;
            user = GetUser(caller);
        }

        private User GetUser(ClaimsPrincipal caller)
        {
            var userID = caller.Claims.Single(c => c.Type == "id");
            var user = presidentDbContext.Users.Single(dbUser => dbUser.Id == userID.Value);
            return user;
        }

        // GET: friends
        [HttpGet]
        public IActionResult GetFriends()
        {
            var friends = relatioshipService.GetFriends(user.Id);
            var friendDtos = mapper.Map<IList<UserDto>>(friends);

            return Ok(friendDtos);
        }

        // GET: friends/Anna
        [HttpGet("{keyWord}")]
        public IActionResult FindFriends([FromRoute] string keyWord)
        {
            var users = relatioshipService.FindUsers(user.Id, keyWord);
            var userDtos = mapper.Map<IList<UserDto>>(users);

            return Ok(userDtos);
        }

    }
}
