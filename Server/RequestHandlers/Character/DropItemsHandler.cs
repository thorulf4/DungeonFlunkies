
using Shared;
using Shared.Requests.Character;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Server.Interactables;
using Server.Model;
using Shared.Alerts;
using Server.Application;
using Server.Application.Character;
using Server.Application.Interactables;
using Server.Application.Alerts;

namespace Server.RequestHandlers.Character
{
    public class DropItemsHandler : RequestHandler<DropItemsRequest>
    {
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;

        public DropItemsHandler(Mediator mediator, Authenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        public override Response Handle(DropItemsRequest request)
        {
            if (request.DroppedItems.Count > 1)
                throw new Exception("this handler cannot handle dropping more than one item descriptor at a time");

            int playerId = authenticator.VerifySession(request.Name, request.SessionId);

            var player = mediator.GetHandler<GetPlayer>().Get(playerId);

            bool hasItems = mediator.GetHandler<PlayerInventory>().PlayerHasItems(request.DroppedItems, playerId);
            if (!hasItems)
                return Response.Fail("Couldnt drop items");


            Loot loot = mediator.GetHandler<GetInteractable>().GetOrCreate(player.LocationId, new Loot { RoomId = player.LocationId });

            mediator.GetHandler<PlayerInventory>().RemoveItems(playerId, request.DroppedItems);
            mediator.GetHandler<LootInteractable>().AddItems(loot, request.DroppedItems).Save();

            mediator.GetHandler<RoomUpdateAlerter>().Send(player.LocationId);
            mediator.GetHandler<DroppedItemsAlerter>().Send(player.LocationId, request.DroppedItems.First(), request.Name);
            return Response.Ok();
        }
    }
}
