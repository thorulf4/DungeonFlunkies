using Server.Model;
using Shared;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Shared.Responses;
using Server.Application;
using Shared.Descriptors;
using Server.Model.Items;

namespace Server.Interactables
{
    public class Loot : Interactable
    {
        public Dictionary<int, Descriptor> Items { get; set; } = new();

        public override InteractionDescriptor GetDescriptor(GameDb context)
        {
            return new InteractionDescriptor
            {
                ActionName = "Loot",
                Id = Id
            };
        }

        public override Response Interact(Player player, GameDb context, Mediator mediator)
        {
            return Response.From(new LootResponse());
        }

        public void AddItems(List<Descriptor> droppedItems)
        {
            foreach(Descriptor item in droppedItems)
            {
                AddItem(item);
            }
        }

        public void AddItem(Descriptor item)
        {
            if (Items.TryGetValue(item.ItemId, out Descriptor descriptor))
            {
                descriptor.Count += item.Count;
            }
            else
            {
                Items.Add(item.ItemId, item.Clone());
            }
        }

        public bool RemoveItem(Descriptor item)
        {

            if (Items.TryGetValue(item.ItemId, out Descriptor descriptor) && descriptor.Count >= item.Count)
            {
                descriptor.Count -= item.Count;
                if (descriptor.Count == 0) 
                    Items.Remove(item.ItemId);
                return true;
            }
            else {
                return false;
            }
        }

        public IEnumerable<Descriptor> GetItems()
        {
            return Items.Values;
        }
    }
}
