using ClientWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Scenes
{
    public abstract class Scene : ViewModel
    {
        public abstract void Unload();
    }
}
