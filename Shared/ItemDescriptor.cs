﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{

    public class ItemDescriptor : Descriptor
    {
        public ItemDescriptor()
        {
        }

        public ItemDescriptor(int itemId, string name, int count) : base(itemId, name, count)
        {
        }
    }
}
