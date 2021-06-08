using System.Collections.Generic;
using SecretSanta.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Business
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext DbContext;

        public UserRepository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> Create(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            DbContext.Users.Add(item);
            await DbContext.SaveChangesAsync();
            return item;
        }

        public async Task<User?> GetItem(int id)
        {
            return await DbContext.Users.FindAsync(id);
        }

        public ICollection<User> List()
        {
            return DbContext.Users.ToList();
        }

        public async Task<bool> Remove(int id)
        {
            User? user = await GetItem(id);
            if(user is not null)
            {
                DbContext.Users.Remove(user);
                await DbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<User>> GetAssignmentUsers(int id)
        {
            IQueryable<int> assignments = DbContext.Assignments.Where(item => item.Giver.UserId == id).Select(item => item.Receiver.UserId); // this is SOOOOOOOOOO gross
            List<User> users = new();
            foreach(int receiverId in assignments)
            {
                User user = await DbContext.Users.FindAsync(receiverId); // EWWWWWWWWWW
                users.Add(user);
            }
            return users;
        }
        public List<Gift> GetGifts(int id)
        {
            return DbContext.Gifts.Where(item => item.UserId == id).ToList();
        }
        public async Task Save(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }
            await Remove(item.UserId);
            DbContext.Users.Add(item);
            await DbContext.SaveChangesAsync();
        }
    }
}
