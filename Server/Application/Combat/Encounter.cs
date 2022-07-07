using Server.Application.Combat.AI;
using Server.Interactables;
using Server.Model;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Combat
{
    public class Encounter : IDescriptable<EncounterDescriptor>
    {
        public List<CombatEntity> entities = new List<CombatEntity>();

        public List<Enemy> enemyTeam = new();
        public List<CombatPlayer> playerTeam = new();

        public int roomId;

        public DispatcherService.QueuedInvocation nextTurnInvocation;
        public DateTime nextTurn;
        internal JoinCombat joinInteraction;

        public Encounter(int roomId, List<Enemy> enemies, List<CombatPlayer> players)
        {
            this.roomId = roomId;

            entities.AddRange(enemies);
            entities.AddRange(players);

            enemyTeam.AddRange(enemies);
            playerTeam.AddRange(players);

            for (int i = 0; i < entities.Count; i++)
                entities[i].Id = i;
        }

        internal void RefreshActions()
        {
            foreach(CombatEntity entity in entities)
            {
                if(entity is CombatPlayer player)
                {
                    player.hasAction = true;
                    player.hasBonusAction = true;
                }

                foreach(LoadedSkill skill in entity.skills)
                {
                    skill.CurrentCooldown = Math.Max(0, skill.CurrentCooldown - 1);
                }
            }
        }

        internal void AddPlayer(CombatPlayer player)
        {
            entities.Add(player);
            playerTeam.Add(player);

            player.Id = entities.IndexOf(player);
        }

        public void GenerateAiActions()
        {
            foreach (Enemy entity in GetAliveAi())
            {
                AiController.SetNextAction(entity, enemyTeam, playerTeam);
            }
        }

        //Assumes we only have enemy ai in fights
        public List<Enemy> GetAliveAi()
        {
            return enemyTeam.Where(e => e.alive).Select(e => (Enemy) e).ToList();
        }

        public bool AllEnemiesDead()
        {
            return enemyTeam.All(e => !e.alive);
        }

        public bool AllPlayersDead()
        {
            return playerTeam.All(e => !e.alive);
        }

        public EncounterDescriptor GetDescriptor()
        {
            return new EncounterDescriptor(enemyTeam.Where(e => e.alive).GetDescriptors().ToList(),
                playerTeam.Where(e => e.alive).GetDescriptors().ToList(),
                nextTurn);
        }
    }
}
