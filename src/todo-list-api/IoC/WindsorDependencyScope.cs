using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;

namespace TodoListAPI.IoC
{
    public class WindsorDependencyScope : IDependencyScope
    {
        public WindsorDependencyScope(IWindsorContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _scope = container.BeginScope();
        }

        #region Implementation of IDependencyScope

        public object GetService(Type serviceType)
        {
            object service = _container.Kernel.HasComponent(serviceType)
                                 ? _container.Resolve(serviceType)
                                 : null;

            if (service != null)
            {
                _servicesInScope.Add(service);
            }

            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (!_container.Kernel.HasComponent(serviceType))
            {
                return new object[] { };
            }

            object[] services = _container.ResolveAll(serviceType).Cast<object>().ToArray();

            foreach (object s in services)
            {
                _servicesInScope.Add(s);
            }
            
            return services;
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            foreach (object s in _servicesInScope)
            {
                _container.Release(s);
            }

            _servicesInScope = null;

            _scope.Dispose();
        }

        #endregion

        private readonly IWindsorContainer _container;
        private readonly IDisposable _scope;
        
        private ConcurrentBag<object> _servicesInScope = new ConcurrentBag<object>();
    }
}
