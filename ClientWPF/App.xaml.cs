using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace ClientWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider provider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            RegisterDependencies(services);
            provider = services.BuildServiceProvider();
        }

        private void RegisterDependencies(ServiceCollection services)
        {
            services.AddSingleton(new RequestClient(IPAddress.Loopback, 5723));
        }
    }
}
