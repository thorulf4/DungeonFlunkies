using Shared.Model.Interactables;
using Shared.Requests.Interactables;
using Shared.Requests.Rooms;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Scenes.Rooms
{
    public class RoomScene : Scene
    {
        private RoomResponse room;

        public RoomScene(RoomResponse room)
        {
            this.room = room;
        }

        public override Scene Handle(RequestClient client)
        {
            Console.Clear();
            Console.WriteLine($"You're in room {room.RoomId}");
            Dictionary<ConsoleKey, InteractionDescriptor> options = new Dictionary<ConsoleKey, InteractionDescriptor>();
            ConsoleKey[] keys = new ConsoleKey[] { ConsoleKey.Q, ConsoleKey.W, ConsoleKey.E, ConsoleKey.A, ConsoleKey.S, ConsoleKey.D, ConsoleKey.Z, ConsoleKey.X, ConsoleKey.C };

            for (int i = 0; i < room.Interactions.Length; i++)
            {
                InteractionDescriptor descriptor = room.Interactions[i];
                ConsoleKey key = keys[i];

                Console.WriteLine($"{key}) {descriptor.ActionName}");
                options.Add(key, descriptor);
            }

            Console.WriteLine("People in room: ");
            foreach (string name in room.PeopleInRoom)
                Console.Write($"{name}, ");

            while (true)
            {
                var input = Console.ReadKey();

                if (options.ContainsKey(input.Key))
                {
                    var descriptor = options[input.Key];

                    var result = client.SendRequest(new MoveInteraction(descriptor.Id));

                    return new RoomScene((RoomResponse)result.data);
                }
            }
        }

        public static RoomScene LoadCurrentRoom(RequestClient client)
        {
            Console.WriteLine("Loading..");
            var result = client.SendRequest(new GetRoomRequest());

            if (result.Success)
            {
                return new RoomScene((RoomResponse)result.data);
            }
            else
            {
                throw new Exception("Unexpected result");
            }
        }
    }
}
