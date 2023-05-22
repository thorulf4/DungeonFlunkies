using Server.Application.GameWorld;
using Server.Interactables;
using Server.Model.Items;
using Shared.Descriptors;
using System.Collections.Generic;
using System.Linq;

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
            Loot loot = mediator.GetHandler<World>().GetRoom(roomId).Get<Loot>();
            if (loot == null)
                return new List<Descriptor>();

            return loot.GetItems().ToList();
        }

        public Item Get(int itemId)
        {
            return context.Items.Find(itemId);
        }

        public Descriptor GetDescriptor(int itemId, int count = 1)
        {
            Item item = Get(itemId);
            if (item is Equipment eq)
            {
                return new EquipmentDescriptor(itemId, eq.Name, count, eq.Type);
            }
            else
            {
                return new ItemDescriptor(itemId, item.Name, count);
            }
        }

    }
}
