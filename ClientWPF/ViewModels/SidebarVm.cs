using ClientWPF.Scenes.Character;
using ClientWPF.Scenes.StartScreen;
using ClientWPF.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.ViewModels
{
    public class SidebarVm : ViewModel
    {
        private readonly Player player;
        private readonly SceneManagerVm sceneManager;

        public SidebarVm()
        {
        }

        public SidebarVm(Player player, SceneManagerVm sceneManager)
        {
            this.player = player;
            this.sceneManager = sceneManager;

            player.PropertyChanged += (s, e) => Notify("PlayerName");
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
    }
}
