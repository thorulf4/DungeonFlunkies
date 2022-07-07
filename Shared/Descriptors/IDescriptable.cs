using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Descriptors
{
    public interface IDescriptable<T>
    {
        public T GetDescriptor();
    }
}
