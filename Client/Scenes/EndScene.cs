using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Scenes
{
    public class EndScene : Scene
    {
        public override Scene Handle(RequestClient client)
        {
            Console.WriteLine("Click any button to continue");
            Console.ReadKey();
            return null;
        }
    }
}
