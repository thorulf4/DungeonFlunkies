using Shared.Descriptors;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Combat
{
    public class GetEncounter : IApplicationLogic
    {
        private readonly GameDb context;
        private readonly Mediator mediator;

        public GetEncounter(GameDb context, Mediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public CombatEncounterResponse Get(int playerId)
        {
            var encounter = mediator.GetHandler<CombatManager>().GetEncounter(playerId);
            var skills = encounter.playerTeam.First(p => p is CombatPlayer player && player.playerId == playerId).skills;

            return new CombatEncounterResponse( skills,
                                                encounter.enemyTeam.Where(e => e.alive).Select(e => e.GetDescriptor()).ToList(),
                                                encounter.playerTeam.Where(e => e.alive).Select(p => p.GetDescriptor()).ToList());
        }
    }
}
