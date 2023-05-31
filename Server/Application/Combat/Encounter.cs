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
        public JoinCombat joinInteraction;

        public bool shouldUpdatePlayers = false;

        int nextEntityId = 0;

        public Encounter(int roomId, List<Enemy> enemies, List<CombatPlayer> players)
        {
            this.roomId = roomId;

            entities.AddRange(enemies);
            entities.AddRange(players);

            enemyTeam.AddRange(enemies);
            playerTeam.AddRange(players);

            foreach(CombatEntity entity in entities)
            {
                ProvideEntityId(entity);
            }
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

        public void AddPlayer(CombatPlayer player)
        {
            entities.Add(player);
            playerTeam.Add(player);

            //player.Id = entities.IndexOf(player);
            ProvideEntityId(player);
        }

        public void AddEnemy(Enemy enemy)
        {
            entities.Add(enemy);
            enemyTeam.Add(enemy);
            ProvideEntityId(enemy);
        }

        public void GenerateAiActions()
        {
            foreach (Enemy entity in GetAliveAi())
            {
                AiController.SetNextAction(entity, enemyTeam, playerTeam);
            }
        }

        public List<Enemy> GetAliveAi()
        {
            return enemyTeam.Where(e => e.alive).ToList();
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

        public void MarkChanged()
        {
            shouldUpdatePlayers = true;
        }

        private void ProvideEntityId(CombatEntity entity)
        {
            entity.Id = nextEntityId;
            nextEntityId++;
        }
    }
}
