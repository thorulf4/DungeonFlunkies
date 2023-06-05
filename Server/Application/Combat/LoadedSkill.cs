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

        public LoadedSkill(Skill skill, int itemPower)
        {
            CurrentCooldown = 0;
            this.skill = skill;
            ItemPower = itemPower;
        }

        public int Id => skill.Id;
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
            return skill.GetDescriptor(CurrentCooldown);
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
