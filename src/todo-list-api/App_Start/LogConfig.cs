using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Serilog;
using Serilog.Events;
using SerilogWeb.Classic;
using SerilogWeb.Classic.Enrichers;
using SerilogWeb.Classic.WebApi.Enrichers;
using TodoListAPI.Logging;

namespace TodoListAPI
{
    public class LogConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            ConfigureSerilog();
            
            // global exception handler
            config.Services.Replace(typeof(IExceptionHandler), new OopsExceptionHandler());

            Log.Debug("logger configured");
        }

        private static void ConfigureSerilog()
        {
            ApplicationLifecycleModule.LogPostedFormData = LogPostedFormDataOption.OnlyOnError;
            ApplicationLifecycleModule.FormDataLoggingLevel = LogEventLevel.Debug;
            ApplicationLifecycleModule.RequestLoggingLevel = LogEventLevel.Debug;

            LoggerConfiguration loggerConfiguration =
                new LoggerConfiguration().ReadFrom.AppSettings()
                                         .Enrich.FromLogContext()
                                         .Enrich.With<HttpRequestIdEnricher>()
                                         .Enrich.With<UserNameEnricher>()
                                         .Enrich.With<HttpRequestUrlEnricher>()
                                         .Enrich.With<WebApiRouteTemplateEnricher>()
                                         .Enrich.With<WebApiControllerNameEnricher>()
                                         .Enrich.With<WebApiRouteDataEnricher>()
                                         .Enrich.With<WebApiActionNameEnricher>();

            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}
