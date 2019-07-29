using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Castle.Windsor;

namespace TodoListAPI.IoC
{
    public class WindsorControllerActivator : IHttpControllerActivator
    {
        public WindsorControllerActivator(IWindsorContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }
 
        #region Implementation of IHttpControllerActivator

        public IHttpController Create(HttpRequestMessage request,
                                      HttpControllerDescriptor controllerDescriptor,
                                      Type controllerType)
        {
            var controller = (IHttpController) _container.Resolve(controllerType);
 
            request.RegisterForDispose(new Release(() => _container.Release(controller)));

            return controller;
        }

        #endregion

        private class Release : IDisposable
        {
            public Release(Action release)
            {
                _release = release ?? throw new ArgumentNullException(nameof(release));
            }
 
            public void Dispose()
            {
                _release();
            }

            private readonly Action _release;
        }

        private readonly IWindsorContainer _container;
    }
}
