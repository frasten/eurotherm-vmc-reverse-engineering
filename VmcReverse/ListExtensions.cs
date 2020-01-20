using System.Collections.Generic;

namespace VmcReverse
{
    public static class ListExtensions
    {
        public static ushort GetUShort(this List<SingleByteData> data, ref int i)
        {
            return (ushort)(data[i++].Value << 8 | data[i++].Value);
        }
    }
}
