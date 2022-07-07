using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Descriptors
{
    public class EncounterDescriptor
    {
        public List<EntityDescriptor> Enemies { get; set; }
        public List<EntityDescriptor> Allies { get; set; }
        public DateTime TurnEnds { get; set; }

        public EncounterDescriptor(List<EntityDescriptor> enemies, List<EntityDescriptor> allies, DateTime turnEnds)
        {
            Enemies = enemies;
            Allies = allies;
            TurnEnds = turnEnds;
        }
    }
}
