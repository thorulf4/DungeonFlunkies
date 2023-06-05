using Microsoft.EntityFrameworkCore;
using Server.Application.Combat.Effects;
using Server.Model.Items;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Skills
{
    public class SkillManager
    {
        private readonly GameDb context;
        private readonly Dictionary<int, Skill> skills = new();
        private int next_id = 0;
        private readonly Dictionary<string, List<Skill>> templates = new();

        public SkillManager(GameDb context)
        {
            this.context = context;

            AddItemTemplate("Sword", new List<Skill>()
            {
                new DamageSkill("Swing", 1)
                {
                    UsesAction = true
                },
                new DamageSkill("Heavy strike", 1.25f)
                {
                    UsesAction = true,
                    Cooldown = 2
                }
            });

            MageArcheType();
            TankArcheType();
            RogueArcheType();
        }

        private void MageArcheType()
        {
            AddItemTemplate("MageRobe", new List<Skill>()
            {
                new ReduceCooldownsSkill("Focus energy")
                {
                    UsesAction = true,
                    Cooldown = 1,
                    Amount = 3
                }
            });
            AddItemTemplate("MageHat", new List<Skill>()
            {
                new ApplyEffectSkill("Empower", TargetType.Self)
                {
                    EffectProvider = () => new PassiveEffect()
                    {
                        TurnsLeft = 1,
                        DamageModifier = 0.33f
                    },
                    Cooldown = 5,
                    UsesBonusAction = true
                }
            });
            AddItemTemplate("FireStaff", new List<Skill>()
            {
                new DamageSkill("Fire ball", 1.5f)
                {
                    UsesAction = true,
                    Cooldown = 7,
                },
                new AoeDamageSkill("Fire wave", 0.75f)
                {
                    UsesAction = true,
                    Cooldown = 10
                }
            });
            AddItemTemplate("NatureStaff", new List<Skill>()
            {
                new HealSkill("Heal", TargetType.Allies, 1)
                {
                    UsesAction = true,
                    Cooldown = 15
                },
                new DamageSkill("Poison Bramble", 0.25f)
                {
                    UsesAction = true,
                    Cooldown = 8,
                    EffectProvider = () => new BleedEffect()
                    {
                        TurnsLeft = 2,
                        DamagePerTurn = 20
                    }
                }
            });
            AddItemTemplate("ArcaneWand", new List<Skill>()
            {
                new DamageSkill("Magic Missle", 0.6f)
                {
                    UsesBonusAction = true,
                    Cooldown = 1
                }
            });
            AddItemTemplate("MageSandals", new List<Skill>()
            {
                new RefreshActionSkill("Enlightened Action")
                {
                    UsesBonusAction = true,
                    RefreshAction = true,
                    Cooldown = 10
                }
            });
        }

        private void TankArcheType()
        {
            AddItemTemplate("Shield", new List<Skill>()
            {
                new DamageSkill("Shield bash", 0.25f)
                {
                    UsesBonusAction=true,
                    Cooldown = 3,
                    EffectProvider = () => new StunEffect(1)
                },
                new DefendSkill("Shield wall")
                {
                    UsesAction=true,
                    DamageReduction=0.75f,
                    Cooldown = 3
                }
            });
            AddItemTemplate("TauntShoes", new List<Skill>()
            {
                new TauntSkill("Taunting dance")
                {
                    Cooldown = 5
                }
            });
            AddItemTemplate("MetalArmor", new List<Skill>()
            {
                new DefendSkill("Stand tall") // Refactor this into a passive effect
                {
                    DamageReduction = 0.25f,
                    Cooldown = 1
                }
            });
            AddItemTemplate("MetalHelmet", new List<Skill>()
            {
                new HealSkill("Rejuvanate", TargetType.Self, 0.25f)
                {
                    UsesBonusAction = true,
                    Cooldown = 3
                }
            });
        }

        private void RogueArcheType()
        {
            AddItemTemplate("FastCap", new List<Skill>()
            {
                new ReduceCooldownsSkill("Refresh")
                {
                    Cooldown = 3,
                    Amount = 1
                }
            });
            AddItemTemplate("LeatherArmor", new List<Skill>()
            {
                new ApplyEffectSkill("Blade Dancer", TargetType.Self)
                {
                    EffectProvider = () => new PassiveEffect()
                    {
                        TurnsLeft = 1,
                        FlatDamageIncrease = 15
                    },
                    Cooldown = 3
                }
            });
            AddItemTemplate("FastShoes", new List<Skill>()
            {
                new RefreshActionSkill("Hasten")
                {
                    Cooldown = 2,
                    RefreshBonusAction = true
                }
            }); 
            AddItemTemplate("Dagger", new List<Skill>()
            {
                new DamageSkill("Lashing strike", 0.25f)
                {
                    UsesAction = true,
                    EffectProvider = () => new BleedEffect()
                    {
                        DamagePerTurn = 30,
                        TurnsLeft = 2
                    },
                    Cooldown = 3
                },
                new DamageSkill("Quick strike", 0.50f)
                {
                    UsesBonusAction=true
                }
            });
        }

        private void AddItemTemplate(string name, List<Skill> templatedSkill)
        {
            foreach(Skill skill in templatedSkill)
            {
                skill.Id = next_id++;
                skills.Add(skill.Id, skill);
            }
            templates.Add(name, templatedSkill);
        }

        public List<Skill> GetFromPlayer(int playerId)
        {
            var items = context.Equipped.Where(e => e.PlayerId == playerId).Include(e => e.Item).Select(o => ((Equipment)o.Item).EquipmentTemplate);
            return items.SelectMany(i => templates[i]).ToList();
        }

        public List<LoadedSkill> GetLoadedFromPlayer(int playerId)
        {
            List<Equipment> items = context.Equipped.Where(e => e.PlayerId == playerId).Include(e => e.Item).Select(o => (Equipment)o.Item).ToList();
            return items.SelectMany(i => templates[i.EquipmentTemplate].Select(s => new LoadedSkill(s, i.ItemPower))).ToList();
        }

        public Skill Get(SkillDescriptor skill)
        {
            return skills[skill.Id];
        }

        public Skill Get(int skillId)
        {
            return skills[skillId];
        }
    }
}
