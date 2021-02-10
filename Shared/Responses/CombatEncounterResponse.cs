using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Responses
{
    public class CombatEncounterResponse
    {
        public List<SkillDescriptor> Skills { get; set; }
        public List<EntityDescriptor> Enemies { get; set; }
        public List<EntityDescriptor> Allies { get; set; }

        public CombatEncounterResponse(List<SkillDescriptor> skills, List<EntityDescriptor> enemies, List<EntityDescriptor> allies)
        {
            Skills = skills;
            Enemies = enemies;
            Allies = allies;
        }
    }
}
