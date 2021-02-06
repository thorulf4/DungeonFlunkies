using Server.Model.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public class InteractableItem : Entity
    {
        public InteractableItem(int interactableId, int itemId)
        {
            ItemId = itemId;
            InteractableId = interactableId;
            Count = 0;
        }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int Count { get; set; }

        public int InteractableId { get; set; }
        public Interactable Interactable { get; set; }

    }
}
