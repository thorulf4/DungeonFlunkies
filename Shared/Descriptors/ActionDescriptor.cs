using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Descriptors
{
    public class ActionDescriptor
    {
        public int TargetId { get; set; }
        public ActionType Action { get; set; }
        public int Magnitude { get; set; }


        public enum ActionType{
            Attack,
            Debuff,
            Buff
        }
    }
}
