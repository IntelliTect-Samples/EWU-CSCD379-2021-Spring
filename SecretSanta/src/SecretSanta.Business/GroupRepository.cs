using System;
using System.Collections.Generic;
using SecretSanta.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Business
{
    public class GroupRepository : IGroupRepository
    {
        private readonly DbContext DbContext;
        public GroupRepository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Group> Create(Group item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await DbContext.Groups.AddAsync(item);
            await DbContext.SaveChangesAsync();
            return item;
        }

        public async Task<Group?> GetItem(int id)
        {
            return await DbContext.Groups.FindAsync(id);
        }

        public ICollection<Group> List()
        {
            return DbContext.Groups.ToList();
        }

        public async Task<bool> Remove(int id)
        {
            Group group = DbContext.Groups.Find(id);
            if(group is not null)
            {
                List<GroupUser> groupUsers = DbContext.GroupUsers.Where(item => item.GroupId == id).ToList();
                foreach(GroupUser groupUser in groupUsers)
                {
                    DbContext.GroupUsers.Remove(groupUser);
                }
                List<Assignment> assignments = DbContext.Assignments.Where(item => item.GroupId == id).ToList();
                foreach(Assignment assignment in assignments)
                {
                    DbContext.Assignments.Remove(assignment);
                }
                await DbContext.SaveChangesAsync();
                DbContext.Groups.Remove(group);
                await DbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task Save(Group item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (await Remove(item.GroupId))
            {
                await DbContext.Groups.AddAsync(item);
                await DbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> AddUser(int groupId, int userId)
        {
            Group? group = await DbContext.Groups.FindAsync(groupId);
            User? user = await DbContext.Users.FindAsync(userId);
            if(user is not null && group is not null)
            {
                GroupUser groupUser = new(){
                    GroupId = groupId,
                    UserId = userId
                };
                await DbContext.GroupUsers.AddAsync(groupUser);
                await DbContext.SaveChangesAsync();

                group = await DbContext.Groups.FindAsync(groupId);
                System.Console.WriteLine(group.Users.Count());


                return true;
            }
            return false;
        }

        public async Task<bool> RemoveUser(int groupId, int userId)
        {
            Group? group = await DbContext.Groups.FindAsync(groupId);
            User? user = await DbContext.Users.FindAsync(userId);
            if(user is not null && group is not null)
            {
                GroupUser groupUser = new(){
                    GroupId = groupId,
                    UserId = userId
                };
                DbContext.GroupUsers.Remove(groupUser);
                await DbContext.SaveChangesAsync();
                if(DbContext.GroupUsers.Where(item => item.GroupId == groupId).Count() < 3)
                {
                    await RemovePreviousAssignments(groupId);
                } else{
                    await GenerateAssignments(groupId);
                }

                return true;
            }
            return false;
        }

        public async Task<List<User>> GetUsers(int groupId)
        {
            IQueryable<GroupUser> groupUsers = DbContext.GroupUsers.Where(item => item.GroupId == groupId);
            List<User> users = new();
            foreach(GroupUser groupUser in groupUsers)
            {
                User user = await DbContext.Users.FindAsync(groupUser.UserId);
                users.Add(user);
            }
            return users;
        }

        public IQueryable<Assignment> GetAssignments(int groupId)
        {
            return DbContext.Assignments.Where(item => item.GroupId == groupId);
        }

        public async Task<AssignmentResult> GenerateAssignments(int groupId)
        {
            await RemovePreviousAssignments(groupId);
            IQueryable<GroupUser> manyMany = DbContext.GroupUsers.Where(item => item.GroupId == groupId);
            Group group = await DbContext.Groups.FindAsync(groupId);
            if (group is null)
            {
                return AssignmentResult.Error("Group not found");
            }

            Random random = new();
            var groupUsers = new List<User>((await GetUsers(groupId)).ToList());

            if (manyMany.Count() < 3)
            {
                return AssignmentResult.Error($"Group {group.Name} must have at least three users");
            }

            var users = new List<User>();
            //Put the users in a random order
            while(groupUsers.Count > 0)
            {
                int index = random.Next(groupUsers.Count);
                users.Add(groupUsers[index]);
                groupUsers.RemoveAt(index);
            }
            //The assignments are created by linking the current user to the next user.
            group.Assignments.Clear();
            for(int i = 0; i < users.Count; i++)
            {
                int endIndex = (i + 1) % users.Count;
                group.Assignments.Add(new Assignment(){
                    Giver = users[i],
                    Receiver = users[endIndex]
                });
                System.Console.WriteLine($"Giver {group.Assignments[i].Giver.FirstName}");
                await DbContext.Assignments.AddAsync(group.Assignments[i]);
                await DbContext.SaveChangesAsync();
            }
            return AssignmentResult.Success();
        }

        private async Task RemovePreviousAssignments(int groupId)
        {
            IQueryable<Assignment> assignments = DbContext.Assignments.Where(item => item.GroupId == groupId);
            foreach(Assignment assignment in assignments)
            {
                DbContext.Assignments.Remove(assignment);
            }
            await DbContext.SaveChangesAsync();
        }
    }
}
