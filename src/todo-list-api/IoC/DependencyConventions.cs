using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Data;
using TodoListAPI.Security;
using TodoListAPI.Services;

namespace TodoListAPI.IoC
{
    public class DependencyConventions : IWindsorInstaller
    {
        public DependencyConventions(bool isTestMode)
        {
            _isTestMode = isTestMode;
        }

        #region Implementation of IWindsorInstaller

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // entity framework configuration

            container.Register(Component.For<DbContext>()
                                        .UsingFactoryMethod(c => InMemoryDbContextFactory.Create(new SessionInfo()).Result));

            // automapper configuration

            container.Install(new AutoMapperInstaller());

            // api controllers configuration

            BasedOnDescriptor controllerRegistrationsCommon =
                Classes.FromThisAssembly()
                       .Pick().If(t => t.Name.EndsWith("Controller"))
                       .Configure(configurer => configurer.Named(configurer.Implementation.Name));

                // in test mode registering controllers using transient lifestyle because in test project http modules are not available
                // when installed in api project using per-web-request lifestyle

            BasedOnDescriptor controllerRegistrations = _isTestMode 
                                                            ? controllerRegistrationsCommon.LifestyleTransient() 
                                                            : controllerRegistrationsCommon.LifestylePerWebRequest();

            container.Register(controllerRegistrations);

            // registering components in core assembly using transient lifestyle

            container.Register(Classes.FromAssemblyContaining<TodoListService>()
                                      .Pick().WithServiceDefaultInterfaces()
                                      .LifestyleTransient());
        }

        #endregion

        private readonly bool _isTestMode;
    }
}
