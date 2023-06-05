using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Application;
using Server.Application.Combat.Skills;
using Server.Application.GameWorld;
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class Program
    {
        public static int startingRoomId;
        public static List<int> testItems = new();

        private static void AddTestItem(GameDb context, string name, EquipmentType type, string template)
        {
            Equipment item = new Equipment(name, type, template);
            context.Add(item);
            context.SaveChanges();
            testItems.Add(item.Id);
        }

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
                    AddTestItem(context, "Dagger", EquipmentType.Holdable, "Dagger");
                    AddTestItem(context, "Light Pants", EquipmentType.Legs, "FastShoes");
                    AddTestItem(context, "Hood", EquipmentType.Head, "FastCap");
                    AddTestItem(context, "Leather armor", EquipmentType.Torso, "LeatherArmor");

                    AddTestItem(context, "Sword", EquipmentType.Holdable, "Sword");
                    AddTestItem(context, "Metal armor", EquipmentType.Torso, "MetalArmor");
                    AddTestItem(context, "Dancers skirt", EquipmentType.Legs, "TauntShoes");
                    AddTestItem(context, "Shield", EquipmentType.Holdable, "Shield");
                    AddTestItem(context, "Metal Helmet", EquipmentType.Head, "MetalHelmet");

                    AddTestItem(context, "Mage Robe", EquipmentType.Torso, "MageRobe");
                    AddTestItem(context, "Mage Hat", EquipmentType.Head, "MageHat");
                    AddTestItem(context, "Fire Staff", EquipmentType.Holdable, "FireStaff");
                    AddTestItem(context, "Nature Staff", EquipmentType.Holdable, "NatureStaff");
                    AddTestItem(context, "Arcane Wand", EquipmentType.Holdable, "ArcaneWand");
                    AddTestItem(context, "Mage Sandals", EquipmentType.Legs, "MageSandals");

                    context.SaveChanges();

                    startingRoomId = 0;                    
                }
                else
                {
                    startingRoomId = 0;
                    testItems = context.Items.Select(i => i.Id).ToList();
                    Console.WriteLine($"Items {testItems.Count}");
                    foreach (int item in testItems)
                        Console.WriteLine(item);
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
