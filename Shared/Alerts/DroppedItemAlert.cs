using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Alerts
{
    public class DroppedItemAlert : Alert
    {
        public Descriptor DroppedItem { get; set; }
    }
}
