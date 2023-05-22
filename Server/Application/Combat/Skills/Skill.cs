using Server.Application.Combat;
using Server.Model;
using Server.Model.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat.Skills
{
    public abstract class Skill
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int Cooldown { get; set; }

        public bool UsesAction { get; set; }
        public bool UsesBonusAction { get; set; }

        public abstract void Apply(CombatEntity target, int ItemPower);
    }
}
