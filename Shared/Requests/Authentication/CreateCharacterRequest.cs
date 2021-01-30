using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests.Authentication
{
    public class CreateCharacterRequest
    {
        public string Name { get; set; }
        public string Secret { get; set; }
    }
}
