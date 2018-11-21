using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using President.API.Controllers;
using President.API.Dtos;
using President.API.Mapping;
using President.API.Tests.MockClasses;
using President.BLL.Services;
using President.DAL.Context;
using President.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace President.API.Tests.ControllersTests
{
    public class RequestsControllerTests
    {
        //GetRequests, AcceptRequest

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
                        mockRelationshipService(),
                        MockPresidentDbContext(),
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
                        mockRelationshipService(),
                        MockPresidentDbContext(),
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
        {//returnsNull
            IActionResult actual = SadRequestsController.Requests();
            Assert.Equal(typeof(NotFoundResult), actual.GetType());
        }

        [Fact]
        public void AcceptRequest_ReturnOk()
        {
            //returnsValue
            IActionResult actual = HappyRequestsController.AcceptRequest(returnsValue);
            Assert.Equal(typeof(OkResult), actual.GetType());
        }

        [Fact]
        public void AcceptRequest_ReturnNotFound()
        {//returnsNull
            IActionResult actual = SadRequestsController.AcceptRequest(returnsNull);
            Assert.Equal(typeof(NotFoundResult), actual.GetType());
        }

        [Fact]
        public void RejectRequest_ReturnOk()
        {
            //returns
            IActionResult actual = HappyRequestsController.RejectRequest(returnsValue);
            Assert.Equal(typeof(OkResult), actual.GetType());
        }

        [Fact]
        public void RejectRequest_ReturnNotFound()
        {
            IActionResult actual = SadRequestsController.RejectRequest(returnsNull);
            Assert.Equal(typeof(NotFoundResult), actual.GetType());
        }

        [Fact]
        public void CreateRequest_ReturnOk()
        {
            IActionResult actual = HappyRequestsController.CreateRequest(returnsValue);
            Assert.Equal(typeof(OkResult), actual.GetType());
        }

        [Fact]
        public void CreateRequest_ReturnNotFound()
        {
            IActionResult actual = SadRequestsController.CreateRequest(returnsNull);
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

        static private RelationshipService mockRelationshipService()
        {
            var mock = new Mock<RelationshipServiceWithMockDb>();
            mock.Setup(service => service.GetRequests(returnsValue)).Returns(new List<User>() { new User() { FirstName = "Eszter" } });
            mock.Setup(service => service.GetRequests(returnsNull)).Returns((List<User>)null);

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
        private PresidentDbContext MockPresidentDbContext()
        {
            var options = new DbContextOptionsBuilder<PresidentDbContext>()
                             .UseInMemoryDatabase(Guid.NewGuid().ToString())
                             .Options;
            var context = new PresidentDbContext(options);

            context.Users.Add(new User { FirstName = "Fanni", Id = "1" });
            context.Users.Add(new User { FirstName = "Eszter", Id = "2" });

            context.SaveChanges();

            return context;
        }
    }
}
