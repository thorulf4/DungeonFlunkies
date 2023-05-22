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

        public abstract void OnEffectStart(CombatEntity entity);
        public abstract void Tick(CombatEntity entity);
        public abstract void OnEffectEnd(CombatEntity entity);
    }
}
