using Client.Scenes.Rooms;
using Shared;
using Shared.Requests.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Scenes.Authentication
{
    public class LoginScene : Scene
    {
        public string message { get; set; }

        public override Scene Handle(RequestClient client)
        {
            Console.Clear();

            Console.WriteLine("Login");
            if (message != null)
                Console.WriteLine(message);

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Pass: ");
            string password = Console.ReadLine();


            Response result = client.SendRequest(new LoginRequest
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
                return new LoginScene { message = result.exception.Message };
            }
        }
    }
}
