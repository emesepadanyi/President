using President.DAL.Entities;
using System.Collections.Generic;

namespace President.BLL.Services
{
    public interface IRelationshipService
    {
        //GET
        List<User> GetFriends(string userId);
        List<User> GetRequests(string userId);
        List<User> FindUsers(string userId, string keyWord);

        //POST
        bool CreateRequest(string senderId, string receiverId);

        //UPDATE
        bool AcceptRequest(string senderId, string receiverId); //throws: InvalidOperationException
        bool RejectRequest(string senderId, string receiverId);

        //DELETE
        void DeleteRelationship(string senderId, string receiverId);
    }
}
