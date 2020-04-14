using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AI_Research_1.Serialization
{
    public static class Serializer
    {
        public static string Serialize(object o)
        {
            if (o is IValuesList list)
                return SerializeList(list.GetValuesList());

            if (o is IEnumerable enumerable)
                return SerializeList(enumerable.Cast<object>());

            return o.ToString();
        }

        private static string SerializeList(IEnumerable<object> list) => 
            $"[{string.Join(",", list.Select(Serialize))}]";
    }
}