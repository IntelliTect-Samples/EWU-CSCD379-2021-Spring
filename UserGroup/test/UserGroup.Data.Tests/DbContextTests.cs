using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserGroup.Data.Tests
{
    [TestClass]
    public class DbContextTests
    {
        [TestMethod]
        async public Task Add_NewEvent_Success()
        {
            Event @event;
            using DbContext dbContext = new DbContext();
            string titlePrefix = $"{nameof(DbContextTests)}.{nameof(Add_NewEvent_Success)}";
            async Task RemoveExistingTestEventsAsync()
            {
                // In addition to remove code here
                IQueryable<Event>? events = dbContext.Events.Where(
                    item => item.Title.StartsWith(titlePrefix));
                dbContext.Events.RemoveRange(events);
                await dbContext.SaveChangesAsync();
            }

            try
            {
                int countBefore = dbContext.Events.Count();
                // Put remove code here!
                await RemoveExistingTestEventsAsync();


                @event = new Event() {Title = $"{titlePrefix} " + Guid.NewGuid().ToString() };
                int id = @event.Id;
                dbContext.Events.Add(@event);
                Assert.AreEqual<int>(0, @event.Id);
                await dbContext.SaveChangesAsync();
                Assert.AreNotEqual<int>(id, @event.Id);
                Assert.AreEqual(countBefore + 1, dbContext.Events.Count());
            }
            finally
            {
                await RemoveExistingTestEventsAsync();
            }
        }
    }
}
