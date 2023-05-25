using Server.Application.Combat.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Descriptors
{
    public class SkillDescriptor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cooldown { get; set; }
        public int CurrentCooldown { get; set; }

        public bool UsesAction { get; set; }
        public bool UsesBonusAction { get; set; }

        public TargetType TargetType { get; set; }


        public bool OnCooldown => CurrentCooldown > 0; 

        public SkillDescriptor()
        {

        }
    }
}
