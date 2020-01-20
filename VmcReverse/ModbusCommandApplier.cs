using System;
using System.Collections.Generic;

namespace VmcReverse
{
    class ModbusCommandApplier
    {
        public void ApplyCommands(ModbusMemoryMap map, IEnumerable<ModbusMessage> messages)
        {
            ushort requestedAddress = 0;
            foreach (var msg in messages)
            {
                if (msg.Request)
                {
                    requestedAddress = msg.Address;
                    continue;
                }

                switch (msg.Function)
                {
                    case ModbusFunction.ReadCoilStatus:
                        map.SetCoil(requestedAddress, (msg.Value & 1) == 1);
                        break;
                    case ModbusFunction.ReadHoldingRegisters:
                        map.SetHoldingRegister(requestedAddress, msg.Value);
                        break;
                    case ModbusFunction.ForceSingleCoil:
                        bool newVal;
                        if (msg.Value == 0xff00)
                            newVal = true;
                        else if (msg.Value == 0x0000)
                            newVal = false;
                        else
                            throw new Exception($"Invalid value for ForceSingleCoil: {msg.Value}");
                        map.SetCoil(requestedAddress, newVal);
                        break;
                    case ModbusFunction.PresetSingleRegister:
                        map.SetHoldingRegister(requestedAddress, msg.Value);
                        break;
                }
            }
        }
    }
}
