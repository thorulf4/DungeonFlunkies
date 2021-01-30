using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests
{
    public class InteractionRequest : AuthenticatedRequest
    {
        public int InteractableId { get; set; }

        public InteractionRequest(int interactableId)
        {
            InteractableId = interactableId;
        }
    }
}
