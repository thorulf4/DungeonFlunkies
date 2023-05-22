using Server.Interactables;
using Server.Model;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.GameWorld
{
    public class World
    {
        private readonly Dictionary<int, Room> rooms = new();

        public World()
        {

        }

        public void CreateDefaultWorld(Mediator mediator)
        {
            Room room1 = new Room(0);
            Room room2 = new Room(1);
            room1.Create(new Path(room2));
            room2.Create(new Path(room1));
            room1.Create(new OptionalCombat());
            var loot = room2.Create(new Loot());
            var item = mediator.GetHandler<GetItem>().GetDescriptor(Program.testSwordId, 5);
            loot.AddItem(item);

            rooms.Add(0, room1);
            rooms.Add(1, room2);
        }

        public Room GetRoom(Player player)
        {
            return GetRoom(player.LocationId);
        }

        internal Room GetRoom(int roomId)
        {
            return rooms[roomId];
        }
    }
}
