using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Scenes.Character
{
    public interface IItemContainer
    {
        void SetCount(DroppableItem item, int count);
        int Add(DroppableItem item);
        void Remove(DroppableItem item);
        bool FitsInSlot(DroppableItem item);
    }
}
