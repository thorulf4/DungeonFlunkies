using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests
{
    public class InteractionRequest : AuthenticatedRequest
    {
        public InteractionDescriptor Interaction { get; set; }

        public InteractionRequest(InteractionDescriptor Interaction)
        {
            this.Interaction = Interaction;
        }
    }
}
