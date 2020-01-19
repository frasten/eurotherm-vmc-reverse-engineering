using System;
using System.Collections.Generic;
using System.IO;

namespace VmcReverse
{
    public class CsvReader
    {
        public static IEnumerable<byte> ReadCsvData(string csvFileName)
        {
            var data = new List<byte>();
            using (var reader = new StreamReader(csvFileName))
            {
                // Discard header line:
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    var field = values[1]; // Second column
                    var stringData = field.Split('x')[1];
                    var converted = StringToByteArray(stringData)[0];
                    data.Add(converted);
                }
            }

            return data;
        }


        /// <summary>
        ///     https://stackoverflow.com/a/9995303/912216
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            var arr = new byte[hex.Length >> 1];

            for (var i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            var val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}