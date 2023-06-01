using Microsoft.EntityFrameworkCore;
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
        private readonly Dictionary<string, List<Skill>> templates = new(); // Missing weapon details

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
            AddItemTemplate("Dagger", new List<Skill>()
            {
                new DamageSkill("Quick strike", 0.50f)
                {
                    UsesBonusAction=true
                }
            });
            AddItemTemplate("Shield", new List<Skill>()
            {
                new StunSkill("Shield bash", 1, 0.25f)
                {
                    UsesBonusAction=true,
                    Cooldown = 3
                },
                new DefendSkill("Shield wall")
                {
                    UsesAction=true,
                    DamageReduction=0.75f,
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
            AddItemTemplate("TauntShoes", new List<Skill>()
            {
                new TauntSkill("Taunting dance")
                {
                    Cooldown = 5
                }
            });
        }

        private void AddItemTemplate(string name, List<Skill> templatedSkill)
        {
            foreach(Skill skill in templatedSkill)
            {
                skill.Id = next_id;
                next_id++;
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
