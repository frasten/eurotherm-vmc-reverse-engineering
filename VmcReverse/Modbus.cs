using System;
using System.Collections.Generic;

namespace VmcReverse
{
    public class Modbus
    {
        public static List<ModbusMessage> ExtractMessages(List<byte> data)
        {
            var result = new List<ModbusMessage>();

            var request = true;
            for (var i = 0; i < data.Count; i++)
            {
                var msg = new ModbusMessage();
                var startIndex = i;
                msg.Request = request;
                msg.SlaveNumber = data[i++];
                msg.Function = (ModbusFunction) data[i++];
                msg.FillData(data, ref i);
                var endIndex = i;
                var payload = data.GetRange(startIndex, endIndex - startIndex);
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