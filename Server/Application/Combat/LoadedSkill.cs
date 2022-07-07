using Server.Model.Skills;
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

        public LoadedSkill(Skill skill)
        {
            CurrentCooldown = 0;
            this.skill = skill;
        }

        public int Id => skill.Id;
        public string Name => skill.Name;
        public string Description => skill.Description;
        public int Cooldown => skill.Cooldown;



        public bool UsesAction => skill.UsesAction;
        public bool UsesBonusAction => skill.UsesBonusAction;

        public bool OnCooldown => CurrentCooldown > 0;

        internal void Apply(CombatEntity target)
        {
            if (OnCooldown)
                throw new Exception("Cannot use ability thats on cooldown");

            skill.Apply(target);
            CurrentCooldown = Cooldown;
        }

        public SkillDescriptor GetDescriptor()
        {
            return new SkillDescriptor
            {
                Id = skill.Id,
                Name = skill.Name,
                Description = skill.Description,
                Cooldown = skill.Cooldown,
                CurrentCooldown = CurrentCooldown,
                UsesAction = skill.UsesAction,
                UsesBonusAction = skill.UsesBonusAction,
            };
        }
    }
}
