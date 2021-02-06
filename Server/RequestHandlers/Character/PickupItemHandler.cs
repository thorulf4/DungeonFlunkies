using Server.Application;
using Server.Application.Alerts;
using Server.Application.Character;
using Server.Application.Interactables;
using Server.Interactables;
using Shared;
using Shared.Requests.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.RequestHandlers.Character
{
    public class PickupItemHandler : RequestHandler<PickupItemRequest>
    {
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;

        public PickupItemHandler(Mediator mediator, Authenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        public override Response Handle(PickupItemRequest request)
        {
            int playerId = authenticator.VerifySession(request.Name, request.SessionId);

            var player = mediator.GetHandler<GetPlayer>().Get(playerId);

            Loot loot = mediator.GetHandler<GetInteractable>().GetOrCreate(player.LocationId, new Loot());
            var lootableItems = mediator.GetHandler<LootInteractable>().GetItemDescriptors(loot);
            if (lootableItems.Any(i => i.ItemId != request.Item.ItemId || i.Count < request.Item.Count))
                return Response.Fail("Not enough lootable items");

            mediator.GetHandler<LootInteractable>().RemoveItem(loot, request.Item);
            mediator.GetHandler<PlayerInventory>().AddItem(playerId, request.Item).Save();

            mediator.GetHandler<PickupItemAlerter>().Send(player.LocationId, request.Item, request.Name);
            return Response.Ok();
        }
    }
}
