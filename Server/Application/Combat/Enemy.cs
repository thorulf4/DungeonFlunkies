using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat
{
    public class Enemy : CombatEntity
    {

        public Enemy(string name, int maxHealth, List<SkillDescriptor> skills)
        {
            this.name = name;
            this.maxHealth = maxHealth;
            this.skills = skills;
            health = maxHealth;

            alive = true;
        }

        public override void Die()
        {
            alive = false;
        }
    }
}
