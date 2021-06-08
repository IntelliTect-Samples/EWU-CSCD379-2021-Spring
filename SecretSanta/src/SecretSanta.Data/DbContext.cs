using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext = SecretSanta.Data.DbContext;
using System.Linq;

namespace SecretSanta.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options)
            : base(options)
        { }

        public DbContext()
           : base(new DbContextOptionsBuilder<DbContext>().UseSqlite("Data Source=main.db").Options)
        { }


        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Assignment> Assignments => Set<Assignment>();
        public DbSet<Gift> Gifts => Set<Gift>();
        public DbSet<GroupUser> GroupUsers => Set<GroupUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<GroupUser>()
                .HasKey(gu => new { gu.GroupId, gu.UserId });
            
            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.Group)
                .WithMany(u => u.GroupUser);

            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.User)
                .WithMany(u => u.GroupUser);
            // modelBuilder.Entity<User>()
            //     .HasAlternateKey(user => new { user.FirstName, user.LastName });
            // modelBuilder.Entity<Group>()
            //     .HasAlternateKey(group => group.Name);
            // modelBuilder.Entity<Gift>()
            //     .HasAlternateKey(gift => new { gift.Name, gift.UserId, gift.Priority });


            //modelBuilder.Entity<Group>().HasData(SampleData.Group);
            modelBuilder.Entity<User>().HasData(SampleData.Users());

        }
    }
}
