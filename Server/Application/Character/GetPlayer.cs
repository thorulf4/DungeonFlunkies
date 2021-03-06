﻿using Microsoft.EntityFrameworkCore;
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
            return context.Players.FirstOrDefault(player => player.Id == playerId);
        }

        public Player Get(string playerName)
        {
            return context.Players.FirstOrDefault(player => player.Name == playerName);
        }

        public Player GetWithLocation(int playerId)
        {
            return context.Players.Where(player => player.Id == playerId).Include(p => p.Location).Single();
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
