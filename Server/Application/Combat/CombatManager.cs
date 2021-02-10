using Server.Model;
using Server.Model.Skills;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Combat
{
    public class CombatManager
    {
        private readonly GameDb context;
        private readonly Mediator mediator;

        private List<Encounter> encounters = new List<Encounter>();
        private Dictionary<int, Encounter> index = new Dictionary<int, Encounter>();

        public CombatManager(GameDb context, Mediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public Encounter GetEncounter(int playerId)
        {
            return encounters[playerId];
        }

        public Encounter StartEncounter(int challengeRating, params Player[] players)
        {
            List<Enemy> enemies = new List<Enemy>();
            for (int i = 0; i < 3; i++)
                enemies.Add(new Enemy("Goblin", 100, new List<SkillDescriptor>()));

            var encounter = new Encounter(enemies, players.ToList());
            encounters.Add(encounter);
            foreach (var player in players)
                index.Add(player.Id, encounter);

            return encounter;
        }
    }
}
