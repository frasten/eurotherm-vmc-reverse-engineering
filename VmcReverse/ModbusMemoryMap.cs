using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VmcReverse
{
    class ModbusMemoryMap
    {
        public Dictionary<ushort, ushort> HoldingRegisters = new Dictionary<ushort, ushort>();
        public Dictionary<ushort, bool> Coils = new Dictionary<ushort, bool>();

        public void SetHoldingRegister(ushort address, ushort value)
        {
            if (!HoldingRegisters.ContainsKey(address))
            {
                Console.WriteLine($"New holding register read: 0x{address:X} = {value}");
                HoldingRegisters.Add(address, value);
            }
            else
            {
                var oldValue = HoldingRegisters[address];
                if (oldValue != value)
                {
                    Console.WriteLine($"Holding register value changed while reading: 0x{address:X} = {oldValue} => {value}");

                }

                HoldingRegisters[address] = value;
            }
        }

        public void SetCoil(ushort address, bool value)
        {
            if (!Coils.ContainsKey(address))
            {
                Console.WriteLine($"New coil read: 0x{address:X} = {value}");
                Coils.Add(address, value);
            }
            else
            {
                var oldValue = Coils[address];
                if (oldValue != value)
                {
                    Console.WriteLine($"Coil value changed while reading: 0x{address:X} = {oldValue} => {value}");

                }

                Coils[address] = value;
            }
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
