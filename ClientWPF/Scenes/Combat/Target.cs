using ClientWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Scenes.Combat
{
    public class Target : ViewModel
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
    }
}
