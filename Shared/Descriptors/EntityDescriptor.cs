using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Descriptors
{
    public class EntityDescriptor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public bool HasAction { get; set; }
        public bool HasBonusAction { get; set; }

        public EntityDescriptor()
        {

        }

        public EntityDescriptor(string name, int health, int maxHealth)
        {
            Name = name;
            Health = health;
            MaxHealth = maxHealth;

            HasAction = true;
            HasBonusAction = true;
        }
    }
}
