using System;
using System.Collections.Generic;
using SecretSanta.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Business
{
    public class GiftRepository : IGiftRepository
    {
        private readonly DbContext DbContext;
        public GiftRepository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public ICollection<Gift> List(int userId)
        {
            return DbContext.Gifts.Where(item => item.UserId == userId).ToList();
        }

        public async Task<Gift> Create(Gift item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await DbContext.Gifts.AddAsync(item);
            await DbContext.SaveChangesAsync();
            return item;
        }
    }
}
