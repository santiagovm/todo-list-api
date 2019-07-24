using System.Linq;
using AutoMapper;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TodoListAPI.Dtos;

namespace TodoListAPI.IoC
{
    /// <summary>
    /// https://automapper.readthedocs.io/en/latest/Dependency-injection.html
    /// </summary>
    public class AutoMapperInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // registering all mapper profiles
            container.Register(Classes.FromAssemblyContaining<DtosMappingProfile>()
                                      .BasedOn<Profile>().WithServiceBase());

            // registering IConfigurationProvider with all registered profiles
            container.Register
                (Component.For<IConfigurationProvider>()
                          .UsingFactoryMethod(kernel => new MapperConfiguration(configuration =>
                                                                                    kernel.ResolveAll<Profile>()
                                                                                          .ToList()
                                                                                          .ForEach(configuration.AddProfile)))
                          .LifestyleSingleton());
            
            // registering IMapper with registered IConfigurationProvider
            container.Register(Component.For<IMapper>()
                                        .UsingFactoryMethod(kernel => new Mapper(kernel.Resolve<IConfigurationProvider>(), 
                                                                                 kernel.Resolve)));
        }
    }
}
