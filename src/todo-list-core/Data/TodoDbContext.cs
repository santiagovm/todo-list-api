using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoListAPI.Domain;
using TodoListAPI.Interfaces;

namespace TodoListAPI.Data
{
    public class TodoDbContext : DbContext
    {
        public DbSet<TodoList> TodoLists { get; set; }

        public TodoDbContext(DbContextOptions options, ISessionInfo session)
            : base(options)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        #region Overrides of DbContext

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(executingAssembly);

            modelBuilder.CreateTrackingPropertiesFromAssembly(executingAssembly);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SetTrackingProperties();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetTrackingProperties();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetTrackingProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
                                                   CancellationToken cancellationToken = new CancellationToken())
        {
            SetTrackingProperties();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        #endregion

        private void SetTrackingProperties()
        {
            IEnumerable<EntityEntry> addedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            IEnumerable<EntityEntry> modifiedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);

            DateTime utcNow = DateTime.UtcNow;

            foreach (EntityEntry entry in addedEntries)
            {
                entry.Property(TrackingProperties.CREATED_BY).CurrentValue = _session.Username;
                entry.Property(TrackingProperties.CREATED_AT_UTC).CurrentValue = utcNow;
                entry.Property(TrackingProperties.LAST_UPDATED_BY).CurrentValue = null;
                entry.Property(TrackingProperties.LAST_UPDATED_AT_UTC).CurrentValue = null;
            }

            foreach (EntityEntry entry in modifiedEntries)
            {
                entry.Property(TrackingProperties.LAST_UPDATED_BY).CurrentValue = _session.Username;
                entry.Property(TrackingProperties.LAST_UPDATED_AT_UTC).CurrentValue = utcNow;
            }
        }

        private readonly ISessionInfo _session;
    }
}
