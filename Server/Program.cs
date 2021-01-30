using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Interactables;
using Server.Model;
using Server.Pipeline;
using Server.RequestHandlers;
using Server.RequestHandlers.Rooms;
using Shared.Requests;
using Shared.Requests.Authentication;
using Shared.Requests.Rooms;
using System;
using System.Linq;

namespace Server
{
    class Program
    {
        public static int startingRoomId;

        static void Main(string[] args)
        {
            using(var context = new GameDb())
            {
                context.Database.Migrate();

                Console.WriteLine(context.Players.Count());
                foreach (var p in context.Players.Select(x => $"{x.Name} in {x.Location.Id}"))
                {
                    Console.WriteLine(p);
                }

                //Seed the database
                if (context.Rooms.Count() == 0)
                {
                    Room room1 = new Room();
                    Room room2 = new Room();

                    context.Add(room1);
                    context.Add(room2);

                    context.SaveChanges();

                    room1.Interactables.Add(new Path { LeadsTo = context.Rooms.Find(room2.Id) });
                    room2.Interactables.Add(new Path { LeadsTo = context.Rooms.Find(room1.Id) });


                    context.SaveChanges();

                    startingRoomId = room1.Id;
                }
                else
                {
                    startingRoomId = context.Rooms.FirstOrDefault().Id;
                }
            }

            RequestListener listener = new RequestListener(5723);
            var configure = listener.Configure();
            configure.BuildDependencies(services =>
            {
                services.AddDbContext<GameDb>();
                services.AddSingleton<Authenticator>();
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
        }
    }
}
