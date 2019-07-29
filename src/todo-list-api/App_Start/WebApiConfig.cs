using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Web.Http.Versioning;
using Microsoft.Web.Http.Versioning.Conventions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiThrottle;

namespace TodoListAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // tweaking json formatter
            JsonMediaTypeFormatter jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;

            jsonFormatter.SerializerSettings = new JsonSerializerSettings
                                               {
                                                   // using snake case (i.e. this_is_snake_case)
                                                   ContractResolver = new DefaultContractResolver
                                                                      {
                                                                          NamingStrategy = new SnakeCaseNamingStrategy()
                                                                      },

                                                   // excluding nulls
                                                   NullValueHandling = NullValueHandling.Ignore,
                                               };

            // json only
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

            // cors
            var corsPolicyProvider = new EnableCorsAttribute(origins: "*", headers: "*", methods: "*", exposedHeaders: "*");
            config.EnableCors(corsPolicyProvider);
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // throttling
            config.MessageHandlers
                  .Add(new ThrottlingHandler
                       {
                           Policy = new ThrottlePolicy(perSecond: 5)
                                    {
                                        IpThrottling = true,

                                        // increasing limits for swagger/* requests
                                        EndpointRules = new Dictionary<string, RateLimits>
                                                        {
                                                            { "swagger", new RateLimits { PerSecond = 1000 }}
                                                        }
                                    },

                           Repository = new CacheRepository()
                       });

            // versioning by namespace convention and media type
            // https://github.com/microsoft/aspnet-api-versioning/wiki/API-Version-Conventions#version-by-namespace-convention
            // https://github.com/Microsoft/aspnet-api-versioning/wiki/Versioning-by-Media-Type#configuration

            config.AddApiVersioning(options =>
                                    {
                                        options.Conventions.Add(new VersionByNamespaceConvention());

                                        options.ApiVersionReader = new MediaTypeApiVersionReader();
                                        options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                                        options.AssumeDefaultVersionWhenUnspecified = true;
                                    });
        }
    }
}
