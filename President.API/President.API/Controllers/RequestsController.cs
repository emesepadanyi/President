using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using President.BLL.Dtos;
using President.BLL.Services;
using System;
using System.Collections.Generic;

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

            userId = relatioshipService.GetUser(httpContextAccessor.HttpContext.User);
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
                return Ok("Request accepted");
            }
            catch (Exception)
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
