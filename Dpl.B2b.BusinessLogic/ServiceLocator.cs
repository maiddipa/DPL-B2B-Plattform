
using DevExpress.CodeParser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.BusinessLogic
{

    public class ServiceLocator
    {
        private IServiceProvider _currentServiceProvider;
        private static IServiceProvider _serviceProvider;

        public ServiceLocator(IServiceProvider currentServiceProvider)
        {
            _currentServiceProvider = currentServiceProvider;
        }

        public static ServiceLocator Current
        {
            get
            {
                return new ServiceLocator(_serviceProvider);
            }
        }

        public static void SetLocatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetInstance(Type serviceType)
        {
            return _currentServiceProvider.GetService(serviceType);
        }

        public TService GetInstance<TService>()
        {
            return _currentServiceProvider.GetService<TService>();
        }
    }

    public static class HostExtensions
    {
        public static IHost AddServiceLocator(this IHost host)
        {
            ServiceLocator.SetLocatorProvider(host.Services);
            return host;
        }
    }
}

