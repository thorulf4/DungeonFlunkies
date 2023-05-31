using Server.Application.Combat.AI;
using Server.Application.Combat.Effects;
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
        public List<Effect> activeEffects = new();

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

        public void HealDamage(int amount)
        {
            health = Math.Min(maxHealth, health + amount);
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
                descriptor.Action = enemy.plannedAction?.ToString()?? "Stunned";
            }

            return descriptor;
        }

        public void AddEffect(Effect effect)
        {
            effect.OnEffectStart(this);
            activeEffects.Add(effect);
        }

        public void HandleEffects()
        {
            for(int i = activeEffects.Count - 1; i >= 0; i--)
            {
                Effect effect = activeEffects[i];

                if(effect.TurnsLeft > 0)
                {
                    effect.Tick(this);
                    effect.TurnsLeft--;
                }
                else
                {
                    activeEffects.RemoveAt(i);
                    effect.OnEffectEnd(this);
                }
            }
        }
    }
}
