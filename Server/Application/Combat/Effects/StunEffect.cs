using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Effects
{
    public class StunEffect : Effect
    {
        public StunEffect(int duration)
        {
            TurnsLeft = duration;
        }

        public override void OnEffectEnd(CombatEntity entity)
        {
        }

        public override void OnEffectStart(CombatEntity entity)
        {
            Tick(entity);
        }

        public override void Tick(CombatEntity entity)
        {
            if(entity is Enemy enemy)
            {
                enemy.plannedAction = null;
            }else if(entity is CombatPlayer player)
            {
                player.hasAction = false;
                player.hasBonusAction = false;
            }
        }
    }
}
