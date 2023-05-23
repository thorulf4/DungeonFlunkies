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

        public IReadOnlyList<SkillViewModel> Skills { get; set; }
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

            var currentPlayer = alert.Allies.First(p => p.Name == player.Name);
            UpdatePlayer(currentPlayer);

            foreach(var skillVm in Skills)
            {
                skillVm.skill.CurrentCooldown = Math.Max(0, skillVm.skill.CurrentCooldown - 1);
                skillVm.Update(HasAction, HasBonusAction);
            }
            Notify("Skills");
        }

        private async void GetEncounter()
        {
            Debug.Assert(!hasWon);
            Response result = await client.SendRequest(new GetEncounterRequest(), player);

            if (result.Success && result.data is CombatEncounterResponse response)
            {
                if (!response.HasEncounter)
                    return;

                Timer.SetEndTime(response.Encounter.TurnEnds);
                UpdateEncounter(response.Encounter.Enemies, response.Encounter.Allies);

                var currentPlayer = response.Encounter.Allies.First(p => p.Name == player.Name);
                UpdatePlayer(currentPlayer);
                Skills = response.Skills.Select(s => new SkillViewModel(s, HasAction, HasBonusAction)).ToList().AsReadOnly();
                Notify("Skills");
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
        }

        private void UpdatePlayer(EntityDescriptor currentPlayer)
        {
            Debug.Assert(!hasWon);
            HasAction = currentPlayer.HasAction;
            HasBonusAction = currentPlayer.HasBonusAction;

            Notify("HasAction");
            Notify("HasBonusAction");
        }

        public RelayCommand SkillSelected
        {
            get
            {
                return new RelayCommand(skillVm =>
                {
                    SkillDescriptor skillDescriptor = ((SkillViewModel)skillVm).skill;

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
