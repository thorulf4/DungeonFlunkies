using Microsoft.EntityFrameworkCore;
using Server.Application.Alerts;
using Server.Application.Interactables;
using Server.Interactables;
using Server.Model;
using Server.Model.Skills;
using Shared;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private readonly List<Encounter> encounters = new List<Encounter>();
        private readonly Dictionary<int, Encounter> encounterIndex = new Dictionary<int, Encounter>();

        public CombatManager(GameDb context, Mediator mediator, DispatcherService dispatcher)
        {
            this.context = context;
            this.mediator = mediator;
            this.dispatcher = dispatcher;
        }

        public Encounter GetEncounter(int playerId)
        {
            return encounters.First(e => e.entities.Any(p => p is CombatPlayer player && player.playerId == playerId));
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

            var actualSkill = player.skills.First(s => s.Id == skill.Id);

            if (actualSkill.CurrentCooldown > 0)
                throw new Exception("Trying to use ability that is on cooldown");
            if (actualSkill.UsesAction && !player.hasAction || actualSkill.UsesBonusAction && !player.hasBonusAction)
                throw new Exception("Trying to use ability without appropiate action points");

            //TODO: Check if valid target

            if (actualSkill.UsesAction)
                player.hasAction = false;
            if (actualSkill.UsesBonusAction)
                player.hasBonusAction = false;

            Skill logicalSkill = mediator.GetHandler<GetSkills>().Get(skill);

            logicalSkill.Apply(target);

            actualSkill.Cooldown = actualSkill.CurrentCooldown;
            skill.Cooldown = actualSkill.CurrentCooldown;

            CheckEarlyTurnEnd(encounter);

            mediator.GetHandler<CombatUpdateAlerter>().SendToAll(encounter);

            return context;
        }

        public Encounter StartEncounter(int challengeRating, params Player[] players)
        {
            int roomId = players.First().LocationId;
            if (players.Any(p => p.LocationId != roomId))
                throw new Exception("Cannot start fight with players in different rooms");

            List<Enemy> enemies = new List<Enemy>();
            for (int i = 0; i < 3; i++)
                enemies.Add(new Enemy("Goblin", 100, new List<SkillDescriptor>()));

            var encounter = new Encounter(enemies, players.Select(p => new CombatPlayer(mediator, p)).ToList());
            encounters.Add(encounter);
            foreach (var player in players)
                encounterIndex.Add(player.Id, encounter);

            encounter.joinInteraction = new JoinCombat(encounter);
            mediator.GetHandler<DynamicInteractables>().AddInteractable(roomId, encounter.joinInteraction);

            encounter.nextTurnInvocation = dispatcher.InvokeIn(GameSettings.turnTimeInMs, () => NextTurn(encounter));

            return encounter;
        }

        private void NextTurn(Encounter encounter)
        {
            encounter.RefreshActions();

            mediator.GetHandler<NewTurnAlerter>().SendToAll(encounter);



            encounter.nextTurnInvocation = dispatcher.InvokeIn(GameSettings.turnTimeInMs, () => NextTurn(encounter));
        }

        public void EndTurn(int playerId)
        {
            var player = (CombatPlayer) encounterIndex[playerId].playerTeam.First(p => p is CombatPlayer player && player.playerId == playerId);

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
