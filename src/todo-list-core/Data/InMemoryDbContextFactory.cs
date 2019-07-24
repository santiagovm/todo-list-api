using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using TodoListAPI.Interfaces;

namespace TodoListAPI.Data
{
    public class InMemoryDbContextFactory
    {
        public static async Task<DbContext> Create(ISessionInfo sessionInfo)
        {
            if (_dbContextOptions != null)
            {
                return new TodoDbContext(_dbContextOptions, sessionInfo);
            }

            _dbContextOptions = await CreateInMemoryContextOptions();

            var context = new TodoDbContext(_dbContextOptions, sessionInfo);
                
            await context.Database.EnsureCreatedAsync();

            return context;
        }

        private static async Task<DbContextOptions> CreateInMemoryContextOptions()
        {
            var connection = new SqliteConnection("DataSource=:memory:");

            await connection.OpenAsync();

            ILoggerFactory serilogFactory = new LoggerFactory().AddSerilog();

            return new DbContextOptionsBuilder<TodoDbContext>().EnableDetailedErrors()
                                                               .EnableSensitiveDataLogging()
                                                               .UseSqlite(connection)
                                                               .UseLoggerFactory(serilogFactory)
                                                               .Options;
        }
        
        private static DbContextOptions _dbContextOptions;
    }
}
