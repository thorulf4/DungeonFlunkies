using ClientWPF.Utils.Wpf;
using ClientWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ClientWPF.Scenes.Combat
{
    public class ChooseTargetVm : Scene
    {
        private readonly SceneManagerVm sceneManager;
        private readonly Action<Target> callback;

        public ChooseVm ParentScene { get; set; }

        public ChooseTargetVm(SceneManagerVm sceneManager,  ChooseVm chooseVm, Action<Target> callback)
        {
            ParentScene = chooseVm;
            this.sceneManager = sceneManager;
            this.callback = callback;

            chooseVm.Enemies.OnTargetSelected += EnemyClicked;
        }

        private void EnemyClicked(object sender, Target target)
        {
            callback(target);
        }

        public RelayCommand Back
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //Return to previous scene
                    sceneManager.PopScene();
                });
            }
        }

        public RelayCommand MouseDown
        {
            get
            {
                return new RelayCommand(o =>
                {
                    var e = (MouseEventArgs)o;
                    
                    if(e.RightButton == MouseButtonState.Pressed)
                        sceneManager.PopScene();
                });
            }
        }

        public override void Unload()
        {
            ParentScene.Enemies.OnTargetSelected -= EnemyClicked;
        }
    }
}
