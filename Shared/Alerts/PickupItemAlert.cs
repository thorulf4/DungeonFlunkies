using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Alerts
{
    public class PickupItemAlert : Alert
    {
        public Descriptor Item { get; set; }
    }
}
