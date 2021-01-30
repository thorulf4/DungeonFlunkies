using ClientWPF.Scenes.RoomScene;
using ClientWPF.Scenes.StartScreen;
using ClientWPF.ViewModels;
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
            services.AddSingleton(new RequestClient(IPAddress.Loopback, 5723, Dispatcher));
            services.AddScoped<SceneManagerVm>();
            services.AddScoped<ChatBoxVm>();

            //Add scenes
            services.AddTransient<StartScreenVm>();
            services.AddTransient<RoomVm>();

            //Add storages
            services.AddScoped<Player>();
        }
    }
}
