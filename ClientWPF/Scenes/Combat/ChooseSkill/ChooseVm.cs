using ClientWPF.Utils.Wpf;
using ClientWPF.ViewModels;
using Shared;
using Shared.Alerts.Combat;
using Shared.Descriptors;
using Shared.Requests.Combat;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ClientWPF.Scenes.Combat
{
    public class ChooseVm : Scene
    {
        private readonly RequestClient client;
        private readonly Player player;
        private readonly SceneManagerVm sceneManager;

        public IReadOnlyList<SkillDescriptor> Skills { get; set; }
        public TargetList Enemies { get; set; }
        public TargetList Allies { get; set; }

        public TurnTimer Timer { get; set; }

        public bool HasAction { get; set; }
        public bool HasBonusAction { get; set; }

        public ChooseVm(RequestClient client, Player player, SceneManagerVm sceneManager)
        {
            this.client = client;
            this.player = player;
            this.sceneManager = sceneManager;

            Enemies = new TargetList();
            Allies = new TargetList();

            Timer = new TurnTimer(GameSettings.turnTimeInMs);

            client.SubscribeTo<NewTurnAlert>(this, NewTurn);
            client.SubscribeTo<CombatUpdateAlert>(this, CombatUpdate);
            client.SubscribeTo<WonCombatAlert>(this, WonCombat);

            GetEncounter();
        }

        bool hasWon = false;
        private void WonCombat(WonCombatAlert alert)
        {
            hasWon = true;

            sceneManager.PopWithChildren(this);
            MessageBox.Show("You won the fight");
        }

        private void CombatUpdate(CombatUpdateAlert alert)
        {
            Debug.Assert(!hasWon);
            UpdateEncounter(alert.Enemies, alert.Allies);

            var currentPlayer = alert.Allies.First(p => p.Name == player.Name);
            UpdatePlayer(currentPlayer);
        }

        private void NewTurn(NewTurnAlert alert)
        {
            Debug.Assert(!hasWon);
            UpdateEncounter(alert.Enemies, alert.Allies);
            Timer.StartTurn(0);

            foreach(var skill in Skills)
            {
                skill.CurrentCooldown = Math.Max(0, skill.CurrentCooldown - 1);
            }

            var currentPlayer = alert.Allies.First(p => p.Name == player.Name);
            UpdatePlayer(currentPlayer);
        }

        private async void GetEncounter()
        {
            Debug.Assert(!hasWon);
            Response result = await client.SendRequest(new GetEncounterRequest(), player);

            if (result.Success && result.data is CombatEncounterResponse response)
            {
                if (!response.HasEncounter)
                    return;

                Skills = response.Skills.AsReadOnly();
                Timer.SetEndTime(response.Encounter.TurnEnds);
                UpdateEncounter(response.Encounter.Enemies, response.Encounter.Allies);

                var currentPlayer = response.Encounter.Allies.First(p => p.Name == player.Name);
                UpdatePlayer(currentPlayer);
            }
            else
            {
                throw new Exception("Wrong response");
            }
        }

        private void UpdateEncounter(List<EntityDescriptor> enemies, List<EntityDescriptor> allies)
        {
            Debug.Assert(!hasWon);

            Enemies = new TargetList();
            Enemies.AddRange(enemies.Select(e => new Target()
            {
                Health = e.Health,
                MaxHealth = e.MaxHealth,
                Name = e.Name,
                Id = e.Id,
                Action = e.Action,
            }).ToList());

            Allies = new TargetList();
            Allies.AddRange(allies.Select(e => new Target()
            {
                Health = e.Health,
                MaxHealth = e.MaxHealth,
                Name = e.Name,
                Id = e.Id,
            }).ToList());

            Notify("Enemies");
            Notify("Allies");
            Notify("Skills");
        }

        private void UpdatePlayer(EntityDescriptor currentPlayer)
        {
            Debug.Assert(!hasWon);
            HasAction = currentPlayer.HasAction;
            HasBonusAction = currentPlayer.HasBonusAction;

            Notify("HasAction");
            Notify("HasBonusAction");
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

        public RelayCommand EndTurn
        {
            get
            {
                return new RelayCommand(o =>
                {
                    if(HasAction || HasBonusAction)
                    {
                        client.SendAction(new EndTurnRequest(), player);

                        HasAction = false;
                        HasBonusAction = false;
                    }
                });
            }
        }

        private void OnTargetChosen(SkillDescriptor skill, Target target)
        {
            sceneManager.PopScene();

            client.SendAction(new UseSkillRequest(skill, target.Id), player);
            GetEncounter();
        }

        public override void Unload()
        {
            client.UnsubscribeAll(this);
            Timer.Dispose();
        }
    }
}
