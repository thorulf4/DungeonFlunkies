using ClientWPF.Utils.Wpf;
using ClientWPF.ViewModels;
using Server.Application.Combat.Skills;
using Shared.Descriptors;
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

        public bool canTargetEnemy { get; set; }
        public bool canTargetAllies { get; set; }

        public ChooseVm ParentScene { get; set; }

        public ChooseTargetVm(SceneManagerVm sceneManager,  ChooseVm chooseVm, SkillDescriptor skill, Action<Target> callback)
        {
            ParentScene = chooseVm;
            this.sceneManager = sceneManager;
            this.callback = callback;

            canTargetEnemy = skill.TargetType == TargetType.Enemies || skill.TargetType == TargetType.All;
            canTargetAllies = skill.TargetType == TargetType.Allies || skill.TargetType == TargetType.All;

            if (canTargetEnemy)
                chooseVm.Enemies.OnTargetSelected += TargetClicked;
            else if (canTargetAllies)
                chooseVm.Allies.OnTargetSelected += TargetClicked;
        }

        private void TargetClicked(object sender, Target target)
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
            if (canTargetEnemy)
                ParentScene.Enemies.OnTargetSelected -= TargetClicked;
            else if (canTargetAllies)
                ParentScene.Allies.OnTargetSelected -= TargetClicked;
        }
    }
}
