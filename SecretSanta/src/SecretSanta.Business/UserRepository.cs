using System.Collections.Generic;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class UserRepository : IUserRepository
    {
        private Dictionary<int, User> Users { get; } = new();

        public User Create(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            Users[item.Id] = item;
            return item;
        }

        public User? GetItem(int id)
        {
            if (Users.TryGetValue(id, out User? user))
            {
                return user;
            }
            return null;
        }

        public ICollection<User> List()
        {
            return Users.Values;
        }

        public bool Remove(int id)
        {
            return Users.Remove(id);
        }

        public void Save(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            Users[item.Id] = item;
        }
    }
}
