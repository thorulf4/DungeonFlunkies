using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Descriptors
{
    public class SkillDescriptor
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cooldown { get; set; }
        public int CurrentCooldown { get; set; }

        public SkillDescriptor(string name, string description, int cooldown)
        {
            Name = name;
            Description = description;
            Cooldown = cooldown;
            CurrentCooldown = 0;
        }
        //Add effect later?


    }
}
