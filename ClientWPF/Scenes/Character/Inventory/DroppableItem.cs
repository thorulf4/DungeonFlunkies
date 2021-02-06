using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Scenes.Character
{
    public class DroppableItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public EquipmentType? SlotType { get; set; }

        public IItemContainer Location { get; set; }
        public event EventHandler<ItemMovedEventArgs> OnItemMoved;

        public DroppableItem(Descriptor itemDescriptor, IItemContainer startingLocation)
        {
            ItemId = itemDescriptor.ItemId;
            Name = itemDescriptor.Name;
            Count = itemDescriptor.Count;
            Location = startingLocation;
            startingLocation.Add(this);

            if (itemDescriptor is EquipmentDescriptor eq)
                SlotType = eq.Slot;
        }

        public DroppableItem(DroppableItem item, int count, IItemContainer startingLocation)
        {
            ItemId = item.ItemId;
            Name = item.Name;
            Count = count;
            Location = startingLocation;
            startingLocation.Add(this);
        }

        public override string ToString()
        {
            return $"{Count}\t {Name}";
        }

        public List<Descriptor> AsDescriptor()
        {
            if (SlotType == null)
                return new List<Descriptor> { new ItemDescriptor(ItemId, Name, Count) };
            else
                return new List<Descriptor> { new EquipmentDescriptor(ItemId, Name, Count, (EquipmentType)SlotType) };
        }

        public void MoveTo(IItemContainer newLocation)
        {
            if (Location == newLocation || !newLocation.FitsInSlot(this))
                return;

            OnItemMoved?.Invoke(this, new ItemMovedEventArgs
            {
                From = Location,
                To = newLocation,
                Item = this
            });

            int maxAdded = newLocation.Add(this);
            Location?.Remove(this);
            if(maxAdded < Count)
            {
                new DroppableItem(this, Count - maxAdded, Location);
                Count = maxAdded;
            }
            Location = newLocation;
        }

        public void SetCount(int count)
        {
            Location.SetCount(this, count);
        }

        public class ItemMovedEventArgs
        {
            public IItemContainer From { get; set; }
            public IItemContainer To { get; set; }
            public DroppableItem Item { get; set; }
        }
    }
}
