using Server.Application.Interactables;
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
        private readonly Mediator mediator;

        public GetInteractable(GameDb context, Mediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public Interactable Get(int id)
        {
            return context.Interactables.Find(id);
        }

        public T GetInRoom<T>(int roomId) where T : Interactable
        {
            return GetInRoom(roomId).SingleOrDefault(i => i is T) as T;
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

        public List<IInteractable> GetInRoom(int roomId)
        {
            return context.Interactables.Where(i => i.RoomId == roomId).ToList().Union<IInteractable>(mediator.GetHandler<DynamicInteractables>().GetForRoom(roomId)).ToList();
        }

        public ISavable Delete(int id)
        {
            context.Remove(context.Interactables.Find(id));
            return context;
        }
    }
}
