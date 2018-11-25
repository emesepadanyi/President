using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using President.API.Dtos;
using President.BLL.Services;
using President.DAL.Entities;
using System;
using System.Collections.Generic;
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
        private readonly string userId;

        public RequestsController(
             IHttpContextAccessor httpContextAccessor,
            IRelationshipService _relatioshipService,
            IMapper _mapper)
        {
            relatioshipService = _relatioshipService;
            mapper = _mapper;

            ClaimsPrincipal caller = httpContextAccessor.HttpContext.User;
            userId = relatioshipService.GetUser(caller);
        }

        // GET: api/requests
        [HttpGet]
        public IActionResult Requests()
        {
            var requests = relatioshipService.GetRequests(userId);
            var requestsDtos = mapper.Map<IList<UserDto>>(requests);

            return Ok(requestsDtos);
        }

        // PUT requests/accept
        [HttpPut("accept")]
        public IActionResult AcceptRequest([FromBody]UserDto sender)
        {
            try
            {
                relatioshipService.AcceptRequest(sender.Id, userId);
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
                relatioshipService.RejectRequest(sender.Id, userId);
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
            if (relatioshipService.CreateRequest(userId, receiver.Id))
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
