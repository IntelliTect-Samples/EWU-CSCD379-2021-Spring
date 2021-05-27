using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext = UserGroup.Data.DbContext;

namespace UserGroup.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext()
            : base(new DbContextOptionsBuilder<DbContext>().UseSqlite("Data Source=main.db").Options)
        { }

        public DbSet<Event> Events => Set<Event>();
        public DbSet<Speaker> Speakers => Set<Speaker>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            //modelBuilder.Entity<Event>().HasAlternateKey(
            //    item => new { item.Date, item.Location }
            //); //.HasName($"{nameof(Event)}.AlternateKey");
            modelBuilder.Entity<Event>().HasIndex(item => new { item.Date, item.Location }).IsUnique();
        }
    }
}
