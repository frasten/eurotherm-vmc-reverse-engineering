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

                ModbusMemoryMap.MapStatus mapStatus;
                object oldValue;
                object newValue = msg.Value;
                
                switch (msg.Function)
                {
                    case ModbusFunction.ReadCoilStatus:
                        newValue = (msg.Value & 1) == 1;
                        (mapStatus, oldValue) = map.SetCoil(requestedAddress, (bool) newValue);
                        break;
                    case ModbusFunction.ReadHoldingRegisters:
                        (mapStatus, oldValue) = map.SetHoldingRegister(requestedAddress, (ushort) newValue);
                        break;
                    case ModbusFunction.ForceSingleCoil:
                        if ((ushort)newValue == 0xff00)
                            newValue = true;
                        else if ((ushort)newValue == 0x0000)
                            newValue = false;
                        else
                            throw new Exception($"Invalid value for ForceSingleCoil: {newValue}");
                        (mapStatus, oldValue) = map.SetCoil(requestedAddress, (bool) newValue);
                        break;
                    case ModbusFunction.PresetSingleRegister:
                        (mapStatus, oldValue) = map.SetHoldingRegister(requestedAddress, (ushort) newValue);
                        break;
                    default:
                        throw new Exception("Not yet.");
                }

                if (mapStatus == ModbusMemoryMap.MapStatus.NotChanged)
                    continue;
                var time = $"{msg.SecondsFromStart:000.000}";
                var statusLabel = GetStatusString(mapStatus);
                var writeLabel = IsWriting(msg.Function) ? "WRIT" : "READ";
                var register = IsCoil(msg.Function) ? "COIL" : "HOLD";

                var valueString = GetValueString(newValue, oldValue, mapStatus);

                Console.WriteLine($"[{time}] [{statusLabel}] [{register}] [{writeLabel}] ADDR: 0x{requestedAddress:X2} = {valueString}");
            }
        }

        private object GetValueString(object newValue, object oldValue, ModbusMemoryMap.MapStatus status)
        {
            if (status == ModbusMemoryMap.MapStatus.RecordChanged)
                return $"{oldValue} => {newValue}";
            return $"{newValue}";
        }

        private bool IsWriting(ModbusFunction function)
        {
            if (function == ModbusFunction.ForceSingleCoil ||
                function == ModbusFunction.PresetSingleRegister)
                return true;
            return false;
        }

        private bool IsCoil(ModbusFunction function)
        {
            if (function == ModbusFunction.ReadCoilStatus ||
                function == ModbusFunction.ForceSingleCoil)
                return true;
            return false;
        }

        private string GetStatusString(ModbusMemoryMap.MapStatus status)
        {
            switch (status)
            {
                case ModbusMemoryMap.MapStatus.NewRecord:
                    return "NEW";
                case ModbusMemoryMap.MapStatus.RecordChanged:
                    return "CHG";
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}
