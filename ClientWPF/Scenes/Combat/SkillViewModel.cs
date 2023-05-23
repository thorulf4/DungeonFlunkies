using ClientWPF.ViewModels;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientWPF.Scenes.Combat
{
    public class SkillViewModel : ViewModel
    {
        public SkillDescriptor skill;
        public bool IsUsable { get; set; }
        public string Text { get; set; }

        public SkillViewModel(SkillDescriptor skill, bool hasAction, bool hasBonusAction)
        {
            this.skill = skill;
            Update(hasAction, hasBonusAction);
        }

        public void Update(bool hasAction, bool hasBonusAction)
        {
            IsUsable = (!skill.UsesAction || hasAction) && (!skill.UsesBonusAction || hasBonusAction) && !skill.OnCooldown;

            if (skill.OnCooldown)
            {
                Text = $"{skill.Name} ({skill.CurrentCooldown})";
            }
            else
            {
                Text = skill.Name;
            }

            Notify("IsUsable");
            Notify("Text");
        }
    }
}
