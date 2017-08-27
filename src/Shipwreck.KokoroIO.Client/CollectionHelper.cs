using System.Collections.Generic;

namespace Shipwreck.KokoroIO
{
    internal static class CollectionHelper
    {
        public static List<T> Get<T>(ref List<T> field)
            => field ?? (field = new List<T>());

        public static void Set<T>(ref List<T> field, ICollection<T> value)
        {
            if (value != field)
            {
                field?.Clear();
                if (value?.Count > 0)
                {
                    (field ?? (field = new List<T>())).AddRange(value);
                }
            }
        }
    }
}