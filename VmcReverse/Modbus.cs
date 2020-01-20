using System;
using System.Collections.Generic;
using System.Linq;

namespace VmcReverse
{
    public class Modbus
    {
        public static List<ModbusMessage> ExtractMessages(List<SingleByteData> data)
        {
            var result = new List<ModbusMessage>();

            var request = true;
            for (var i = 0; i < data.Count; i++)
            {
                var msg = new ModbusMessage();
                var startIndex = i;
                msg.SecondsFromStart = data[i].SecondsFromStart;
                msg.Request = request;
                msg.SlaveNumber = data[i++].Value;
                msg.Function = (ModbusFunction) data[i++].Value;
                msg.FillData(data, ref i);
                var endIndex = i;
                var payload = data.GetRange(startIndex, endIndex - startIndex).Select(d => d.Value);
                var foundCrc = data.GetUShort(ref i);
                var computedCrc = Crc16.Calculate(payload);
                if (computedCrc != foundCrc)
                    throw new Exception($"Found CRC different from expected ({computedCrc})");

                request = !request;
                i--;
                result.Add(msg);
            }

            return result;
        }
    }

    public enum ModbusFunction
    {
        ReadCoilStatus = 0x01,
        ReadInputStatus = 0x02,
        ReadHoldingRegisters = 0x03,
        ReadInputRegisters = 0x04,
        ForceSingleCoil = 0x05,
        PresetSingleRegister = 0x06
    }
}