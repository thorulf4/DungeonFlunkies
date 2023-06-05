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

        protected Skill(string name)
        {
            Name = name;
        }

        public abstract TargetType TargetType { get; }
        public abstract void Apply(Encounter encounter, CombatEntity user, CombatEntity target, int ItemPower);
    }
}
