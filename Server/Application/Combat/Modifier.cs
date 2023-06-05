using Server.Application.Combat.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat
{
    public class Modifier
    {
        public float DamageReduction { get; set; } = 0;
        public int FlatDamageIncrease { get; set; } = 0;
        public float DamageModifier { get; set; } = 1f;

        public Dictionary<Type, Action<object>> skillModifiers = new();

        public int ModifyIncomingDamage(int damage)
        {
            float damagePercent = Math.Max(0, 1f - DamageReduction);
            return (int) (damage * damagePercent);
        }

        public int ModifyOutgoingDamage(int damage)
        {
            return (int) (damage * DamageModifier + FlatDamageIncrease);
        }
    }
}
