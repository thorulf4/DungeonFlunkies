using ClientWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Scenes
{
    public class Scene : ViewModel
    {
        protected SceneManagerVm sceneManager;
        protected RequestClient client;

        public Scene(SceneManagerVm sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public void GiveDependencies(SceneManagerVm sceneManager, RequestClient client)
        {
            this.sceneManager = sceneManager;
            this.client = client;
        }
    }
}
