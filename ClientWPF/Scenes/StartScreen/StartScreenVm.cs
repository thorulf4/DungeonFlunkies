using ClientWPF.Scenes.RoomScene;
using ClientWPF.Utils.Wpf;
using ClientWPF.ViewModels;
using Shared;
using Shared.Requests.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Scenes.StartScreen
{
    public class StartScreenVm : Scene
    {
        private readonly SceneManagerVm sceneManager;
        private readonly RequestClient client;
        private readonly Player player;

        public StartScreenVm(SceneManagerVm sceneManager, RequestClient client, Player player)
        {
            this.sceneManager = sceneManager;
            this.client = client;
            this.player = player;
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }

        public RelayCommand Login { get
            {
                return new RelayCommand(o =>
                {
                    Response result = client.SendRequest(new CreateCharacterRequest
                    {
                        Name = this.Name,
                        Secret = Password
                    });

                    if (result.Success && result.data is string token)
                    {
                        player.name = Name;
                        player.sessionToken = token;

                        sceneManager.SetScene<RoomVm>();
                    }
                    else
                    {
                        var scene = sceneManager.CreateScene<StartScreenVm>();
                        scene.Name = Name;
                        scene.Password = Password;
                        scene.Message = result.exception.Message;
                        sceneManager.SetScene(scene);
                    }
                });
            }
        }

        public RelayCommand CreateCharacter
        {
            get
            {
                return new RelayCommand(o =>
                {
                    Response result = client.SendRequest(new LoginRequest
                    {
                        Name = this.Name,
                        Secret = Password
                    });

                    if (result.Success && result.data is string token)
                    {
                        player.name = Name;
                        player.sessionToken = token;

                        sceneManager.SetScene<RoomVm>();
                    }
                    else
                    {
                        var scene = sceneManager.CreateScene<StartScreenVm>();
                        scene.Name = Name;
                        scene.Password = Password;
                        scene.Message = result.exception.Message;
                        sceneManager.SetScene(scene);
                    }
                });
            }
        }
    }
}
