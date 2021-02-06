using Server.Application;
using Server.Application.Character;
using Server.Model.Items;
using Shared;
using Shared.Requests.Character;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.RequestHandlers.Character
{
    public class GetInventoryHandler : RequestHandler<GetInventoryRequest>
    {
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;

        public GetInventoryHandler(Mediator mediator, Authenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        public override Response Handle(GetInventoryRequest request)
        {
            int playerid = authenticator.VerifySession(request.Name, request.SessionId);

            var player = mediator.GetHandler<GetPlayer>().Get(playerid);

            var items = mediator.GetHandler<PlayerInventory>().GetDescriptors(playerid);
            var droppedItems = mediator.GetHandler<GetItem>().GetDroppedItems(player.LocationId);
            var equipped = mediator.GetHandler<GetEquipment>().GetAsDictionary(playerid);

            return Response.From(new InventoryResponse
            {
                Inventory = items,
                DroppedItems = droppedItems,
                EquippedItems = equipped
            });
        }
    }
}
