using System;

namespace TodoListAPI.Dtos
{
    /// <summary>
    /// to-do list
    /// </summary>
    public class TodoListDto
    {
        /// <summary>
        /// to-do list id
        /// </summary>
        public int Id { get; private set; }
        
        /// <summary>
        /// list name
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// last time list was updated (UTC)
        /// </summary>
        public DateTime LastUpdatedAt { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="lastUpdatedAt"></param>
        public TodoListDto(int id, string name, DateTime lastUpdatedAt)
        {
            Id = id;
            Name = name;
            LastUpdatedAt = lastUpdatedAt;
        }
    }
}
