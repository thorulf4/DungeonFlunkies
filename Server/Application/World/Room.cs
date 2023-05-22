using Server.Interactables;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.GameWorld
{
    public class Room
    {
        public int Id { get; set; }
        public List<Interactable> Interactables { get; set; } = new();
        private int next_index = 0;

        public Room(int id)
        {
            Id = id;
        }

        public T GetOrCreate<T>(T defaultInteractable) where T : Interactable
        {
            T interactable = Get<T>();
            if(interactable != null)
            {
                return interactable;
            }
            else
            {
                return Create(defaultInteractable);
            }
        }

        public T Create<T>(T interactable) where T : Interactable
        {
            interactable.Id = next_index;
            next_index++;
            Interactables.Add(interactable);
            return interactable;
        }

        public T Get<T>() where T : Interactable => Interactables.FirstOrDefault(i => i is T) as T;

        public InteractionDescriptor[] GetDescriptors(GameDb context)
        {
            return Interactables.Select(i => i.GetDescriptor(context)).ToArray();
        }

        public IInteractable GetInteraction(int id)
        {
            return Interactables.FirstOrDefault(i => i.Id == id);
        }

        public void Remove(JoinCombat interaction)
        {
            Interactables.Remove(interaction);
        }
    }
}
