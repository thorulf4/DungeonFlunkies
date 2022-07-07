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

        public void SetScene(Scene newScene, bool forceful = true)
        {
            if (forceful)
            {
                foreach (Scene scene in scenes)
                    scene.Unload();

                scenes.Clear();
            }
            else
            {
                CurrentScene.Unload();
                scenes.Pop();
            }

            CurrentScene = newScene;

            scenes.Push(newScene);

            Notify("CurrentScene");
        }

        public void SetScene<TScene>(bool forceful = true) where TScene : Scene
        {
            Scene newScene = CreateScene<TScene>();

            SetScene(newScene, forceful);
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
            Pop();
            CurrentScene.Refresh();
            Notify("CurrentScene");
        }

        private void Pop()
        {
            scenes.Pop();
            CurrentScene.Unload();
            CurrentScene = scenes.Peek();
        }

        public void PopWithChildren(Scene scene)
        {
            while(CurrentScene != scene)
                Pop();

            //After popping children also pop scene
            PopScene();
        }

        public void PushScene(Scene scene)
        {
            scenes.Push(scene);
            CurrentScene = scene;
            CurrentScene.Refresh();
            Notify("CurrentScene");
        }

        public T PushScene<T>() where T : Scene
        {
            var scene = CreateScene<T>();
            scenes.Push(scene);
            CurrentScene = scene;
            Notify("CurrentScene");
            return scene;
        }

        public bool StackContains(Scene scene)
        {
            return scenes.Contains(scene);
        }
    }
}
