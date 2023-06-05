using Server.Application.Combat.Skills;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat
{
    public class LoadedSkill : IDescriptable<SkillDescriptor>
    {
        public int CurrentCooldown { get; set; }
        public Skill skill;
        public int ItemPower { get; set; }

        public LoadedSkill(int id, Skill skill, int itemPower)
        {
            Id = id;
            CurrentCooldown = 0;
            this.skill = skill;
            ItemPower = itemPower;
        }

        public int Id { get; set; }
        public string Name => skill.Name;
        public string Description => skill.Description;
        public int Cooldown => skill.Cooldown;



        public bool UsesAction => skill.UsesAction;
        public bool UsesBonusAction => skill.UsesBonusAction;

        public bool OnCooldown => CurrentCooldown > 0;

        public void Apply(Encounter encounter, CombatEntity user, CombatEntity target)
        {
            if (OnCooldown)
                throw new Exception("Cannot use ability thats on cooldown");

            skill.Apply(encounter, user, target, ItemPower);
            CurrentCooldown = Cooldown;
        }

        public SkillDescriptor GetDescriptor()
        {
            int cooldown = CurrentCooldown;
            if (cooldown == -1)
                cooldown = skill.Cooldown;

            return new SkillDescriptor()
            {
                Id = Id,
                Name = skill.Name,
                Description = skill.Description,
                Cooldown = skill.Cooldown,
                CurrentCooldown = cooldown,
                UsesAction = skill.UsesAction,
                UsesBonusAction = skill.UsesBonusAction,
                TargetType = skill.TargetType
            };
        }

        public void LowerCooldown(int amount = 1)
        {
            CurrentCooldown = Math.Max(0, CurrentCooldown - amount);
        }

        public void PutOnCooldown()
        {
            CurrentCooldown = Cooldown;
        }
    }
}
