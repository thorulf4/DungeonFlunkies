using Shared.Requests.Rooms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Scenes.RoomScene
{
    class RoomVm : Scene
    {
        public int RoomId { get; set; }
        public string People { get; set; }

        private readonly RequestClient client;

        public RoomVm(RequestClient client)
        {
            this.client = client;
            client.SendRequest(new GetRoomRequest
            {
                Name = 
            })
        }
    }
}
