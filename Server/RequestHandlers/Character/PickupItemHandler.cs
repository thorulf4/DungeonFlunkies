using Server.Application;
using Server.Application.Alerts;
using Server.Application.Character;
using Server.Application.GameWorld;
using Server.Interactables;
using Shared;
using Shared.Requests.Character;

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

            Loot loot = mediator.GetHandler<World>().GetRoom(player).Get<Loot>();
            if(loot != null && !loot.RemoveItem(request.Item))
                return Response.Fail("Not enough lootable items");
            
            mediator.GetHandler<PlayerInventory>().AddItem(playerId, request.Item).Save();
            mediator.GetHandler<PickupItemAlerter>().Send(player.LocationId, request.Item, request.Name);
            return Response.Ok();
        }
    }
}
