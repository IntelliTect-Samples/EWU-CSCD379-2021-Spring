using System.Collections.Generic;

namespace SecretSanta.Data
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public List<Gift> Gifts { get; } = new();
        public List<Group> Groups { get; } = new();

        public IList<GroupUser> GroupUser {get; set;}
    }
}
