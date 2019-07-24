using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using CacheCow.Server.WebApi;
using Serilog;

namespace TodoListAPI.Caching
{
    internal class SafeHttpCache : HttpCacheAttribute
    {
        public override async Task OnActionExecutedAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
        {
            try
            {
                await base.OnActionExecutedAsync(context, cancellationToken);
            }
            catch (Exception e)
            {
                Log.Warning(e, "CacheCow HttpCacheAttribute bug, null reference exception when null returned from content negotiation");
            }
        }
    }
}