using Microsoft.EntityFrameworkCore;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Character
{
    public class GetPlayer : IApplicationLogic
    {
        private readonly GameDb context;

        public GetPlayer(GameDb context)
        {
            this.context = context;
        }

        public Player Get(int playerId)
        {
            return context.Players.First(player => player.Id == playerId); // May get a null pointer after death
        }

        public Player Get(string playerName)
        {
            return context.Players.First(player => player.Name == playerName); // Null pointer when logging in without a character
        }
        
        public List<Player> GetInRoom(int roomId)
        {
            return context.Players.Where(p => p.LocationId == roomId).ToList();
        }

        public bool PlayerExists(string playerName)
        {
            return context.Players.Any(p => p.Name == playerName);
        }
    }
}
