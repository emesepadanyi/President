using President.Tests.MockClasses;
using System;
using System.Linq;
using Xunit;

namespace President.Tests.ServicesTests
{
    public class RelationshipServiceTests
    {
        //GET FRIENDS
        [Fact]
        public void GetFriends_ZeroFriends_ZeroFriendReturned()
        {
            var service = new RelationshipServiceWithMockDb();
            var FannisFriends = service.GetFriends("1");
            Assert.Empty(FannisFriends);
        }

        [Fact]
        public void GetFriends_OneSended_OneFriendReturned()
        {
            var service = new RelationshipServiceWithMockDb();
            var EsztersFriends = service.GetFriends("2");
            Assert.Single(EsztersFriends);
            Assert.NotNull(EsztersFriends.ToArray()[0]);
            Assert.Equal("Noncsi", EsztersFriends.ToArray()[0].UserName);
        }

        [Fact]
        public void GetFriends_OneReceived_OneFriendReturned()
        {
            var service = new RelationshipServiceWithMockDb();
            var NoncsisFriends = service.GetFriends("3");
            Assert.Single(NoncsisFriends);
        }

        //GET REQUESTS
        [Fact]
        public void GetRequests_ZeroRequests_ZeroRequestReturned()
        {
            var service = new RelationshipServiceWithMockDb();
            var BlankasRequests = service.GetRequests("4");
            Assert.Empty(BlankasRequests);
        }

        [Fact]
        public void GetRequests_OneSended_ZeroRequestReturned()
        {
            var service = new RelationshipServiceWithMockDb();
            var FannisRequests = service.GetRequests("1");
            Assert.Empty(FannisRequests);
        }

        [Fact]
        public void GetRequests_OneReceived_OneUserReturned()
        {
            var service = new RelationshipServiceWithMockDb();
            var EsztersRequests = service.GetRequests("2");
            Assert.Single(EsztersRequests);
            Assert.Equal("Fanni", EsztersRequests.ToArray()[0].UserName);
        }

        //FIND FRIENDS
        [Fact]
        public void FindUsers_NoMatch_EmptyResoults()
        {
            var service = new RelationshipServiceWithMockDb();
            var resoults = service.FindUsers("2", "ABCDE");
            Assert.Empty(resoults);
        }

        [Fact]
        public void FindUsers_OneMatch_SomeResoults()
        {
            var service = new RelationshipServiceWithMockDb();
            var resoults = service.FindUsers("2", "a");
            Assert.Single(resoults);
        }

        //ACCEPT REQUESTS
        [Fact]
        public void AcceptRequest_InvalidRequest_ThrowError()
        {
            var service = new RelationshipServiceWithMockDb();
            Assert.Throws<InvalidOperationException>(() => service.AcceptRequest("2", "3"));
        }

        [Fact]
        public void AcceptRequest_ValidRequest_RequestFound()
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
        public void RejectRequest_InvalidRequest_ThrowError()
        {
            var service = new RelationshipServiceWithMockDb();
            Assert.Throws<InvalidOperationException>(() => service.RejectRequest("2", "3"));
        }

        [Fact]
        public void RejectRequest_ValidRequest_RequestFound()
        {
            var service = new RelationshipServiceWithMockDb();
            service.RejectRequest("1", "2");
            var FannisFriends = service.GetFriends("1");
            var EsztersFriends = service.GetFriends("2");
            Assert.Empty(FannisFriends);
            Assert.Single(EsztersFriends);
        }

        [Fact]
        public void CreateRequest_RequestAlreadyExists_ReturnFalse()
        {
            var service = new RelationshipServiceWithMockDb();
            var actual = service.CreateRequest("1", "2");
            Assert.False(actual);
        }

        [Fact]
        public void CreateRequest_NewRequest_ReturnTrue()
        {
            var service = new RelationshipServiceWithMockDb();
            var numberOfRelationships = RelationshipServiceWithMockDb.Context.Relationships.Count();
            var actual = service.CreateRequest("1", "4");
            Assert.True(actual);
            Assert.Equal(numberOfRelationships+1, RelationshipServiceWithMockDb.Context.Relationships.Count());
        }
    }
}
