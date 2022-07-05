using ClientWPF.Scenes.Character;
using ClientWPF.Scenes.Combat;
using ClientWPF.Scenes.StartScreen;
using ClientWPF.Utils.Wpf;
using Shared.Alerts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClientWPF.ViewModels
{
    public class SidebarVm : ViewModel
    {
        private readonly Player player;
        private readonly SceneManagerVm sceneManager;
        private readonly RequestClient client;

        public SidebarVm()
        {
        }

        public SidebarVm(Player player, SceneManagerVm sceneManager, RequestClient client)
        {
            this.player = player;
            this.sceneManager = sceneManager;
            this.client = client;

            player.PropertyChanged += (s, e) => Notify("PlayerName");
            client.SubscribeTo<PlayerDiedAlert>(this, PlayerDied);
        }

        private void PlayerDied(PlayerDiedAlert alert)
        {
            MessageBox.Show($"You died by: {alert.DeathReason}"); //TODO: Refactor this be a full on seperate death screen
            sceneManager.SetScene<StartScreenVm>();
        }

        public string PlayerName { get { return player.Name; } }

        public RelayCommand Inventory { get {
                return new RelayCommand(o =>
                {
                    var inventory = sceneManager.CreateScene<InventoryVm>();
                    sceneManager.PushScene(inventory);
                });
            } }
        
        public RelayCommand Logout
        {
            get
            {
                return new RelayCommand(o =>
                {
                    sceneManager.SetScene<StartScreenVm>();
                });
            }
        }

        public RelayCommand TempCombat
        {
            get
            {
                return new RelayCommand(o =>
                {
                    sceneManager.PushScene<ChooseVm>();
                });
            }
        }
    }
}
