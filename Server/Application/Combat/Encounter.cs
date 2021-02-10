using Server.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat
{
    public class Encounter
    {
        public List<Enemy> enemies;
        public List<Player> players;

        public Encounter(List<Enemy> enemies, List<Player> players)
        {
            this.enemies = enemies;
            this.players = players;
        }
    }
}
