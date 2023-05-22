using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Application;
using Server.Application.Combat.Skills;
using Server.Interactables;
using Server.Model;
using Server.Model.Items;
using Server.Pipelining;
using Server.RequestHandlers;
using Server.RequestHandlers.Character;
using Server.RequestHandlers.Combat;
using Server.RequestHandlers.Rooms;
using Shared;
using Shared.Requests;
using Shared.Requests.Authentication;
using Shared.Requests.Character;
using Shared.Requests.Combat;
using Shared.Requests.Rooms;
using System;
using System.Linq;
using System.Text;

namespace Server
{
    class Program
    {
        public static int startingRoomId;
        public static int testSwordId;

        static void Main(string[] args)
        {
            using(var context = new GameDb())
            {
                context.Database.Migrate();

                Console.WriteLine(context.Players.Count());
                foreach (var p in context.Players.Select(x => $"{x.Name} in {x.LocationId}"))
                {
                    Console.WriteLine(p);
                }

                //Seed the database
                if (context.Items.Count() == 0)
                {
                    Equipment sword = new Equipment { BaseValue = 1, Name = "Sword", Type = EquipmentType.Holdable, EquipmentTemplate="Sword", ItemPower=65 };
                    context.Add(sword);
                    context.SaveChanges();

                    startingRoomId = 0;
                    testSwordId = sword.Id;
                }
                else
                {
                    startingRoomId = 0;
                    testSwordId = 1;
                }
            }

            RequestListener listener = new RequestListener(5723);
            var configure = listener.Configure();
            configure.BuildDependencies(services =>
            {
                services.AddDbContext<GameDb>();
                services.AddSingleton<Authenticator>();
                services.AddSingleton<IAlerter>(listener);
                services.AddSingleton<Mediator>();
                services.AddSingleton(new DispatcherService());
            });
            configure.AddMiddleware(new Debugger());
            configure.AddInteractionHandler<InteractionHandler>();
            AddHandlers(configure);

            configure.StartListening();
        }

        private static void AddHandlers(RequestListener.ListenerBuilder configure)
        {
            configure.AddHandler<CreateCharacterRequest, CreateCharacterHandler>();
            configure.AddHandler<LoginRequest, LoginHandler>();
            configure.AddHandler<GetRoomRequest, GetRoomHandler>();
            configure.AddHandler<SayRequest, SayHandler>();
            configure.AddHandler<GetInventoryRequest, GetInventoryHandler>();
            configure.AddHandler<DropItemsRequest, DropItemsHandler>();
            configure.AddHandler<PickupItemRequest, PickupItemHandler>();
            configure.AddHandler<EquipItemRequest, EquipItemHandler>();
            configure.AddHandler<UnequipItemRequest, UnequipItemHandler>();
            configure.AddHandler<GetEncounterRequest, GetEncounterHandler>();
            configure.AddHandler<UseSkillRequest, UseSkillHandler>();
            configure.AddHandler<EndTurnRequest, EndTurnHandler>();
        }
    }
}
