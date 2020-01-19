using System;
using System.Collections.Generic;

namespace VmcReverse
{
    public class ModbusMessage
    {
        public byte SlaveNumber { get; set; }

        /// <summary>
        ///     If true, it's a request from the Master (control panel) to the Slave (VMC).
        ///     If false, it's a response from Slave (VMC) to the Master (control panel).
        /// </summary>
        public bool Request { get; set; }

        public ModbusFunction Function { get; set; }

        public ushort Address { get; set; }

        public ushort Value { get; set; }


        public void FillData(List<byte> data, ref int i)
        {
            switch (Function)
            {
                case ModbusFunction.ReadCoilStatus:
                    if (Request)
                    {
                        Address = data.GetUShort(ref i);
                        var numRequests = data.GetUShort(ref i);
                        if (numRequests != 1)
                            throw new NotImplementedException("Not yet.");
                        // TODO
                    }
                    else
                    {
                        var numBytesToRead = data[i++];
                        if (numBytesToRead != 1)
                            throw new NotImplementedException("Not yet.");
                        for (var j = 0; j < numBytesToRead; j++)
                        {
                            Value = (ushort) data[i++];
                        }
                    }
                    break;
                case ModbusFunction.ReadHoldingRegisters:
                    if (Request)
                    {
                        Address = data.GetUShort(ref i);
                        var numRequests = data.GetUShort(ref i);
                        if (numRequests != 1)
                            throw new NotImplementedException("Not yet.");
                        // TODO
                    }
                    else
                    {
                        var numBytesToRead = data[i++];
                        if (numBytesToRead != 2)
                            throw new NotImplementedException("Not yet.");
                        Value = data.GetUShort(ref i);
                    }
                    break;
                default:
                    throw new NotImplementedException("Not yet.");
            }
        }
    }
}