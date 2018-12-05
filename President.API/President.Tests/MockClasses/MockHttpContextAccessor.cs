using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace President.Tests.MockClasses
{
    public static class MockHttpContextAccessor
    {
        public static IHttpContextAccessor Get(string userId)
        {
            var caller = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", userId),
            }));

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.SetupGet(m => m.HttpContext.User)
                .Returns(caller);

            return mockHttpContextAccessor.Object;
        }
    }
}
