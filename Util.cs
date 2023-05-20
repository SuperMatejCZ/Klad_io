using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io
{
    public static class Util
    {
        public static T KeyByValue<T, W>(this Dictionary<T, W> dict, W val)
        {
            T key = default;
            foreach (KeyValuePair<T, W> pair in dict) {
                if (EqualityComparer<W>.Default.Equals(pair.Value, val)) {
                    key = pair.Key;
                    break;
                }
            }
            return key;
        }

        public static string ToBinary(this byte value, int minimumDigits) => ToBinary((int)value, minimumDigits);
        public static string ToBinary(this int value, int minimumDigits)
        {
            return Convert.ToString(value, 2).PadLeft(minimumDigits, '0');
        }

        public static string ToStringA<T>(this T[] value)
        {
            StringBuilder builder = new StringBuilder("[");
            for (int i = 0; i < value.Length; i++)
                builder = builder.Append(i + ":" + value[i].ToString() + ",");
            builder = builder.Append("]");

            return builder.ToString();
        }
    }
}
