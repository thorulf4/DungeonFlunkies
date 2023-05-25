using ClientWPF.ViewModels;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Scenes.Combat
{
    public class Target : ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; } 
        public int MaxHealth { get; set; }
        public string Action { get; set; }

        public Target(EntityDescriptor entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Health = entity.Health;
            MaxHealth = entity.MaxHealth;
            Action = entity.Action;
        }
    }
}
