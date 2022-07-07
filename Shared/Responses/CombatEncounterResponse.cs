using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Responses
{
    public class CombatEncounterResponse
    {
        public bool HasEncounter { get; set; }
        public List<SkillDescriptor> Skills { get; set; }
        public EncounterDescriptor Encounter { get; set; }

        public static CombatEncounterResponse CreateEmpty() => new CombatEncounterResponse();

        private CombatEncounterResponse()
        {
            HasEncounter = false;
        }

        public CombatEncounterResponse(List<SkillDescriptor> skills, EncounterDescriptor encounter)
        {
            HasEncounter = true;
            Skills = skills;
            Encounter = encounter;
        }
    }
}
