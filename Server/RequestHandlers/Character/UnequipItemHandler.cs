using Server.Application;
using Server.Application.Character;
using Shared;
using Shared.Requests.Character;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.RequestHandlers.Character
{
    public class UnequipItemHandler : RequestHandler<UnequipItemRequest>
    {
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;

        public UnequipItemHandler(Mediator mediator, Authenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        public override Response Handle(UnequipItemRequest request)
        {
            int playerId = authenticator.VerifySession(request.Name, request.SessionId);

            var equipment = mediator.GetHandler<GetEquipment>().GetRelationWithItem(playerId, request.Type).GetValueOrDefault(request.Slot);

            if (equipment == null)
                return Response.Fail("Cannot unequip nothing :/");


            ItemDescriptor item = new ItemDescriptor(equipment.ItemId, equipment.Item.Name, 1);

            mediator.GetHandler<PlayerInventory>().AddItem(playerId, item);
            mediator.GetHandler<GetEquipment>().Unequip(playerId, request.Type, request.Slot).Save();
            return Response.Ok();
        }
    }
}
