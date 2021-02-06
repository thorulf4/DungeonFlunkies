using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application
{
    public class GetInteractable : IApplicationLogic
    {
        private readonly GameDb context;

        public GetInteractable(GameDb context)
        {
            this.context = context;
        }

        public Interactable Get(int id)
        {
            return context.Interactables.Find(id);
        }

        public T GetInRoom<T>(int roomId) where T : Interactable
        {
            return (from i in context.Interactables
                    where i.RoomId == roomId && i is T
                    select i).SingleOrDefault() as T;
        }

        public T GetOrCreate<T>(int roomId, T defaultT) where T : Interactable
        {
            T interactable = GetInRoom<T>(roomId);
            if(interactable == null)
            {
                interactable = defaultT;
                context.Add(interactable);
                context.SaveChanges();
            }

            return interactable;
        }

        public List<Interactable> GetInRoom(int roomId)
        {
            return context.Interactables.Where(i => i.RoomId == roomId).ToList();
        }

        public ISavable Delete(int id)
        {
            context.Remove(context.Interactables.Find(id));
            return context;
        }
    }
}
