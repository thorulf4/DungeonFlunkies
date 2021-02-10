using Microsoft.EntityFrameworkCore;
using Server.Application.Interactables;
using Server.Interactables;
using Server.Model;
using Server.Model.Items;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application
{
    public class GetItem : IApplicationLogic
    {
        private readonly GameDb context;
        private readonly Mediator mediator;

        public GetItem(GameDb context, Mediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public List<Descriptor> GetDroppedItems(int roomId)
        {
            Loot loot = mediator.GetHandler<GetInteractable>().GetInRoom<Loot>(roomId);
            if (loot == null)
                return new List<Descriptor>();

            return mediator.GetHandler<LootInteractable>().GetItemDescriptors(loot);
        }

        public Item Get(int itemId)
        {
            return context.Items.Find(itemId);
        }

    }
}
