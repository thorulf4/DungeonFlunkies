using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Descriptors
{
    public class EntityDescriptor
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public EntityDescriptor(string name, int health, int maxHealth)
        {
            Name = name;
            Health = health;
            MaxHealth = maxHealth;
        }
    }
}
