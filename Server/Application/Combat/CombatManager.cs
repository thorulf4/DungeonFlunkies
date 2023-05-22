using Server.Application.Alerts;
using Server.Application.Character;
using Server.Application.Combat.Enemies;
using Server.Application.GameWorld;
using Server.Interactables;
using Server.Model;
using Shared;
using Shared.Alerts.Combat;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Application.Combat
{
    public class CombatManager
    {
        /*
         * Add in a dispatcher service so we can use Dispatcher timers to handle the turn times
         * Use lock on encounters to ensure synchronized actions and avoid race conditions
         */

        private readonly GameDb context;
        private readonly Mediator mediator;
        private readonly DispatcherService dispatcher;
        private readonly IAlerter alerter;

        private readonly List<Encounter> encounters = new List<Encounter>();
        private readonly Dictionary<int, Encounter> encounterIndex = new Dictionary<int, Encounter>();

        public CombatManager(GameDb context, Mediator mediator, DispatcherService dispatcher, IAlerter alerter)
        {
            this.context = context;
            this.mediator = mediator;
            this.dispatcher = dispatcher;
            this.alerter = alerter;
        }

        public Encounter GetEncounter(int playerId)
        {
            return encounters.FirstOrDefault(e => e.entities.Any(p => p is CombatPlayer player && player.playerId == playerId));
        }

        public List<Encounter> GetEncountersInRoom(int roomId)
        {
            return encounters.Where(e => e.roomId == roomId).ToList();
        }

        public void PlayerJoinEncounter(Player player, Encounter encounter)
        {
            encounter.AddPlayer(new CombatPlayer(mediator, player));
            encounterIndex.Add(player.Id, encounter);

            mediator.GetHandler<CombatUpdateAlerter>().SendToAll(encounter);
        }

        public ISavable PlayerUseSkill(int playerId, SkillDescriptor skill, int targetId)
        {
            var encounter = encounterIndex[playerId];

            var target = encounter.entities[targetId];
            var player = (CombatPlayer)encounter.entities.First(e => e is CombatPlayer player && player.playerId == playerId);

            LoadedSkill actualSkill = player.skills.First(s => s.skill.Id == skill.Id);

            if (actualSkill.CurrentCooldown > 0)
                throw new Exception("Trying to use ability that is on cooldown");
            if (actualSkill.UsesAction && !player.hasAction || actualSkill.UsesBonusAction && !player.hasBonusAction)
                throw new Exception("Trying to use ability without appropiate action points");

            //TODO: Check if valid target

            if (actualSkill.UsesAction)
                player.hasAction = false;
            if (actualSkill.UsesBonusAction)
                player.hasBonusAction = false;

            actualSkill.skill.Apply(target);

            actualSkill.CurrentCooldown = actualSkill.Cooldown;
            skill.Cooldown = actualSkill.CurrentCooldown;

            if (encounter.AllEnemiesDead())
            {
                WinEncounter(encounter);
            }
            else
            {
                CheckEarlyTurnEnd(encounter);
                mediator.GetHandler<CombatUpdateAlerter>().SendToAll(encounter);
            }

            return context;
        }

        public void WinEncounter(Encounter encounter)
        {
            mediator.GetHandler<PlayerStats>().HealPlayers(encounter.playerTeam.Select(p => p.playerId));
            EndEncounter(encounter);
        }

        public void EndEncounter(Encounter encounter)
        {
            encounters.Remove(encounter);
            foreach(var player in encounter.playerTeam)
            {
                encounterIndex.Remove(player.playerId);
            }
            var alivePlayerNames = encounter.playerTeam.Where(p => p.alive).Select(p => p.name).ToList();
            mediator.GetHandler<World>().GetRoom(encounter.roomId).Remove(encounter.joinInteraction);
            alerter.SendAlerts(new WonCombatAlert(), alivePlayerNames);
        }

        public Encounter StartEncounter(int challengeRating, params Player[] players)
        {
            int roomId = players.First().LocationId;
            if (players.Any(p => p.LocationId != roomId))
                throw new Exception("Cannot start fight with players in different rooms");

            List<Enemy> enemies = new List<Enemy>();
            for (int i = 0; i < 3; i++)
                enemies.Add(GoblinFactory.Create());

            var encounter = new Encounter(roomId, enemies, players.Select(p => new CombatPlayer(mediator, p)).ToList());
            encounters.Add(encounter);
            foreach (var player in players)
                encounterIndex.Add(player.Id, encounter);

            encounter.joinInteraction = new JoinCombat(encounter);
            mediator.GetHandler<World>().GetRoom(roomId).Create(encounter.joinInteraction);

            encounter.nextTurn = DateTime.Now.AddMilliseconds(GameSettings.turnTimeInMs);
            encounter.nextTurnInvocation = dispatcher.InvokeIn(GameSettings.turnTimeInMs, () => NextTurn(encounter));

            //Consider making the first turn a free turn where no one can do anything
            encounter.GenerateAiActions();
            
            return encounter;
        }

        private void NextTurn(Encounter encounter)
        {
            foreach(Enemy enemy in encounter.GetAliveAi())
            {
                enemy.plannedAction.Apply();
            }

            encounter.RefreshActions();

            encounter.GenerateAiActions();

            mediator.GetHandler<NewTurnAlerter>().SendToAll(encounter);

            encounter.nextTurn = DateTime.Now.AddMilliseconds(GameSettings.turnTimeInMs);
            encounter.nextTurnInvocation = dispatcher.InvokeIn(GameSettings.turnTimeInMs, () => NextTurn(encounter));
        }

        public void EndTurn(int playerId)
        {
            var player = encounterIndex[playerId].playerTeam.First(p => p is CombatPlayer player && player.playerId == playerId);

            player.hasAction = false;
            player.hasBonusAction = false;

            CheckEarlyTurnEnd(encounterIndex[playerId]);
        }

        private void CheckEarlyTurnEnd(Encounter encounter)
        {
            bool anyActionsLeft = encounter.playerTeam.Any(p => p is CombatPlayer player && (player.hasAction || player.hasBonusAction));

            if (!anyActionsLeft)
            {
                encounter.nextTurnInvocation.Cancel();
                NextTurn(encounter);
            }
        }
    }
}
