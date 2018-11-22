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

namespace President.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RequestsController : Controller
    {
        private IRelationshipService relatioshipService;
        private IMapper mapper;
        private readonly User user;
        private readonly PresidentDbContext presidentDbContext;

        public RequestsController(
             IHttpContextAccessor httpContextAccessor,
            IRelationshipService _relatioshipService,
            PresidentDbContext _presidentDbContext,
            IMapper _mapper)
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

        // GET: api/requests
        [HttpGet]
        public IActionResult Requests()
        {
            var requests = relatioshipService.GetRequests(user.Id);
            var requestsDtos = mapper.Map<IList<UserDto>>(requests);

            return Ok(requestsDtos);
        }

        // PUT requests/accept
        [HttpPut("accept")]
        public IActionResult AcceptRequest([FromBody]UserDto sender)
        {
            try
            {
                relatioshipService.AcceptRequest(sender.Id, user.Id);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        // PUT requests/reject
        [HttpPut("reject")]
        public IActionResult RejectRequest([FromBody]UserDto sender)
        {
            try
            {
                relatioshipService.RejectRequest(sender.Id, user.Id);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult CreateRequest([FromBody]UserDto receiver)
        {
            if (relatioshipService.CreateRequest(user.Id, receiver.Id))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
