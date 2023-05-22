using Server.Application.Combat.AI;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat
{
    public abstract class CombatEntity : IDescriptable<EntityDescriptor>
    {
        public int Id { get; set; }
        public string name;
        public int health;
        public int maxHealth;
        public List<LoadedSkill> skills;

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

        public EntityDescriptor GetDescriptor()
        {
            var descriptor = new EntityDescriptor()
            {
                Id = Id,
                Health = health,
                MaxHealth = maxHealth,
                Name = name,
                Action = null
            };


            if (this is CombatPlayer player)
            {
                descriptor.HasAction = player.hasAction;
                descriptor.HasBonusAction = player.hasBonusAction;
            }
            else if (this is Enemy enemy)
            {
                descriptor.Action = enemy.plannedAction.ToString();
            }

            return descriptor;
        }
    }
}
