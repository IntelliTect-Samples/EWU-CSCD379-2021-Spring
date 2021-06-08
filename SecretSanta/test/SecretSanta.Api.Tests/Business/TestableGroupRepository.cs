using System;
using System.Collections.Generic;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Tests.Business
{
    internal class TestableGroupRepository : IGroupRepository
    {
        private Dictionary<int, Group> Groups { get; } = new();

        public Group Create(Group item)
        {
            Groups.Add(item.GroupId, item);
            return item;
        }

        public AssignmentResult? AssignmentResult { get; set; }
        public AssignmentResult GenerateAssignments(int groupId)
        {
            return AssignmentResult ?? throw new InvalidOperationException();
        }

        public Group? GetItem(int id)
        {
            Groups.TryGetValue(id, out Group? rv);
            return rv;
        }

        public ICollection<Group> List() => Groups.Values;

        public bool Remove(int id) => Groups.Remove(id);

        public void Save(Group item) => Groups[item.GroupId] = item;
    }
}
