using System.Web;
using Serilog;
using TodoListAPI;

[assembly: PreApplicationStartMethod(typeof(AppPostShutdown), nameof(AppPostShutdown.PostApplicationShutdown))]

namespace TodoListAPI
{
    public class AppPostShutdown
    {
        public static void PostApplicationShutdown()
        {
            Log.Debug("closing the logger...");
            Log.CloseAndFlush();
        }
    }
}
