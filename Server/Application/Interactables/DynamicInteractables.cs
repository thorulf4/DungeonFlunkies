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
        private static Dictionary<int, List<DynamicInteractable>> interactables = new ();
        int nextAvailableId = 0;

        public void AddInteractable(int roomId, DynamicInteractable interactable)
        {
            interactable.Id = nextAvailableId;
            nextAvailableId = nextAvailableId + 1 % (int.MaxValue - 1);

            if (!interactables.ContainsKey(roomId))
                interactables.Add(roomId, new());

            interactables[roomId].Add(interactable);
        }
        
        public void Remove(int roomId, DynamicInteractable interactable)
        {
            interactables[roomId].Remove(interactable);
        }

        public List<DynamicInteractable> GetForRoom(int roomId)
        {
            if (!interactables.ContainsKey(roomId))
                return new List<DynamicInteractable>();

            //Copy to limit race condition window
            return interactables[roomId].ToList();
        }
    }
}
