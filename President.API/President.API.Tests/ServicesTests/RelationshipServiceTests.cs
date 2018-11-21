using President.API.Tests.MockClasses;
using System;
using Xunit;

namespace President.API.Tests.ServicesTests
{
    public class RelationshipServiceTests
    {
        [Fact]
        public void GetFriends_ZeroFriends()
        {
            var service = new RelationshipServiceWithMockDb();
            var FannisFriends = service.GetFriends("1");
            Assert.Empty(FannisFriends);
        }

        [Fact]
        public void GetFriends_OneSended()
        {
            var service = new RelationshipServiceWithMockDb();
            var EsztersFriends = service.GetFriends("2");
            Assert.Single(EsztersFriends);
            Assert.NotNull(EsztersFriends.ToArray()[0]);
            Assert.Equal("Noncsi", EsztersFriends.ToArray()[0].FirstName);
        }

        [Fact]
        public void GetFriends_OneRecieved()
        {
            var service = new RelationshipServiceWithMockDb();
            var NoncsisFriends = service.GetFriends("3");
            Assert.Single(NoncsisFriends);
        }

        //GET REQUESTS
        [Fact]
        public void GetRequests_ZeroRequests()
        {
            var service = new RelationshipServiceWithMockDb();
            var BlankasRequests = service.GetRequests("4");
            Assert.Empty(BlankasRequests);
        }

        [Fact]
        public void GetRequests_OneSended()
        {
            var service = new RelationshipServiceWithMockDb();
            var FannisRequests = service.GetRequests("1");
            Assert.Empty(FannisRequests);
        }

        [Fact]
        public void GetRequests_OneRecieved()
        {
            var service = new RelationshipServiceWithMockDb();
            var EsztersRequests = service.GetRequests("2");
            Assert.Single(EsztersRequests);
            Assert.NotNull(EsztersRequests.ToArray()[0]);
            Assert.Equal("Fanni", EsztersRequests.ToArray()[0].FirstName);
        }

        //ACCEPT REQUESTS
        [Fact]
        public void AcceptRequest_RequestNotFound()
        {
            var service = new RelationshipServiceWithMockDb();
            Assert.Throws<InvalidOperationException>(() => service.AcceptRequest("2", "3"));
        }

        [Fact]
        public void AcceptRequest_RequestFound()
        {
            var service = new RelationshipServiceWithMockDb();
            service.AcceptRequest("1", "2");
            var FannisFriends = service.GetFriends("1");
            var EsztersFriends = service.GetFriends("2");
            Assert.Single(FannisFriends);
            Assert.Equal(2, EsztersFriends.Count);
        }

        //REJECT REQUEST
        [Fact]
        public void RejectRequest_RequestNotFound()
        {
            var service = new RelationshipServiceWithMockDb();
            Assert.Throws<InvalidOperationException>(() => service.RejectRequest("2", "3"));
        }

        [Fact]
        public void RejectRequest_RequestFound()
        {
            var service = new RelationshipServiceWithMockDb();
            service.RejectRequest("1", "2");
            var FannisFriends = service.GetFriends("1");
            var EsztersFriends = service.GetFriends("2");
            Assert.Empty(FannisFriends);
            Assert.Single(EsztersFriends);
        }
    }
}
