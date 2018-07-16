// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcpReadCommand.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusApp.Commands
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Globalization;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;
    using NModbus;
    using NModbus.Extensions;

    using CommandLine.Core.Hosting.Abstractions;
    using CommandLine.Core.CommandLineUtils;

    #endregion

    [Command(Name = "read",
             Description = "Supporting Modbus TCP read operations.",
             ExtendedHelpText = "Please specify the read option (coils, discrete inputs, holding registers, or input registers).")]
    [HelpOption("-?|--help")]
    public class TcpReadCommand : BaseCommand<AppSettings>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpReadCommand"/> class.
        /// Selected properties are initialized with data from the AppSettings instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        public TcpReadCommand(ILogger<TcpCommand> logger, IHostingEnvironment environment, IConfiguration configuration)
            : base(logger, environment, configuration)
        {
            _logger.LogDebug("TcpReadCommand()");
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="TcpCommand"/>.
        /// </summary>
        private TcpCommand Parent { get; set; }

        #endregion

        #region Public Properties

        [Option("-c|--coil", "Reads coil(s).", CommandOptionType.NoValue)]
        public bool OptionC { get; set; }

        [Option("-d|--discrete", "Reads discrete input(s).", CommandOptionType.NoValue)]
        public bool OptionD { get; set; }

        [Option("-h|--holding", "Reads holding register(s).", CommandOptionType.NoValue)]
        public bool OptionH { get; set; }

        [Option("-i|--input", "Reads input register(s).", CommandOptionType.NoValue)]
        public bool OptionI { get; set; }

        [Option("-x|--hex", "Displays the values in HEX.", CommandOptionType.NoValue)]
        public bool OptionX { get; set; }

        [Option("-n|--number <number>", "The number of items to read.", CommandOptionType.SingleValue)]
        public ushort Number { get; set; } = 1;

        [Option("-o|--offset <number>", "The offset of the first item to read.", CommandOptionType.SingleValue)]
        public ushort Offset { get; set; }

        [Option("-t|--type <string>", "Reads the specified data type", CommandOptionType.SingleValue)]
        public string Type { get; set; } = string.Empty;

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called processing the tcp read command.
        /// </summary>
        /// <returns></returns>
        public Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            _logger.LogDebug("OnExecuteAsync()");

            if (CheckOptions())
            {
                try
                {
                    TcpClient client = new TcpClient(Parent.Address, Parent.Port);
                    var factory = new ModbusFactory();
                    IModbusMaster modbus = factory.CreateMaster(client);

                    // Reading coils.
                    if (OptionC)
                    {
                        if (Number == 1)
                        {
                            Console.WriteLine($"Reading a single coil[{Offset}]");
                            bool[] values = modbus.ReadCoils(Parent.SlaveID, Offset, Number);
                            Console.WriteLine($"Value of coil[{Offset}] = {values[0]}");
                        }
                        else
                        {
                            Console.WriteLine($"Reading {Number} coils starting at {Offset}");
                            bool[] values = modbus.ReadCoils(Parent.SlaveID, Offset, Number);

                            for (int index = 0; index < values.Length; ++index)
                            {
                                Console.WriteLine($"Value of coil[{index}] = {values[index]}");
                            }
                        }
                    }

                    // Reading discrete inputs.
                    if (OptionD)
                    {
                        if (Number == 1)
                        {
                            Console.WriteLine($"Reading a discrete input[{Offset}]");
                            bool[] values = modbus.ReadInputs(Parent.SlaveID, Offset, Number);
                            Console.WriteLine($"Value of discrete input[{Offset}] = {values[0]}");
                        }
                        else
                        {
                            Console.WriteLine($"Reading {Number} discrete inputs starting at {Offset}");
                            bool[] values = modbus.ReadInputs(Parent.SlaveID, Offset, Number);

                            for (int index = 0; index < values.Length; ++index)
                            {
                                Console.WriteLine($"Value of discrete input[{index}] = {values[index]}");
                            }
                        }
                    }

                    // Reading holding registers.
                    if (OptionH)
                    {
                        if (string.Equals(Type, "string", StringComparison.OrdinalIgnoreCase))
                        {
                            if (OptionX)
                            {
                                Console.WriteLine($"Reading a HEX string from offset = {Offset}");
                                string value = modbus.ReadHexString(Parent.SlaveID, Offset, Number);
                                Console.WriteLine($"Value of HEX string = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading an ASCII string from offset = {Offset}");
                                string value = modbus.ReadString(Parent.SlaveID, Offset, Number);
                                Console.WriteLine($"Value of ASCII string = {value}");
                            }
                        }
                        else if (string.Equals(Type, "bits", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Reading a 16 bit array from offset = {Offset}");
                            BitArray value = modbus.ReadBits(Parent.SlaveID, Offset);
                            Console.WriteLine($"Value of 16 bit array = {value.ToDigitString()}");
                        }
                        else if (string.Equals(Type, "byte", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single byte from offset = {Offset}");
                                byte[] values = modbus.ReadBytes(Parent.SlaveID, Offset, Number);
                                Console.WriteLine($"Value of single byte = {values[0]}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} bytes from offset = {Offset}");
                                byte[] values = modbus.ReadBytes(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of byte array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "short", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single short from offset = {Offset}");
                                short value = modbus.ReadShort(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single short = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} shorts from offset = {Offset}");
                                short[] values = modbus.ReadShortArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of short array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "ushort", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single ushort from offset = {Offset}");
                                ushort value = modbus.ReadUShort(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single ushort = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} ushorts from offset = {Offset}");
                                ushort[] values = modbus.ReadUShortArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of ushort array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "int", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single integer from offset = {Offset}");
                                Int32 value = modbus.ReadInt32(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single integer = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number}  integers from offset = {Offset}");
                                Int32[] values = modbus.ReadInt32Array(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of integer array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "uint", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single unsigned integer from offset = {Offset}");
                                UInt32 value = modbus.ReadUInt32(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single unsigned integer = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} unsigned integers from offset = {Offset}");
                                UInt32[] values = modbus.ReadUInt32Array(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of unsigned integer array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "float", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single float from offset = {Offset}");
                                float value = modbus.ReadFloat(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single float = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} floats from offset = {Offset}");
                                float[] values = modbus.ReadFloatArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of float array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "double", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single double from offset = {Offset}");
                                double value = modbus.ReadDouble(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single double = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} doubles from offset = {Offset}");
                                double[] values = modbus.ReadDoubleArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of double array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "long", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single long from offset = {Offset}");
                                long value = modbus.ReadLong(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single long = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} longs from offset = {Offset}");
                                long[] values = modbus.ReadLongArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of long array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "ulong", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single ulong from offset = {Offset}");
                                ulong value = modbus.ReadULong(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single ulong = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} ulongs from offset = {Offset}");
                                ulong[] values = modbus.ReadULongArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of ulong array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (Number == 1)
                        {
                            Console.WriteLine($"Reading a holding register[{Offset}]");
                            ushort[] values = modbus.ReadHoldingRegisters(Parent.SlaveID, Offset, Number);
                            if (OptionX) Console.WriteLine($"Value of holding register[{Offset}] = {values[0]:X2}");
                            else Console.WriteLine($"Value of holding register[{Offset}] = {values[0]}");
                        }
                        else
                        {
                            Console.WriteLine($"Reading {Number} holding registers starting at {Offset}");
                            ushort[] values = modbus.ReadHoldingRegisters(Parent.SlaveID, Offset, Number);

                            for (int index = 0; index < values.Length; ++index)
                            {
                                if (OptionX) Console.WriteLine($"Value of holding register[{index}] = {values[index]:X2}");
                                else Console.WriteLine($"Value of holding register[{index}] = {values[index]}");
                            }
                        }
                    }

                    // Reading input registers.
                    if (OptionI)
                    {
                        if (string.Equals(Type, "string", StringComparison.OrdinalIgnoreCase))
                        {
                            if (OptionX)
                            {
                                Console.WriteLine($"Reading a HEX string from offset = {Offset}");
                                string value = modbus.ReadOnlyHexString(Parent.SlaveID, Offset, Number);
                                Console.WriteLine($"Value of HEX string = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading an ASCII string from offset = {Offset}");
                                string value = modbus.ReadOnlyString(Parent.SlaveID, Offset, Number);
                                Console.WriteLine($"Value of ASCII string = {value}");
                            }
                        }
                        else if (string.Equals(Type, "bits", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Reading a 16 bit array from offset = {Offset}");
                            BitArray value = modbus.ReadOnlyBits(Parent.SlaveID, Offset);
                            Console.WriteLine($"Value of 16 bit array = {value.ToDigitString()}");
                        }
                        else if (string.Equals(Type, "byte", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single byte from offset = {Offset}");
                                byte[] values = modbus.ReadOnlyBytes(Parent.SlaveID, Offset, Number);
                                Console.WriteLine($"Value of single byte = {values[0]}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} bytes from offset = {Offset}");
                                byte[] values = modbus.ReadOnlyBytes(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of byte array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "short", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single short from offset = {Offset}");
                                short value = modbus.ReadOnlyShort(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single short = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} short values from offset = {Offset}");
                                short[] values = modbus.ReadOnlyShortArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of short array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "ushort", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single ushort from offset = {Offset}");
                                ushort value = modbus.ReadOnlyUShort(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single ushort = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} ushort values from offset = {Offset}");
                                ushort[] values = modbus.ReadOnlyUShortArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of ushort array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "int", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single int from offset = {Offset}");
                                Int32 value = modbus.ReadOnlyInt32(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single integer = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} int values from offset = {Offset}");
                                Int32[] values = modbus.ReadOnlyInt32Array(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of int array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "uint", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single unsigned int from offset = {Offset}");
                                UInt32 value = modbus.ReadOnlyUInt32(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single unsigned int = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} unsigned int values from offset = {Offset}");
                                UInt32[] values = modbus.ReadOnlyUInt32Array(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of unsigned int array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "float", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single float from offset = {Offset}");
                                float value = modbus.ReadOnlyFloat(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single float = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} float values from offset = {Offset}");
                                float[] values = modbus.ReadOnlyFloatArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of float array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "double", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single double from offset = {Offset}");
                                double value = modbus.ReadOnlyDouble(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single double = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} double values from offset = {Offset}");
                                double[] values = modbus.ReadOnlyDoubleArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of double array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "long", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single long from offset = {Offset}");
                                long value = modbus.ReadOnlyLong(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single long = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} long values from offset = {Offset}");
                                long[] values = modbus.ReadOnlyLongArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of long array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else if (string.Equals(Type, "ulong", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a single unsigned long from offset = {Offset}");
                                ulong value = modbus.ReadOnlyULong(Parent.SlaveID, Offset);
                                Console.WriteLine($"Value of single ulong = {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} unsigned long values from offset = {Offset}");
                                ulong[] values = modbus.ReadOnlyULongArray(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    Console.WriteLine($"Value of ulong array[{index}] = {values[index]}");
                                }
                            }
                        }
                        else
                        {
                            if (Number == 1)
                            {
                                Console.WriteLine($"Reading a input register[{Offset}]");
                                ushort[] values = modbus.ReadInputRegisters(Parent.SlaveID, Offset, Number);
                                if (OptionX) Console.WriteLine($"Value of input register[{Offset}] = {values[0]:X2}");
                                else Console.WriteLine($"Value of input register[{Offset}] = {values[0]}");
                            }
                            else
                            {
                                Console.WriteLine($"Reading {Number} input registers starting at {Offset}");
                                ushort[] values = modbus.ReadInputRegisters(Parent.SlaveID, Offset, Number);

                                for (int index = 0; index < values.Length; ++index)
                                {
                                    if (OptionX) Console.WriteLine($"Value of input register[{index}] = {values[index]:X2}");
                                    else Console.WriteLine($"Value of input register[{index}] = {values[index]}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"TcpReadCommand - {ex.Message}");
                }
            }
            else
            {
                app.ShowHint();
            }

            return Task.FromResult(0);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckOptions()
        {
            if (!OptionC && !OptionD && !OptionH && !OptionI)
            {
                Console.WriteLine($"Specify the read option (coils, discrete inputs, holding registers, input registers).");
                return false;
            }

            if ((OptionC && (OptionD || OptionH || OptionI)) ||
                (OptionD && (OptionC || OptionH || OptionI)) ||
                (OptionH && (OptionD || OptionC || OptionI)) ||
                (OptionI && (OptionD || OptionH || OptionC)))
            {
                Console.WriteLine($"Specify only a single read option (coils, discrete inputs, holding registers, input register).");
                return false;
            }

            if ((OptionC || OptionD) && OptionX)
            {
                Console.WriteLine($"HEX output option is ignored.");
            }

            if (Type.Length > 0)
            {
                if (OptionI || OptionH)
                {
                    switch (Type.ToLower(CultureInfo.CurrentCulture))
                    {
                        case "bits":
                            if (Number > 1)
                            {
                                Console.WriteLine($"Only a single bit array value is supported (Number == 1).");
                                Number = 1;
                            }

                            return true;
                        case "string":
                            return true;
                        case "byte":
                        case "short":
                        case "ushort":
                        case "int":
                        case "uint":
                        case "float":
                        case "double":
                        case "long":
                        case "ulong":
                            if (OptionX)
                            {
                                Console.WriteLine($"HEX output option is ignored.");
                            }

                            return true;
                        default:
                            Console.WriteLine($"Unsupported data type '{Type}'.");
                            return false;
                    }
                }
                else
                {
                    Console.WriteLine($"Specified type '{Type}' is ignored.");
                }
            }

            return true;
        }

        #endregion
    }
}
