using Server.Application;
using Server.Application.Combat;
using Server.Model;
using Shared;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Interactables
{
    public class JoinCombat : Interactable
    {
        public Encounter encounter;

        public JoinCombat(Encounter encounter)
        {
            this.encounter = encounter;
        }

        public override InteractionDescriptor GetDescriptor(GameDb context)
        {
            return new InteractionDescriptor()
            {
                Id = Id,
                ActionName = "Join combat"
            };
        }

        public override Response Interact(Player player, GameDb context, Mediator mediator)
        {
            mediator.GetHandler<CombatManager>().PlayerJoinEncounter(player, encounter);

            var response = mediator.GetHandler<GetEncounter>().Get(player.Id);
            return Response.From(response);
        }
    }
}
