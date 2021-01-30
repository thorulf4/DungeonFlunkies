using ClientWPF.Utils.Wpf;
using ClientWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Scenes.StartScreen
{
    public class StartScreenVm : Scene
    {
        public StartScreenVm(SceneManagerVm sceneManager) : base(sceneManager)
        {

        }

        public string Name { get; set; }
        public string Password { get; set; }

        public RelayCommand Login { get
            {
                return new RelayCommand(o =>
                {

                });
            }
        }

        public RelayCommand CreateCharacter
        {
            get
            {
                return new RelayCommand(o =>
                {

                });
            }
        }
    }
}
