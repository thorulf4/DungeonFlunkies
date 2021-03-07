using Server.Interactables;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Interactables
{
    public class DynamicInteractables : IApplicationLogic
    {
        private static Dictionary<int, List<WeakReference<DynamicInteractable>>> interactables = new Dictionary<int, List<WeakReference<DynamicInteractable>>>();
        int nextAvailableId = 0;

        public void AddInteractable(int roomId, DynamicInteractable interactable)
        {
            interactable.Id = nextAvailableId;
            nextAvailableId = nextAvailableId + 1 % (int.MaxValue - 1);

            var reference = new WeakReference<DynamicInteractable>(interactable);

            if (!interactables.ContainsKey(roomId))
                interactables.Add(roomId, new List<WeakReference<DynamicInteractable>>());

            interactables[roomId].Add(reference);
        }

        public List<DynamicInteractable> GetForRoom(int roomId)
        {
            if (!interactables.ContainsKey(roomId))
                return new List<DynamicInteractable>();

            List<DynamicInteractable> output = new List<DynamicInteractable>();
            for(int i = interactables[roomId].Count -1; i>=0; i--)
            {
                WeakReference<DynamicInteractable> element = interactables[roomId][i];

                if(element.TryGetTarget(out DynamicInteractable interactable))
                {
                    output.Add(interactable);
                }
                else
                {
                    interactables[roomId].RemoveAt(i);
                }
            }

            return output;
        }
    }
}
