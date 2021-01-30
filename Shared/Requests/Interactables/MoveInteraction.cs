using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests.Interactables
{
    public class MoveInteraction : InteractionRequest
    {
        public MoveInteraction(int interactableId) : base(interactableId)
        {
        }
    }
}
