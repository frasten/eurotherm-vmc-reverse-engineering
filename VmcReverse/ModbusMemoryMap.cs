using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VmcReverse
{
    class ModbusMemoryMap
    {
        public enum MapStatus
        {
            NotChanged,
            NewRecord,
            RecordChanged
        }

        public Dictionary<ushort, ushort> HoldingRegisters = new Dictionary<ushort, ushort>();
        public Dictionary<ushort, bool> Coils = new Dictionary<ushort, bool>();

        public (MapStatus, object) SetHoldingRegister(ushort address, ushort value)
        {
            if (!HoldingRegisters.ContainsKey(address))
            {
                HoldingRegisters.Add(address, value);
                return (MapStatus.NewRecord, null);
            }

            var oldValue = HoldingRegisters[address];
            if (oldValue != value)
            {
                HoldingRegisters[address] = value;
                return (MapStatus.RecordChanged, oldValue);
            }

            return (MapStatus.NotChanged, null);
        }

        public (MapStatus, object) SetCoil(ushort address, bool value)
        {
            if (!Coils.ContainsKey(address))
            {
                Coils.Add(address, value);
                return (MapStatus.NewRecord, null);
            }

            var oldValue = Coils[address];
            if (oldValue != value)
            {
                Coils[address] = value;
                return (MapStatus.RecordChanged, oldValue);
            }

            return (MapStatus.NotChanged, null);
        }

        public void Print()
        {
            Console.WriteLine(GetString());
        }

        private string GetString()
        {
            var sb = new StringBuilder();

            sb.Append(GetHoldingRegistersString());
            sb.Append(GetCoilsString());

            return sb.ToString();
        }

        private string GetCoilsString()
        {
            var sb = new StringBuilder();
            return sb.ToString();
        }

        private string GetHoldingRegistersString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("*".Repeat(22));
            var keys = HoldingRegisters.Keys.ToList();
            keys.Sort();

            foreach (var addr in keys)
            {
                var value = HoldingRegisters[addr];
                var paddedValue = $"{value}".PadLeft(6);
                sb.Append($"0x{addr:X4}  0x{value:X4}  {paddedValue}\n");
            }

            sb.AppendLine("*".Repeat(22));
            return sb.ToString();
        }
    }
}
