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
                return new RelayCommand(async o =>
                {
                    Response result = await client.SendRequest(new LoginRequest
                    {
                        Name = this.Name,
                        Secret = Password
                    });

                    if (result.Success && result.data is string token)
                    {
                        player.Name = Name;
                        player.SessionToken = token;

                        sceneManager.SetScene<RoomVm>();
                    }
                    else if(!result.Success)
                    {
                        var scene = sceneManager.CreateScene<StartScreenVm>();
                        scene.Name = Name;
                        scene.Password = Password;
                        scene.Message = result.exception.Message;
                        sceneManager.SetScene(scene);
                    }
                    else
                    {
                        throw new Exception("Unexpected output");
                    }
                });
            }
        }

        public RelayCommand CreateCharacter
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    Response result = await client.SendRequest(new CreateCharacterRequest
                    {
                        Name = this.Name,
                        Secret = Password
                    });

                    if (result.Success && result.data is string token)
                    {
                        player.Name = Name;
                        player.SessionToken = token;

                        sceneManager.SetScene<RoomVm>();
                    }
                    else if (!result.Success && result.data is RequestFailure exception)
                    {
                        var scene = sceneManager.CreateScene<StartScreenVm>();
                        scene.Name = Name;
                        scene.Password = Password;
                        scene.Message = exception.Message;
                        sceneManager.SetScene(scene);
                    }
                    else
                    {
                        throw new Exception("Unexpected output");
                    }
                });
            }
        }

        public override void Unload()
        {

        }
    }
}
