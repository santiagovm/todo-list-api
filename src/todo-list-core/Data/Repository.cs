using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoListAPI.Interfaces;

namespace TodoListAPI.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public Repository(DbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        #region Implementation of IRepository<out T>

        public IQueryable<T> Query()
        {
            return _db.Set<T>();
        }

        public EntityEntry<T> Add(T item)
        {
            return _db.Set<T>().Add(item);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        #endregion

        private readonly DbContext _db;
    }
}
