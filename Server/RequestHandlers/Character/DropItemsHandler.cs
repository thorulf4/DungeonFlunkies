
using Shared;
using Shared.Requests.Character;
using System;
using System.Linq;
using Server.Interactables;
using Server.Application;
using Server.Application.Character;
using Server.Application.Alerts;
using Server.Application.GameWorld;

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
             
            Loot loot = mediator.GetHandler<World>().GetRoom(player).GetOrCreate(new Loot());

            loot.AddItems(request.DroppedItems);
            mediator.GetHandler<PlayerInventory>().RemoveItems(playerId, request.DroppedItems).Save();

            mediator.GetHandler<RoomUpdateAlerter>().Send(player.LocationId);
            mediator.GetHandler<DroppedItemsAlerter>().Send(player.LocationId, request.DroppedItems.First(), request.Name);
            return Response.Ok();
        }
    }
}
