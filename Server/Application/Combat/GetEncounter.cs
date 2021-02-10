using Shared.Descriptors;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Combat
{
    public class GetEncounter
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
            var skills = mediator.GetHandler<GetSkills>().GetFromPlayer(playerId).Select(s => new SkillDescriptor(s.Name, s.Description, s.Cooldown)).ToList();

            return new CombatEncounterResponse( skills,
                                                encounter.enemies.Select(e => new EntityDescriptor(e.name, e.health, e.maxHealth)).ToList(),
                                                encounter.players.Select(p => new EntityDescriptor(p.Name, 100, 100)).ToList());
        }
    }
}
