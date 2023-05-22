using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Effects
{
    public abstract class Effect
    {
        public int TurnsLeft { get; set; }

        public abstract void Tick(CombatEntity entity);
    }
}
