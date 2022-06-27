using Server.Application;
using Server.Application.Character;
using Server.Application.Combat;
using Server.Model;
using Shared;
using Shared.Descriptors;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Interactables
{
    public class OptionalCombat : Interactable
    {
        public override InteractionDescriptor GetDescriptor(GameDb context)
        {
            return new InteractionDescriptor
            {
                ActionName = "Start fight",
                Id = this.Id
            };
        }

        public override Response Interact(Player player, GameDb context, Mediator mediator)
        {
            mediator.GetHandler<CombatManager>().StartEncounter(0, player);
            CombatEncounterResponse response = mediator.GetHandler<GetEncounter>().Get(player.Id);
            return Response.From(response);
        }
    }
}
