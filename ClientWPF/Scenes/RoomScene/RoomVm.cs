using ClientWPF.Utils.Wpf;
using ClientWPF.ViewModels;
using Shared.Alerts;
using Shared.Model.Interactables;
using Shared.Requests;
using Shared.Requests.Rooms;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientWPF.Scenes.RoomScene
{
    class RoomVm : Scene
    {
        public int RoomId { get; set; }
        public string People { get; set; }
        public IReadOnlyList<InteractionDescriptor> Interactions { get; set; }


        private readonly RequestClient client;
        private readonly Player player;
        private readonly SceneManagerVm sceneManager;

        public RoomVm(SceneManagerVm sceneManager, RequestClient client, Player player)
        {
            this.sceneManager = sceneManager;
            this.client = client;
            this.player = player;

            client.SubscribeTo<RoomAlert>(this, OnRoomUpdate);

            var result = client.SendRequest(new GetRoomRequest(), player);
            if (result.Success && result.data is RoomResponse room)
            {
                RoomId = room.RoomId;
                People = room.PeopleInRoom.Aggregate((a, b) => $"{a}, {b}");
                Interactions = room.Interactions.ToList().AsReadOnly();
            }
            else
            {
                throw new Exception("Failed");
            }
        }

        private void OnRoomUpdate(RoomAlert alert)
        {
            People = alert.PeopleInRoom.Aggregate((a, b) => $"{a}, {b}");
            Interactions = alert.Interactions.ToList().AsReadOnly();
            
            Notify("People");
            Notify("Interactions");
        }

        public override void Unload()
        {
            client.Unsubscribe(this);
        }

        public RelayCommand Interact { get
            {
                return new RelayCommand(o =>
                {
                    InteractionDescriptor interaction = (InteractionDescriptor)o;
                    var result = client.SendRequest(new InteractionRequest(interaction.Id), player);
                    sceneManager.LookupScene(result);
                });
            }
        }
    }
}
 