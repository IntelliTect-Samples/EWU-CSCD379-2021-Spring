using System.Collections.Generic;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Tests.Business
{
    internal class TestableUserRepository : IUserRepository
    {
        private Dictionary<int, User> Users { get; } = new();

        public User Create(User item)
        {
            Users.Add(item.UserId, item);
            return item;
        }

        public User? GetItem(int id)
        {
            Users.TryGetValue(id, out User? rv);
            return rv;
        }

        public ICollection<User> List() => Users.Values;

        public bool Remove(int id) => Users.Remove(id);

        public void Save(User item) => Users[item.UserId] = item;
    }
}
