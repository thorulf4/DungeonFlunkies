using Microsoft.EntityFrameworkCore;
using Server.Model;
using Server.Model.Items;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Character
{
    public class PlayerInventory : IApplicationLogic
    {
        private readonly GameDb context;

        public PlayerInventory(GameDb context)
        {
            this.context = context;
        }

        public List<OwnedBy> GetPlayersItems(int playerId)
        {
            return context.OwnedBys.Where(o => o.OwnerId == playerId).Include(o => o.Owns).ToList();
        }

        public List<Descriptor> GetDescriptors(int playerId)
        {
            List<Descriptor> list = new List<Descriptor>();

            var ownedBys = context.OwnedBys.Where(o => o.OwnerId == playerId).Include(o => o.Owns).ToList();
            foreach(var ownedBy in ownedBys)
            {
                if(ownedBy.Owns is Equipment eq)
                {
                    list.Add(new EquipmentDescriptor(eq.Id, eq.Name, ownedBy.Count, eq.Type));
                }
                else
                {
                    list.Add(new ItemDescriptor(ownedBy.Owns.Id, ownedBy.Owns.Name, ownedBy.Count));
                }
            }
            return list;
        }

        public bool PlayerHasItems(IEnumerable<Descriptor> items, int playerId)
        {
            return items.All(i => PlayerHasItem(i, playerId));
        }

        public bool PlayerHasItem(Descriptor item, int playerId)
        {
            return (from owned in context.OwnedBys
                    where owned.OwnsId == item.ItemId && owned.OwnerId == playerId
                    select owned.Count).Single() >= item.Count;
        }

        public bool PlayerOwns(int playerId, Descriptor item)
        {
            return context.OwnedBys.Any(o => o.OwnerId == playerId && o.OwnsId == item.ItemId && o.Count >= item.Count);
        }

        public ISavable RemoveItem(int playerId, Descriptor item)
        {
            var inventoryItem = context.OwnedBys.Find(item.ItemId, playerId);
            inventoryItem.Count -= item.Count;

            if (inventoryItem.Count <= 0)
                context.Remove(inventoryItem);
            else
                context.Update(inventoryItem);

            return context;
        }

        public ISavable RemoveItems(int playerId, List<Descriptor> items)
        {
            foreach (var item in items)
                RemoveItem(playerId, item);

            return context;
        }

        public ISavable AddItem(int playerId, Descriptor item)
        {
            var lootItem = context.OwnedBys.Where(i => i.OwnsId == item.ItemId && i.OwnerId == playerId).SingleOrDefault();

            if (lootItem == null)
            {
                lootItem = new OwnedBy
                {
                    OwnsId = item.ItemId,
                    OwnerId = playerId,
                    Count = item.Count
                };
                context.Add(lootItem);
            }
            else
            {
                lootItem.Count += item.Count;
                context.Update(lootItem);
            }

            return context;
        }
    }
}
