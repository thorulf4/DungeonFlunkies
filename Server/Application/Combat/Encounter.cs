using Server.Interactables;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat
{
    public class Encounter
    {
        public List<CombatEntity> entities = new List<CombatEntity>();

        public List<CombatEntity> enemyTeam = new List<CombatEntity>();
        public List<CombatEntity> playerTeam = new List<CombatEntity>();

        public int roomId;

        public DispatcherService.QueuedInvocation nextTurnInvocation;
        internal JoinCombat joinInteraction;

        public Encounter(List<Enemy> enemies, List<CombatPlayer> players)
        {
            entities.AddRange(enemies);
            entities.AddRange(players);

            enemyTeam.AddRange(enemies);
            playerTeam.AddRange(players);

            for (int i = 0; i < entities.Count; i++)
                entities[i].Id = i;
        }

        internal void RefreshActions()
        {
            foreach(CombatEntity entity in playerTeam)
            {
                if(entity is CombatPlayer player)
                {
                    player.hasAction = true;
                    player.hasBonusAction = true;
                }
            }
        }

        internal void AddPlayer(CombatPlayer player)
        {
            entities.Add(player);
            playerTeam.Add(player);

            player.Id = entities.IndexOf(player);
        }
    }
}
