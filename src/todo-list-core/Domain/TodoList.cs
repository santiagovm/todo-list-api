using System;

namespace TodoListAPI.Domain
{
    public class TodoList : IDomainEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public int OwnerId { get; private set; }

        public static TodoList Create(string name, DateTime lastUpdatedAt, int ownerId)
        {
            return new TodoList
                   {
                       Name = name,
                       LastUpdatedAt = lastUpdatedAt,
                       OwnerId = ownerId
                   };
        }

        private TodoList()
        {
            // hiding constructor, only used by EF
        }
    }
}
