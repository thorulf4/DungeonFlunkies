using Server.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Server.Application.Character
{
    public class CreateNewCharacter : IApplicationLogic
    {
        private readonly GameDb context;

        public CreateNewCharacter(GameDb context)
        {
            this.context = context;
        }

        public Player CreatePlayer(string name, string secret)
        {
            var player = new Player(name, secret)
            {
                LocationId = Program.startingRoomId
            };

            context.Players.Add(player);
            context.SaveChanges();

            GiveStartingItems(player);

            return player;
        }

        private void GiveStartingItems(Player player)
        {
            context.Add(new OwnedBy { OwnsId = Program.testSwordId, OwnerId = player.Id, Count = 10 });
            context.SaveChanges();
        }
    }
}
