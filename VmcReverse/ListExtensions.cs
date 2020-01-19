using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VmcReverse
{
    public static class ListExtensions
    {
        public static ushort GetUShort(this List<byte> data, ref int i)
        {
            return (ushort)(data[i++] << 8 | data[i++]);
        }
    }
}
