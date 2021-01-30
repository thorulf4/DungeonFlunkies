using Shared.Requests.Authentication;
using System;
using Shared;
using System.Collections.Generic;
using System.Text;
using Client.Scenes.Rooms;

namespace Client.Scenes.Authentication
{
    class CreateCharacterScene : Scene
    {
        public string Message { get; set; }

        public override Scene Handle(RequestClient client)
        {
            Console.Clear();

            Console.WriteLine("Create character");
            if (Message != null)
                Console.WriteLine(Message);

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Pass: ");
            string password = Console.ReadLine();


            Response result = client.SendRequest(new CreateCharacterRequest
            {
                Name = name,
                Secret = password
            });

            if (result.Success && result.data is string token)
            {
                Player.sessionToken = token;
                Player.name = name;
                Console.WriteLine($"Received session token: {token}");

                return RoomScene.LoadCurrentRoom(client);
            }
            else
            {
                return new CreateCharacterScene { Message = result.exception.Message };
            }
        }
    }
}
