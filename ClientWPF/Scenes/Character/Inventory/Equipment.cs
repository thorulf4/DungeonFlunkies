using ClientWPF.Scenes.Character;
using ClientWPF.Utils;
using ClientWPF.ViewModels;
using Shared;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Scenes.Character
{
    public class Equipment : ViewModel
    {
        public EquipmentSlot Head { get; set; }
        public EquipmentSlot Torso { get; set; }
        public EquipmentSlot Legs { get; set; }
        public EquipmentSlot LeftHand { get; set; }
        public EquipmentSlot RightHand { get; set; }

        public Equipment()
        {
            Head = new EquipmentSlot(EquipmentType.Head, 0);
            Torso = new EquipmentSlot(EquipmentType.Torso, 0);
            Legs = new EquipmentSlot(EquipmentType.Legs, 0);
            LeftHand = new EquipmentSlot(EquipmentType.Holdable, 0);
            RightHand = new EquipmentSlot(EquipmentType.Holdable, 1);
        }

        internal void EquipAll(Dictionary<EquipmentType, Dictionary<int, EquipmentDescriptor>> equippedItems, EventHandler<DroppableItem.ItemMovedEventArgs> onItemMoved)
        {
            TryEquip(equippedItems.GetOrDefault(EquipmentType.Head, 0), Head, onItemMoved);
            TryEquip(equippedItems.GetOrDefault(EquipmentType.Torso, 0), Torso, onItemMoved);
            TryEquip(equippedItems.GetOrDefault(EquipmentType.Legs, 0), Legs, onItemMoved);
            TryEquip(equippedItems.GetOrDefault(EquipmentType.Holdable, 0), LeftHand, onItemMoved);
            TryEquip(equippedItems.GetOrDefault(EquipmentType.Holdable, 1), RightHand, onItemMoved);
        }

        private void TryEquip(EquipmentDescriptor item, EquipmentSlot slot, EventHandler<DroppableItem.ItemMovedEventArgs> onItemMoved)
        {
            if(item != null)
                new DroppableItem(item, slot).OnItemMoved += onItemMoved;
        }
    }
}
