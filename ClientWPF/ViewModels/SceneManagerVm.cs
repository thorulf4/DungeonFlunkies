using ClientWPF.Scenes;
using ClientWPF.Scenes.StartScreen;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.ViewModels
{
    public class SceneManagerVm : ViewModel
    {
        public Scene CurrentScene { get; set; }
        private IServiceProvider provider;

        public void SetServiceProvider(IServiceProvider provider)
        {
            this.provider = provider;
            CurrentScene = CreateScene<StartScreenVm>();
        }

        public Scene CreateScene<TScene>() where TScene : Scene
        {
            return provider.GetRequiredService<TScene>();
        }

        public void SetScene(Scene scene)
        {
            CurrentScene = scene;
            Notify("CurrentScene");
        }

        public void SetScene<TScene>() where TScene : Scene
        {
            CurrentScene = CreateScene<TScene>();
            Notify("CurrentScene");
        }
    }
}
