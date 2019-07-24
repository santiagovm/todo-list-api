using System;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Serilog;
using TodoListAPI.IoC;

namespace TodoListAPI
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), 
                                                               new WindsorControllerActivator(_container));

            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(_container);

            Log.Information("app started");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Log.Information("app shutting down. Reason = [{@shutdownReason}]", HostingEnvironment.ShutdownReason);
        }

        protected WebApiApplication()
        {
            _container = new WindsorContainer().Install(new DependencyConventions(isTestMode: false));
        }

        #region Overrides of HttpApplication

        public override void Dispose()
        {
            _container.Dispose();
            base.Dispose();
        }

        #endregion

        private readonly IWindsorContainer _container;
    }
}
