using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public class Room : Entity
    {
        public Room()
        {
            Interactables = new List<Interactable>();
        }

        public virtual ICollection<Interactable> Interactables { get; set; }
    }
}
