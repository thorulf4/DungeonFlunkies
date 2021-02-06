using ClientWPF.Utils.Wpf;
using ClientWPF.ViewModels;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ClientWPF.Scenes.Character
{
    public class DroppableList : ViewModel, IEnumerable<DroppableItem>, IItemContainer
    {
        public List<DroppableItem> itemDescriptors = new List<DroppableItem>();

        public DroppableItem this[int index] { get { return itemDescriptors[index]; } set { itemDescriptors[index] = value; } }

        public IReadOnlyList<DroppableItem> Items { get; private set; }

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

        public int Count => itemDescriptors.Count();

        public bool IsReadOnly => false;

        public int Add(DroppableItem item)
        {
            if (itemDescriptors.Any(i => i.ItemId == item.ItemId))
            {
                itemDescriptors.First(i => i.ItemId == item.ItemId).Count += item.Count;
            }
            else
            {
                itemDescriptors.Add(item);
            }

            Items = itemDescriptors.AsReadOnly();
            Notify("Items");
            return item.Count;
        }

        public bool Contains(DroppableItem item)
        {
            return itemDescriptors.Contains(item);
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(DroppableItem item)
        {
            if (itemDescriptors.Contains(item))
            {
                itemDescriptors.Remove(item);

                Items = itemDescriptors.AsReadOnly();
                Notify("Items");

                return true;
            }

            return false;
        }

        IEnumerator<DroppableItem> IEnumerable<DroppableItem>.GetEnumerator()
        {
            return itemDescriptors.GetEnumerator();
        }

        public void SetCount(DroppableItem item, int count)
        {
            item.Count = count;
            Items = itemDescriptors.AsReadOnly();
            Notify("Items");
        }

        void IItemContainer.Remove(DroppableItem item)
        {
            Remove(item);
        }

        public bool FitsInSlot(DroppableItem item)
        {
            return true;
        }
    }
}
