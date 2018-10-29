using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using President.API.Controllers;
using President.API.Hubs;
using President.DAL.Context;
using President.DAL.Entities;
using System;
using System.Security.Claims;
using Xunit;

namespace President.API.Tests.ControllersTests
{
    public class GameControllerTests
    {
        private GameController GameController { get; set; }
        public GameControllerTests()
        {
            GameController = new GameController(
                MockHttpContextAccessor(),
                MockPresidentDbContext(),
                MockGameContext()
            );
        }

        private IHttpContextAccessor MockHttpContextAccessor()
        {
            var caller = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", "1"),
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

            var Ede = new User { UserName = "Ede", Id = "1" };
            var Peti = new User { UserName = "Peti", Id = "2" };
            var Noncsi = new User { UserName = "Noncsi", Id = "3" };
            var Gergo = new User { UserName = "Gergo", Id = "4" };

            context.Users.Add(Ede);
            context.Users.Add(Peti);
            context.Users.Add(Noncsi);
            context.Users.Add(Gergo);

            context.SaveChanges();

            return context;
        }
        private IHubContext<GameHub, IGameHub> MockGameContext()
        {
           return new Mock<IHubContext<GameHub, IGameHub>>().Object;
        }

        [Fact]
        public void Test1()
        {

        }
    }
}
