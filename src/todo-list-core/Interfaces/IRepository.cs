using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TodoListAPI.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();

        EntityEntry<T> Add(T item);

        void SaveChanges();
    }
}
