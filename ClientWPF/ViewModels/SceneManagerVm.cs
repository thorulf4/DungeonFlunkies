using ClientWPF.Scenes;
using ClientWPF.Scenes.RoomScene;
using ClientWPF.Scenes.StartScreen;
using Microsoft.Extensions.DependencyInjection;
using Shared;
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

        public TScene CreateScene<TScene>() where TScene : Scene
        {
            return provider.GetRequiredService<TScene>();
        }

        public void SetScene(Scene scene)
        {
            CurrentScene.Unload();
            CurrentScene = scene;
            Notify("CurrentScene");
        }

        public void SetScene<TScene>() where TScene : Scene
        {
            CurrentScene.Unload();
            CurrentScene = CreateScene<TScene>();
            Notify("CurrentScene");
        }

        public void LookupScene(Response response)
        {
            if (response.Success)
            {
                //Always use default case for now
                SetScene<RoomVm>();
            }
            else
            {
                //Error page later for some sort of error handling
                throw new Exception("Oh god this shouldnt happen D:");
            }
        }
    }
}
