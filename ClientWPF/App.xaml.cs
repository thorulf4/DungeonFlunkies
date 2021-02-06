using ClientWPF.Scenes.Character;
using ClientWPF.Scenes.RoomScene;
using ClientWPF.Scenes.StartScreen;
using ClientWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Shared;
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
            var requestClient = new RequestClient(IPAddress.Loopback, 5723, Dispatcher);
            requestClient.OnFailReceived += OnFailReceived;

            services.AddSingleton(requestClient);
            services.AddScoped<SceneManagerVm>();
            services.AddScoped<ChatBoxVm>();
            services.AddScoped<SidebarVm>();

            //Add scenes
            services.AddTransient<StartScreenVm>();
            services.AddTransient<RoomVm>();
            services.AddTransient<InventoryVm>();

            //Add storage services
            services.AddScoped<Player>();
        }

        private void OnFailReceived(object sender, RequestFailure e)
        {
            MessageBox.Show("Fail: " + e.Message);
        }
    }
}
