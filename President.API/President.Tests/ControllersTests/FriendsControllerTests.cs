using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using President.API.Controllers;
using President.BLL.Dtos;
using President.BLL.Services;
using President.DAL.Entities;
using President.Tests.MockClasses;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace President.Tests.ControllersTests
{

    public class FriendsControllerTests
    {
        const string returnsValue = "1";
        const string returnsNull = "2";
        private static FriendsController happyFriendsController;

        public FriendsController HappyFriendsController
        {
            get
            {
                if (happyFriendsController == null)
                {
                    happyFriendsController = new FriendsController(
                        HappyMockHttpContextAccessor(),
                        MockRelationshipService(),
                        MapperConfig.MapperInstance
                    );
                }
                return happyFriendsController;
            }
        }

        private static FriendsController sadFriendsController;

        public FriendsController SadFriendsController
        {
            get
            {
                if (sadFriendsController == null)
                {
                    sadFriendsController = new FriendsController(
                        SadMockHttpContextAccessor(),
                        MockRelationshipService(),
                        MapperConfig.MapperInstance
                    );
                }
                return sadFriendsController;
            }
        }

        [Fact]
        public void GetFriends_OneFriend_ReturnsOkObject()
        {
            IActionResult actual = HappyFriendsController.GetFriends();

            Assert.Equal(typeof(OkObjectResult), actual.GetType());
            Assert.Single((IList<UserDto>)((OkObjectResult)actual).Value);
        }

        [Fact]
        public void GetFriends_NoFriends_ReturnsOkObject()
        {
            IActionResult actual = SadFriendsController.GetFriends();

            Assert.Equal(typeof(OkObjectResult), actual.GetType());
            Assert.Empty((IList<UserDto>)((OkObjectResult)actual).Value);
        }

        [Fact]
        public void FindFriends_OneResult_ReturnsOkObject()
        {
            IActionResult actual = HappyFriendsController.FindFriends("a");

            Assert.Equal(typeof(OkObjectResult), actual.GetType());
            Assert.Single((IList<UserDto>)((OkObjectResult)actual).Value);
        }

        [Fact]
        public void FindFriends_NoResults_ReturnsOkObject()
        {
            IActionResult actual = SadFriendsController.FindFriends("a");

            Assert.Equal(typeof(OkObjectResult), actual.GetType());
            Assert.Empty((IList<UserDto>)((OkObjectResult)actual).Value);
        }

        static private RelationshipService MockRelationshipService()
        {
            var mock = new Mock<RelationshipServiceWithMockDb>() { CallBase = true };
            mock.Setup(service => service.GetFriends(returnsValue)).Returns(new List<User>() { new User() { FirstName = "Eszter" } });
            mock.Setup(service => service.GetFriends(returnsNull)).Returns(new List<User>());

            mock.Setup(service => service.FindUsers(returnsValue, "a")).Returns(new List<User>() { new User() { FirstName = "Eszter" } });
            mock.Setup(service => service.FindUsers(returnsNull, "a")).Returns(new List<User>());

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
