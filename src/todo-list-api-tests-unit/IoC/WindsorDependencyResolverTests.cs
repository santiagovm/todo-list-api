using System;
using Castle.Windsor;
using FluentAssertions;
using NUnit.Framework;
using TodoListAPI.IoC;
using TodoListAPI.v1.Controllers;

namespace TodoListAPI.Tests.Unit.IoC
{
    [TestFixture]
    public class WindsorDependencyResolverTests
    {
        [Test]
        public void when_get_service_given_lists_controller_requested_should_return_new_instance()
        {
            // given
            IWindsorContainer container = new WindsorContainer().Install(new DependencyConventions(isTestMode: true));
            var sut = new WindsorDependencyResolver(container);

            Type requestedType = typeof(ListsController);

            // when
            object service = sut.GetService(requestedType);

            // should
            service.Should().NotBeNull();
        }
    }
}
