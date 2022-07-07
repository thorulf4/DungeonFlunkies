using System.Collections.Generic;
using System.Linq;

namespace Shared.Descriptors
{
    public static class IDescriptableHelpers
    {
        public static IEnumerable<T> GetDescriptors<T>(this IEnumerable<IDescriptable<T>> enumerable)
        {
            return enumerable.Select(u => u.GetDescriptor());
        }
    }
}