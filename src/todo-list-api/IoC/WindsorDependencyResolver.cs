using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace TodoListAPI.IoC
{
    public class WindsorDependencyResolver : IDependencyResolver
    {
        public WindsorDependencyResolver(IWindsorContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        #region Implementation of IDependencyScope

        public object GetService(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType)
                       ? _container.Resolve(serviceType)
                       : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType)
                       ? _container.ResolveAll(serviceType).Cast<object>().ToArray()
                       : new object[] { };
        }

        public IDependencyScope BeginScope()
        {
            var scope = new WindsorDependencyScope(_container);
            _scopes.Add(scope);
            return scope;
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            foreach (IDependencyScope scope in _scopes)
            {
                scope.Dispose();
            }
        }

        #endregion

        private readonly ConcurrentBag<IDependencyScope> _scopes = new ConcurrentBag<IDependencyScope>();
        private readonly IWindsorContainer _container;
    }
}
