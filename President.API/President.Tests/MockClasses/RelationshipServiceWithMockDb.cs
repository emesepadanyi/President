using Microsoft.EntityFrameworkCore;
using President.BLL.Services;
using President.DAL.Context;
using President.DAL.Entities;
using System;

namespace President.Tests.MockClasses
{
    public class RelationshipServiceWithMockDb : RelationshipService
    {
        public RelationshipServiceWithMockDb(PresidentDbContext c) : base(DbInit())
        {
        }

        public RelationshipServiceWithMockDb() : base(DbInit())
        {
        }

        private static PresidentDbContext DbInit()
        {
            var options = new DbContextOptionsBuilder<PresidentDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new PresidentDbContext(options);

            var Fanni = new User { FirstName = "Fanni", Id = "1" };
            var Eszter = new User { FirstName = "Eszter", Id = "2" };
            var Noncsi = new User { FirstName = "Noncsi", Id = "3" };
            var Blanka = new User { FirstName = "Blanka", Id = "4" };

            context.Users.Add(Fanni);
            context.Users.Add(Eszter);
            context.Users.Add(Noncsi);
            context.Users.Add(Blanka);

            context.Relationships.Add(new Relationship { Sender = Fanni, Receiver = Eszter, Status = "requested" });
            context.Relationships.Add(new Relationship { Sender = Eszter, Receiver = Noncsi, Status = "accepted" });
            context.Relationships.Add(new Relationship { Sender = Blanka, Receiver = Noncsi, Status = "rejected" });

            context.SaveChanges();

            return context;
        }
    }
}
