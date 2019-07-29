using System.Web;
using System.Web.Http;
using Serilog;
using TodoListAPI;

[assembly: PreApplicationStartMethod(typeof(AppPreStart), nameof(AppPreStart.PreApplicationStart))]

namespace TodoListAPI
{
    public class AppPreStart
    {
        public static void PreApplicationStart()
        {
            LogConfig.Configure(GlobalConfiguration.Configuration);
            Log.Information("app is starting...");
        }
    }
}
