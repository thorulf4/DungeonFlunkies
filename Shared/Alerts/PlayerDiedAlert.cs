using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Alerts
{
    public class PlayerDiedAlert : Alert
    {
        public string DeathReason { get; set; }

        public PlayerDiedAlert(string deathReason)
        {
            DeathReason = deathReason;
        }
    }
}
