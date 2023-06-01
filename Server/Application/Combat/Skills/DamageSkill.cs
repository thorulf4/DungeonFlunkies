﻿using Server.Application.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat.Skills
{
    public class DamageSkill : Skill
    {
        public float DamageRatio { get; set; }

        public override TargetType TargetType => TargetType.Enemies;

        public DamageSkill(string name, float damageRatio) : base(name)
        {
            DamageRatio = damageRatio;
        }

        public override void Apply(Encounter encounter, CombatEntity target, int ItemPower)
        {
            target.TakeDamage((int)(ItemPower * DamageRatio));
        }
    }
}
