using Client.Scenes.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Scenes
{
    public class StartScene : Scene
    {
        public override Scene Handle(RequestClient client)
        {
            Console.Clear();
            Console.WriteLine("Dungeon Flunkies!\n");
            Console.WriteLine("a) Living flunky");
            Console.WriteLine("c) Create flunky");

            var key = Console.ReadKey();

            if(key.Key == ConsoleKey.A)
            {
                return new LoginScene();
            }
            else if(key.Key == ConsoleKey.C)
            {
                return new CreateCharacterScene();
            }

            return new StartScene();
        }
    }
}
