using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Skills
{
    public class SpawnHelperSkill : Skill
    {
        private readonly Func<Enemy> producer;
        public int EnemyCount { get; set; } = 1;

        public SpawnHelperSkill(string name, Func<Enemy> producer) : base(name)
        {
            this.producer = producer;
        }

        public override TargetType TargetType => TargetType.Self;

        public override void Apply(Encounter encounter, CombatEntity user, CombatEntity target, int ItemPower)
        {
            int maxSpawn = GameSettings.MaxEnemyCount - encounter.GetAliveAi().Count;
            int left = Math.Min(EnemyCount, maxSpawn);

            while(left-->0)
                encounter.AddEnemy(producer.Invoke());
        }
    }
}
