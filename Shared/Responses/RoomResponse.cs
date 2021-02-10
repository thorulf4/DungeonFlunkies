using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Responses
{
    public class RoomResponse
    {
        public int RoomId { get; set; }
        public InteractionDescriptor[] Interactions { get; set; }
        public string[] PeopleInRoom { get; set; }
    }
}
