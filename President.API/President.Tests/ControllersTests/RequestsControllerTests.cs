using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using President.API.Controllers;
using President.BLL.Dtos;
using President.BLL.Mapping;
using President.Tests.MockClasses;
using President.BLL.Services;
using President.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace President.Tests.ControllersTests
{
    public class RequestsControllerTests
    {
        const string returnsValue = "1";
        const string returnsNull = "2";
        private static RequestsController happyRequestsController;

        public RequestsController HappyRequestsController
        {
            get
            {
                if (happyRequestsController == null)
                {
                    happyRequestsController = new RequestsController(
                        HappyMockHttpContextAccessor(),
                        MockRelationshipService(),
                        MapperInstance
                    );
                }
                return happyRequestsController;
            }
        }

        private static RequestsController sadRequestsController;

        public RequestsController SadRequestsController
        {
            get
            {
                if (sadRequestsController == null)
                {
                    sadRequestsController = new RequestsController(
                        SadMockHttpContextAccessor(),
                        MockRelationshipService(),
                        MapperInstance
                    );
                }
                return sadRequestsController;
            }
        }

        [Fact]
        public void GetRequests_ReturnOkWithRequestst()
        {
            IActionResult actual = HappyRequestsController.Requests();

            Assert.Equal(typeof(OkObjectResult), actual.GetType());
            Assert.Equal(1, ((IList<UserDto>)((OkObjectResult)actual).Value).Count);
        }

        [Fact]
        public void GetRequests_ReturnNotFound()
        {
            IActionResult actual = SadRequestsController.Requests();
            Assert.Equal(typeof(OkObjectResult), actual.GetType());
            Assert.Empty((IList<UserDto>)((OkObjectResult)actual).Value);
        }

        [Fact]
        public void AcceptRequest_ReturnOk()
        {
            IActionResult actual = HappyRequestsController.AcceptRequest(new UserDto() { Id = returnsValue });
            Assert.Equal(typeof(OkObjectResult), actual.GetType());
        }

        [Fact]
        public void AcceptRequest_ReturnNotFound()
        {
            IActionResult actual = SadRequestsController.AcceptRequest(new UserDto() { Id = returnsNull });
            Assert.Equal(typeof(NotFoundResult), actual.GetType());
        }

        [Fact]
        public void RejectRequest_ReturnOk()
        {
            IActionResult actual = HappyRequestsController.RejectRequest(new UserDto() { Id = returnsValue });
            Assert.Equal(typeof(OkResult), actual.GetType());
        }

        [Fact]
        public void RejectRequest_ReturnNotFound()
        {
            IActionResult actual = SadRequestsController.RejectRequest(new UserDto() { Id = returnsNull });
            Assert.Equal(typeof(NotFoundResult), actual.GetType());
        }

        [Fact]
        public void CreateRequest_ReturnOk()
        {
            IActionResult actual = HappyRequestsController.CreateRequest(new UserDto() { Id = returnsValue });
            Assert.Equal(typeof(OkResult), actual.GetType());
        }

        [Fact]
        public void CreateRequest_ReturnNotFound()
        {
            IActionResult actual = SadRequestsController.CreateRequest(new UserDto() { Id = returnsNull });
            Assert.Equal(typeof(NotFoundResult), actual.GetType());
        }

        private static string mapper;

        public IMapper MapperInstance
        {
            get
            {
                if(mapper == null)
                {
                    var mappings = new MapperConfigurationExpression();
                    mappings.AddProfile<AutoMapperProfile>();
                    Mapper.Initialize(mappings);
                    mapper = "ready";
                }
                return Mapper.Instance;
            }
        }

        static private RelationshipService MockRelationshipService()
        {
            var mock = new Mock<RelationshipServiceWithMockDb>() { CallBase = true };
            mock.Setup(service => service.GetRequests(returnsValue)).Returns(new List<User>() { new User() { FirstName = "Eszter" } });
            mock.Setup(service => service.GetRequests(returnsNull)).Returns(new List<User>());

            mock.Setup(service => service.AcceptRequest(returnsValue, returnsValue)).Returns(true);
            mock.Setup(service => service.AcceptRequest(returnsNull, returnsNull)).Throws<InvalidOperationException>();

            mock.Setup(service => service.RejectRequest(returnsValue, returnsValue)).Returns(true);
            mock.Setup(service => service.RejectRequest(returnsNull, returnsNull)).Throws<InvalidOperationException>();

            mock.Setup(service => service.CreateRequest(returnsValue, returnsValue)).Returns(true);
            mock.Setup(service => service.CreateRequest(returnsNull, returnsNull)).Returns(false);
            return mock.Object;
        }

        private IHttpContextAccessor HappyMockHttpContextAccessor()
        {
            var caller = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", returnsValue),
            }));

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.SetupGet(m => m.HttpContext.User)
                .Returns(caller);

            return mockHttpContextAccessor.Object;
        }

        private IHttpContextAccessor SadMockHttpContextAccessor()
        {
            var caller = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", returnsNull),
            }));

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.SetupGet(m => m.HttpContext.User)
                .Returns(caller);

            return mockHttpContextAccessor.Object;
        }
    }
}
