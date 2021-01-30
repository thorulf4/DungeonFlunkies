using Client.Scenes;
using Client.Scenes.Authentication;
using System;
using System.Net;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            RequestClient client = new RequestClient(IPAddress.Loopback, 5723);

            Scene scene = new StartScene();
            while(scene != null)
            {
                scene = scene.Handle(client);
            }
        }
    }
}
