using Server.Model;
using Shared;
using Shared.Model.Interactables;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Shared.Responses;

namespace Server.Interactables
{
    public class Loot : Interactable
    {
        public override InteractionDescriptor GetDescriptor(GameDb context)
        {
            return new InteractionDescriptor
            {
                ActionName = "Loot",
                Id = Id
            };
        }

        public override Response Interact(Player player, GameDb context, IAlerter alerter)
        {
            //var playerItems = from owned in context.OwnedBys
            //            join item in context.Items on owned.OwnsId equals item.Id
            //            where owned.OwnerId == player.Id
            //            select new ItemDescriptor(item.Id, item.Name, owned.Count);

            //var lootableItems = from i in context.InteractionItems
            //                    join item in context.Items on i.ItemId equals item.Id
            //                    where i.InteractableId == Id
            //                    select new ItemDescriptor(item.Id, item.Name, i.Count);

            //return Response.From(new LootResponse(playerItems.ToList(), lootableItems.ToList()));

            return Response.From(new LootResponse());
        }
    }
}
