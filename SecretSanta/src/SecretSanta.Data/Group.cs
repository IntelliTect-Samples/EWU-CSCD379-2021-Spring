using System.Collections.Generic;

namespace SecretSanta.Data
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = "";

        public List<User> Users { get; } = new();
        public List<Assignment> Assignments { get; } = new();

        public IList<GroupUser> GroupUser {get; set;}
    }
}
