using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Descriptors
{
    public class SkillDescriptor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cooldown { get; set; }
        public int CurrentCooldown { get; set; }

        public bool UsesAction { get; set; }
        public bool UsesBonusAction { get; set; }


        public SkillDescriptor()
        {

        }

        public SkillDescriptor(int id, string name, string description, int cooldown)
        {
            this.Id = id;
            Name = name;
            Description = description;
            Cooldown = cooldown;
            CurrentCooldown = 0;
        }
        //Add effect later?


    }
}
