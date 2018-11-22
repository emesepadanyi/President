using President.DAL.Context;
using President.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace President.BLL.Services
{
    public class RelationshipService : IRelationshipService
    {
        private PresidentDbContext context;

        public RelationshipService(PresidentDbContext _context)
        {
            context = _context;
        }
        
        //GET
        virtual public List<User> GetFriends(string userId)
        {
            return context.Relationships.
                Where((rel) => ((rel.Sender.Id == userId || rel.Receiver.Id == userId) && rel.Status == "accepted")).
                Select(rel => rel.Sender.Id != userId ? rel.Sender : rel.Receiver).
                ToList();
        }

        virtual public List<User> GetRequests(string userId)
        {
            return context.Relationships.
                Where((rel) => (rel.Receiver.Id == userId && rel.Status == "requested")).
                Select(rel => rel.Sender).
                ToList();
        }

        virtual public List<User> FindUsers(string userId, string keyWord)
        {
            return context.Users.
                Where(user => user.Id != userId &&
                          (user.FirstName.Contains(keyWord) ||
                          user.LastName.Contains(keyWord) ||
                          user.UserName.Contains(keyWord))).ToList();
        }

        virtual public bool AcceptRequest(string senderId, string receiverId)
        {
            context.Relationships.
                Where(rel => (rel.Sender.Id == senderId && rel.Receiver.Id == receiverId && rel.Status == "requested")).
                First().
                Status = "accepted";
            context.SaveChanges();
            return true;
        }

        virtual public bool RejectRequest(string senderId, string receiverId)
        {
            context.Relationships.
                Where(rel => (rel.Sender.Id == senderId && rel.Receiver.Id == receiverId && rel.Status == "requested")).
                First().
                Status = "rejected";
            context.SaveChanges();
            return true;
        }

        virtual public bool CreateRequest(string senderId, string receiverId)
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

        virtual public void DeleteRelationship(string senderId, string receiverId)
        {
            throw new NotImplementedException();
        }
    }
}
