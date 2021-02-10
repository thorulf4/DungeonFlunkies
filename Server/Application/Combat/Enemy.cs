using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat
{
    public class Enemy
    {
        public string name;
        public int health;
        public int maxHealth;
        public List<SkillDescriptor> skills;

        public Enemy(string name, int maxHealth, List<SkillDescriptor> skills)
        {
            this.name = name;
            this.maxHealth = maxHealth;
            this.skills = skills;
            health = maxHealth;
        }
    }
}
