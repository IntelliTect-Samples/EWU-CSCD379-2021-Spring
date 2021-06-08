using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.Data;
using System.Linq;

namespace SecretSanta.Business
{
    public interface IGroupRepository
    {
        ICollection<Group> List();
        Task<Group?> GetItem(int id);
        Task<bool> Remove(int id);
        Task<Group> Create(Group item);
        Task Save(Group item);
        Task<bool> AddUser(int groupId, int userId);
        Task<bool> RemoveUser(int groupId, int userId);
        Task<List<User>> GetUsers(int groupId);
        IQueryable<Assignment> GetAssignments(int groupId);
        Task<AssignmentResult> GenerateAssignments(int groupId);
    }

}
