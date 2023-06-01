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

        public int ModifyDamage(int damage)
        {
            float damagePercent = Math.Max(0, 1f - DamageReduction);
            return (int)(damage * damagePercent);
        }
    }
}
