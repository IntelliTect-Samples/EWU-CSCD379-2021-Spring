using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Data
{
    static class SampleData
    {
        public static List<User> Users()
        {
            return new List<User>
            {
                new User
                {
                    UserId = 1,
                    FirstName = "Tyler",
                    LastName = "Jones"
                },
                new User
                {
                    UserId = 2,
                    FirstName = "Jeff",
                    LastName = "Peterson"
                },
                new User
                {
                    UserId = 3,
                    FirstName = "Luke",
                    LastName = "Post"               
                }
            };
        }
        public static Group Group 
        { 
            get
            {
                Group group = new Group
                {
                    GroupId = 1,
                    Name = "Really Cool Group",
                };
                foreach(User user in Users())
                {
                    group.Users.Add(user);
                }
                return group;
            } 
        }
    }
}
