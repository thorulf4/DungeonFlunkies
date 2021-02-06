using ClientWPF.Utils.Wpf;
using ClientWPF.ViewModels;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClientWPF.Scenes.Character
{
    public class EquipmentSlot : ViewModel, IItemContainer
    {
        public DroppableItem Item { get; set; }

        public EquipmentType Type { get; set; }
        public int Slot { get; set; }

        public EquipmentSlot(EquipmentType type, int slot)
        {
            Type = type;
            Slot = slot;
        }

        public RelayCommand Drop
        {
            get
            {
                return new RelayCommand(o =>
                {
                    DragEventArgs dragEventArgs = (DragEventArgs)o;
                    var item = (DroppableItem)dragEventArgs.Data.GetData(typeof(DroppableItem));
                    item.MoveTo(this);
                });
            }
        }

        public void SetCount(DroppableItem item, int count)
        {
            if (count > 1)
                throw new Exception("Cant be over 1");

            if (count == 0)
                Item = null;
            Notify("Item");
        }

        public int Add(DroppableItem item)
        {
            if (Item != null)
                throw new Exception("Cant equip weapon without removing existing on first");

            Item = item;
            Notify("Item");

            return 1;
        }

        public void Remove(DroppableItem item)
        {
            if (Item != item)
                throw new Exception("Removing wrong item");

            Item = null;
            Notify("Item");
        }

        public bool FitsInSlot(DroppableItem item)
        {
            return item.SlotType != null && item.SlotType == Type;
        }
    }
}
