using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Character
{
    public class PlayerStats : IApplicationLogic
    {
        private readonly Mediator mediator;
        private readonly GameDb context;

        public PlayerStats(Mediator mediator, GameDb context)
        {
            this.mediator = mediator;
            this.context = context;
        }

        public ISavable DecreaseHealth(int playerId, int amount)
        {
            var player = context.Players.Find(playerId);
            player.Health -= amount;
            if (player.Health < 0)
                player.Health = 0;

            return context;
        }

        public void HealPlayers(IEnumerable<int> playerIds)
        {
            foreach(int playerId in playerIds)
            {
                var player = context.Players.Find(playerId);
                player.Health = 100;
            }
            context.SaveChanges();
        }
    }
}
