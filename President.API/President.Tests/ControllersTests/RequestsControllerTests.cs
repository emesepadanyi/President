using Microsoft.AspNetCore.Mvc;
using President.API.Controllers;
using President.BLL.Dtos;
using President.Tests.MockClasses;
using System.Collections.Generic;
using Xunit;

namespace President.Tests.ControllersTests
{
    public class RequestsControllerTests
    {
        const string returnsValue = "1";
        const string returnsNull = "2";

        public RequestsController RequestsController(string userId)
        {
            return new RequestsController(
                MockHttpContextAccessor.Get(userId),
                MockRelationshipService.Get(userId),
                MapperConfig.MapperInstance);
        }
        
        [Fact]
        public void GetRequests_HasRequests_ReturnOkWithRequestst()
        {
            IActionResult actual = RequestsController(returnsValue).Requests();

            Assert.Equal(typeof(OkObjectResult), actual.GetType());
            Assert.Single((IList<UserDto>)((OkObjectResult)actual).Value);
        }

        [Fact]
        public void GetRequests_NoRequests_ReturnNotFound()
        {
            IActionResult actual = RequestsController(returnsNull).Requests();
            Assert.Equal(typeof(OkObjectResult), actual.GetType());
            Assert.Empty((IList<UserDto>)((OkObjectResult)actual).Value);
        }

        [Fact]
        public void AcceptRequest_ValidRequest_ReturnOk()
        {
            IActionResult actual = RequestsController(returnsValue).AcceptRequest(new UserDto() { Id = returnsValue });
            Assert.Equal(typeof(OkObjectResult), actual.GetType());
        }

        [Fact]
        public void AcceptRequest_InvalidRequest_ReturnNotFound()
        {
            IActionResult actual = RequestsController(returnsNull).AcceptRequest(new UserDto() { Id = returnsNull });
            Assert.Equal(typeof(NotFoundResult), actual.GetType());
        }

        [Fact]
        public void RejectRequest_ValidRequest_ReturnOk()
        {
            IActionResult actual = RequestsController(returnsValue).RejectRequest(new UserDto() { Id = returnsValue });
            Assert.Equal(typeof(OkResult), actual.GetType());
        }

        [Fact]
        public void RejectRequest_InvalidRequest_ReturnNotFound()
        {
            IActionResult actual = RequestsController(returnsNull).RejectRequest(new UserDto() { Id = returnsNull });
            Assert.Equal(typeof(NotFoundResult), actual.GetType());
        }

        [Fact]
        public void CreateRequest_NotYetExists_ReturnOk()
        {
            IActionResult actual = RequestsController(returnsValue).CreateRequest(new UserDto() { Id = returnsValue });
            Assert.Equal(typeof(OkResult), actual.GetType());
        }

        [Fact]
        public void CreateRequest_AlreadyExists_ReturnNotFound()
        {
            IActionResult actual = RequestsController(returnsNull).CreateRequest(new UserDto() { Id = returnsNull });
            Assert.Equal(typeof(NotFoundResult), actual.GetType());
        }
    }
}
