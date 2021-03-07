using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat
{
    public abstract class CombatEntity
    {
        public int Id { get; set; }
        public string name;
        public int health;
        public int maxHealth;
        public List<SkillDescriptor> skills;

        public bool alive;

        public virtual void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        public virtual void Die()
        {
            alive = false;
        }
    }
}
