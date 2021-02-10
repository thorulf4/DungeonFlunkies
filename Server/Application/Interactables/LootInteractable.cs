using Microsoft.EntityFrameworkCore;
using Server.Application.Alerts;
using Server.Interactables;
using Server.Model;
using Server.Model.Items;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Interactables
{
    public class LootInteractable : IApplicationLogic
    {
        private readonly GameDb context;
        private readonly Mediator mediator;

        public LootInteractable(GameDb context, Mediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public List<Descriptor> GetItemDescriptors(Loot loot)
        {
            var list = new List<Descriptor>();
            var items = context.InteractionItems.Where(i => i.InteractableId == loot.Id)
                                                .Include(i => i.Item)
                                                .ToList();

            foreach (var interactionItem in items)
            {
                if (interactionItem.Item is Equipment eq)
                {
                    list.Add(new EquipmentDescriptor(eq.Id, eq.Name, interactionItem.Count, eq.Type));
                }
                else
                {
                    list.Add(new ItemDescriptor(interactionItem.Item.Id, interactionItem.Item.Name, interactionItem.Count));
                }
            }
            return list;
        }

        public ISavable AddItem(Loot loot, Descriptor item)
        {
            var lootItem = context.InteractionItems.Where(i => i.ItemId == item.ItemId && i.InteractableId == loot.Id).SingleOrDefault();

            if (lootItem == null)
            {
                lootItem = new InteractableItem(loot.Id, item.ItemId);
                lootItem.Count += item.Count;
                context.Add(lootItem);
            }
            else
            {
                lootItem.Count += item.Count;
                context.Update(lootItem);
            }

            return context;
        }

        public ISavable RemoveItem(Loot loot, Descriptor item)
        {
            var inventoryItem = context.InteractionItems.FirstOrDefault(i => i.ItemId == item.ItemId && i.InteractableId == loot.Id);
            inventoryItem.Count -= item.Count;

            if (inventoryItem.Count <= 0)
            {
                context.Remove(inventoryItem);

                if (context.InteractionItems.Where(i => i.InteractableId == loot.Id).Count() <= 1)
                {
                    mediator.GetHandler<GetInteractable>().Delete(loot.Id).Save();
                    mediator.GetHandler<RoomUpdateAlerter>().Send(loot.RoomId);
                }
            }
            else
                context.Update(inventoryItem);


            return context;
        }

        public ISavable AddItems(Loot loot, List<Descriptor> items)
        {
            foreach (var item in items)
                AddItem(loot, item);

            return context;
        }
    }
}
