using Microsoft.Extensions.DependencyInjection;
using Server.Application.Alerts;
using Server.Application.Character;
using Server.Application.Combat;
using Server.Application.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Server.Application
{
    //TODO This isnt actually the mediator pattern :/ Closer to a service provider
    public class Mediator
    {
        private IServiceProvider provider;
        private ServiceCollection services;

        public Mediator(GameDb context, IAlerter alerter, DispatcherService dispatcherService, Authenticator authenticator)
        {
            services = new ServiceCollection();
            services.AddSingleton(context);
            services.AddSingleton(alerter);
            services.AddSingleton(dispatcherService);
            services.AddSingleton(authenticator);
            services.AddSingleton(this);
            services.AddSingleton<World>();
            AddHandlers();
            CreateProvider();

            GetHandler<World>().CreateDefaultWorld(this);
        }

        public void CreateProvider()
        {
            if (services == null)
                throw new Exception("Cant create provider as configuration has been completed");

            provider = services.BuildServiceProvider();
            services = null;
        }

        public T GetHandler<T>()
        {
            return provider.GetRequiredService<T>();
        }

        public void AddHandlers()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IApplicationLogic))).ToArray();
            foreach (var type in types)
                services.AddTransient(type);

            services.AddSingleton<CombatManager>();
        }
    }

    //Used with reflection to add handlers into mediator
    public interface IApplicationLogic
    {

    }
}
