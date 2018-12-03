using President.DAL.Context;
using President.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace President.BLL.Services
{
    public class RelationshipService : IRelationshipService
    {
        private PresidentDbContext context;

        public RelationshipService(PresidentDbContext _context)
        {
            context = _context;
        }

        public virtual string GetUser(ClaimsPrincipal caller)
        {
            var userID = caller.Claims.Single(c => c.Type == "id");
            var user = context.Users.Single(dbUser => dbUser.Id == userID.Value);
            return user.Id;
        }

        //GET
        public virtual List<User> GetFriends(string userId)
        {
            return context.Relationships.
                Where((rel) => ((rel.Sender.Id == userId || rel.Receiver.Id == userId) && rel.Status == "accepted")).
                Select(rel => rel.Sender.Id != userId ? rel.Sender : rel.Receiver).
                ToList();
        }

        public virtual List<User> GetRequests(string userId)
        {
            return context.Relationships.
                Where((rel) => (rel.Receiver.Id == userId && rel.Status == "requested")).
                Select(rel => rel.Sender).
                ToList();
        }

        public virtual List<User> FindUsers(string userId, string keyWord)
        {
            var finalUsers = new List<User>();
            context.Users.
                Where(user => user.Id != userId &&
                          (user.FirstName.Contains(keyWord) ||
                          user.LastName.Contains(keyWord) ||
                          user.UserName.Contains(keyWord))).ToList()
                .ForEach(user =>
                {
                    if (context.Relationships.Where(rel =>
                            (rel.Sender.Id == userId && rel.Receiver.Id == user.Id) ||
                            (rel.Sender.Id == user.Id && rel.Receiver.Id == userId)).FirstOrDefault() == null)
                    {
                        finalUsers.Add(user);
                    }
                });

            return finalUsers;
        }

        public virtual bool AcceptRequest(string senderId, string receiverId)
        {
            context.Relationships.
                Where(rel => (rel.Sender.Id == senderId && rel.Receiver.Id == receiverId && rel.Status == "requested")).
                First().
                Status = "accepted";
            context.SaveChanges();
            return true;
        }

        public virtual bool RejectRequest(string senderId, string receiverId)
        {
            context.Relationships.
                Where(rel => (rel.Sender.Id == senderId && rel.Receiver.Id == receiverId && rel.Status == "requested")).
                First().
                Status = "rejected";
            context.SaveChanges();
            return true;
        }

        public virtual bool CreateRequest(string senderId, string receiverId)
        {
            if (context.Relationships.Where(rel =>
                     (rel.Sender.Id == senderId && rel.Receiver.Id == receiverId) ||
                     (rel.Sender.Id == receiverId && rel.Receiver.Id == senderId)).
                 FirstOrDefault() == null)
            {
                var sender = context.Users.Where(user => user.Id == senderId).First();
                var receiver = context.Users.Where(user => user.Id == receiverId).First();
                context.Relationships.Add(new Relationship { Sender = sender, Receiver = receiver, Status = "requested" });
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public virtual void DeleteRelationship(string senderId, string receiverId)
        {
            throw new NotImplementedException();
        }
    }
}
