using ClientWPF.Scenes;
using ClientWPF.Scenes.StartScreen;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.ViewModels
{
    public class SceneManagerVm : ViewModel
    {
        public Scene CurrentScene { get; set; }

        public SceneManagerVm()
        {
            CurrentScene = new StartScreenVm(this);
        }

        public void SetScene(Scene scene)
        {
            CurrentScene = scene;
            Notify("CurrentScene");
        }
    }
}
