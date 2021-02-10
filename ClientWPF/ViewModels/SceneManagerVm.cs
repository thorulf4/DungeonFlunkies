using ClientWPF.Scenes;
using ClientWPF.Scenes.Character;
using ClientWPF.Scenes.Combat;
using ClientWPF.Scenes.RoomScene;
using ClientWPF.Scenes.StartScreen;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.ViewModels
{
    public class SceneManagerVm : ViewModel
    {
        public Scene CurrentScene { get; set; }
        private IServiceProvider provider;

        private Stack<Scene> scenes = new Stack<Scene>();

        public void SetServiceProvider(IServiceProvider provider)
        {
            this.provider = provider;
            CurrentScene = CreateScene<StartScreenVm>();
        }

        public TScene CreateScene<TScene>() where TScene : Scene
        {
            return provider.GetRequiredService<TScene>();
        }

        public void SetScene(Scene scene, bool forceful = true)
        {
            if (forceful)
                scenes = new Stack<Scene>();
            else
                scenes.Pop();

            CurrentScene.Unload();
            CurrentScene = scene;

            scenes.Push(scene);

            Notify("CurrentScene");
        }

        public void SetScene<TScene>(bool forceful = true) where TScene : Scene
        {
            if (forceful)
            {
                foreach (Scene scene in scenes)
                    scene.Unload();
            }
            else
            {
                scenes.Pop();
                CurrentScene.Unload();
            }

            CurrentScene = CreateScene<TScene>();

            scenes.Push(CurrentScene);

            Notify("CurrentScene");
        }

        public void LookupScene(Response response)
        {
            if (response.Success)
            {
                if(response.data is LootResponse)
                {
                    PushScene<InventoryVm>();
                }else if(response.data is CombatEncounterResponse)
                {
                    PushScene<ChooseVm>();
                }
                else
                {
                    //default case will just go back to room
                    SetScene<RoomVm>(forceful: true);
                }
            }
            else
            {
                //Error page later for some sort of error handling
                throw new Exception("Oh god this shouldnt happen D:");
            }
        }

        public void PopScene()
        {
            scenes.Pop();
            CurrentScene.Unload();
            CurrentScene = scenes.Peek();
            Notify("CurrentScene");
        }

        public void PushScene<T>(T scene) where T : Scene
        {
            scenes.Push(scene);
            CurrentScene = scene;
            Notify("CurrentScene");
        }

        public void PushScene<T>() where T : Scene
        {
            var scene = provider.GetRequiredService<T>();
            scenes.Push(scene);
            CurrentScene = scene;
            Notify("CurrentScene");
        }

        public bool StackContains<T>(T scene) where T : Scene
        {
            return scenes.Contains(scene);
        }
    }
}
