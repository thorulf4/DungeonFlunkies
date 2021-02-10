using Microsoft.EntityFrameworkCore;
using Server.Model;
using Server.Model.Items;
using Shared;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Character
{
    public class GetEquipment : IApplicationLogic
    {
        private readonly GameDb context;
        private readonly Mediator mediator;

        public GetEquipment(GameDb context, Mediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public List<Equipment> Get(int playerId, EquipmentType type)
        {
            return context.Equipped.Where(e => e.PlayerId == playerId && e.EquipmentType == type).Include(e => e.Item).OrderBy(e => e.Slot).Select(e => e.Item).OfType<Equipment>().ToList();
        }

        public Dictionary<EquipmentType, Dictionary<int, EquipmentDescriptor>> GetAsDictionary(int playerId)
        {
            Dictionary<EquipmentType, Dictionary<int, EquipmentDescriptor>> result = new Dictionary<EquipmentType, Dictionary<int, EquipmentDescriptor>>();

            var list = context.Equipped.Where(e => e.PlayerId == playerId).Include(e => e.Item).ToList();
            foreach(var equipped in list)
            {
                if (!result.ContainsKey(equipped.EquipmentType))
                    result.Add(equipped.EquipmentType, new Dictionary<int, EquipmentDescriptor>());

                EquipmentDescriptor equipmentDescriptor = new EquipmentDescriptor(equipped.Item.Id, equipped.Item.Name, 1, equipped.EquipmentType);

                result[equipped.EquipmentType].Add(equipped.Slot, equipmentDescriptor);
            }

            return result;
        }

        public Dictionary<int, Equipped> GetRelation(int playerId, EquipmentType type)
        {
            return context.Equipped.Where(e => e.PlayerId == playerId && e.EquipmentType == type).OrderBy(e => e.Slot).ToDictionary(e => e.Slot);
        }

        public Dictionary<int, Equipped> GetRelationWithItem(int playerId, EquipmentType type)
        {
            return context.Equipped.Where(e => e.PlayerId == playerId && e.EquipmentType == type).Include(e => e.Item).OrderBy(e => e.Slot).ToDictionary(e => e.Slot);
        }

        public ISavable Unequip(int playerId, EquipmentType type, int slot)
        {
            var equipped = GetRelation(playerId, type).GetValueOrDefault(slot);
            if (equipped != null)
                context.Remove(equipped);

            return context;
        }

        public ISavable Equip(int playerId, Equipment equipment, int slot)
        {
            Equipped equipped = GetRelationWithItem(playerId, equipment.Type).GetValueOrDefault(slot);

            if(equipped == null)
            {
                equipped = new Equipped
                {
                    ItemId = equipment.Id,
                    PlayerId = playerId,
                    Slot = slot,
                    EquipmentType = equipment.Type
                };

                context.Add(equipped);
            }
            else
            {
                mediator.GetHandler<PlayerInventory>().AddItem(playerId, new ItemDescriptor(equipped.Item.Id, equipped.Item.Name, 1));
                equipped.ItemId = equipment.Id;
            }

            return context;
        }
    }
}
