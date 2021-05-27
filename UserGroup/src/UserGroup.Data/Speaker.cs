using System.Collections.Generic;

namespace UserGroup.Data
{
    public class Speaker
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public override string ToString()
        {
            return $"{Id} {FirstName} {LastName}";
        }

        List<Event> Events = new();
    }
}
