using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Alerts
{
    public class RoomUpdateAlert : Alert
    {
        public InteractionDescriptor[] Interactions { get; set; }
        public string[] PeopleInRoom { get; set; }
    }
}
