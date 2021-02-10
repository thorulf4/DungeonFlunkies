using ClientWPF.Utils.Wpf;
using ClientWPF.ViewModels;
using Shared;
using Shared.Descriptors;
using Shared.Requests.Combat;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientWPF.Scenes.Combat
{
    public class ChooseVm : Scene
    {
        private readonly RequestClient client;
        private readonly Player player;
        private readonly SceneManagerVm sceneManager;

        public IReadOnlyList<SkillDescriptor> Skills { get; set; }
        public TargetList Enemies { get; set; }

        public ChooseVm(RequestClient client, Player player, SceneManagerVm sceneManager)
        {
            this.client = client;
            this.player = player;
            this.sceneManager = sceneManager;

            Enemies = new TargetList();
            Enemies.Add(new Target
            {
                Name = "Goblin",
                Health = 34,
                MaxHealth = 100
            });

            GetEncounter(client, player);
        }

        private async void GetEncounter(RequestClient client, Player player)
        {
            Response result = await client.SendRequest(new GetEncounterRequest(), player);

            if (result.Success && result.data is CombatEncounterResponse response)
            {
                Skills = response.Skills.AsReadOnly();
            }
            else
            {
                throw new Exception("Wrong response");
            }
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
        

        public RelayCommand SkillSelected
        {
            get
            {
                return new RelayCommand(skill =>
                {
                    SkillDescriptor skillDescriptor = (SkillDescriptor)skill;

                    if(skillDescriptor.CurrentCooldown <= 0)
                        sceneManager.PushScene(new ChooseTargetVm(sceneManager, this, (target) => OnTargetChosen(skillDescriptor, target)));
                });
            }
        }

        private void OnTargetChosen(SkillDescriptor skill, Target target)
        {
            sceneManager.PopScene();
            

        }

        public override void Unload()
        {

        }
    }
}
