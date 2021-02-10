using Server.Application;
using Server.Application.Character;
using Server.Model.Items;
using Shared;
using Shared.Descriptors;
using Shared.Requests.Character;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.RequestHandlers.Character
{
    public class EquipItemHandler : RequestHandler<EquipItemRequest>
    {
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;

        public EquipItemHandler(Mediator mediator, Authenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        public override Response Handle(EquipItemRequest request)
        {
            int playerId = authenticator.VerifySession(request.Name, request.SessionId);

            Item item = mediator.GetHandler<GetItem>().Get(request.Item.ItemId);
            bool ownsItem = mediator.GetHandler<PlayerInventory>().PlayerOwns(playerId, new ItemDescriptor(item.Id, item.Name, 1));

            if (ownsItem && item is Equipment equip)
            {
                if (equip.Type != request.Type)
                    return Response.Fail("Cannot equip this equipment as it is of the wrong type");

                mediator.GetHandler<GetEquipment>().Equip(playerId, equip, request.Slot);
                mediator.GetHandler<PlayerInventory>().RemoveItem(playerId, new ItemDescriptor(equip.Id, equip.Name, 1)).Save();
                return Response.Ok();
            }

            return Response.Fail("Item is not in inventory or not equipment");
        }
    }
}
