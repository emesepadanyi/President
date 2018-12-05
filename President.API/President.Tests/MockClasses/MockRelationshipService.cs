using Moq;
using President.BLL.Services;
using President.DAL.Entities;
using System;
using System.Collections.Generic;

namespace President.Tests.MockClasses
{
    public static class MockRelationshipService
    {
        const string returnsValue = "1";
        const string returnsNull = "2";
        public static IRelationshipService Get(string userId)
        {
            var mock = new Mock<RelationshipServiceWithMockDb>() { CallBase = true };
            mock.Setup(service => service.GetRequests(returnsValue)).Returns(new List<User>() { new User() { UserName = "Eszter" } });
            mock.Setup(service => service.GetRequests(returnsNull)).Returns(new List<User>());

            mock.Setup(service => service.AcceptRequest(returnsValue, returnsValue)).Returns(true);
            mock.Setup(service => service.AcceptRequest(returnsNull, returnsNull)).Throws<InvalidOperationException>();

            mock.Setup(service => service.RejectRequest(returnsValue, returnsValue)).Returns(true);
            mock.Setup(service => service.RejectRequest(returnsNull, returnsNull)).Throws<InvalidOperationException>();

            mock.Setup(service => service.CreateRequest(returnsValue, returnsValue)).Returns(true);
            mock.Setup(service => service.CreateRequest(returnsNull, returnsNull)).Returns(false);
            return mock.Object;
        }
    }
}
