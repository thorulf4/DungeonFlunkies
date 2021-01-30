using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public abstract class Scene
    {
        public abstract Scene Handle(RequestClient client);
    }
}
