// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RtuWriteCommand.cs" company="DTV-Online">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO.Ports;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;
    using Newtonsoft.Json;
    using NModbus;
    using NModbus.Serial;
    using NModbus.Extensions;

    using CommandLine.Core.Hosting.Abstractions;
    using CommandLine.Core.CommandLineUtils;

    #endregion

    [Command(Name = "write", Description = "Supporting Modbus RTU write operations.")]
    [HelpOption("-?|--help")]
    public class RtuWriteCommand : BaseCommand<AppSettings>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RtuWriteCommand"/> class.
        /// Selected properties are initialized with data from the AppSettings instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        public RtuWriteCommand(ILogger<TcpCommand> logger, IHostingEnvironment environment, IConfiguration configuration)
            : base(logger, environment, configuration)
        {
            _logger.LogDebug("TcpReadCommand()");
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RtuCommand"/>.
        /// </summary>
        private RtuCommand Parent { get; set; }

        #endregion

        #region Public Properties

        [Argument(0, "Data values (JSON format).")]
        public string Values { get; set; } = "[]";

        [Option("-c|--coil", "Writes coil(s).", CommandOptionType.NoValue)]
        public bool OptionC { get; set; }

        [Option("-h|--holding", "Writes holding register(s).", CommandOptionType.NoValue)]
        public bool OptionH { get; set; }

        [Option("-o|--offset <number>", "The offset of the first item to write.", CommandOptionType.SingleValue)]
        public ushort Offset { get; set; }

        [Option("-t|--type <string>", "Writes the specified data type", CommandOptionType.SingleValue)]
        public string Type { get; set; } = string.Empty;

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called processing the tcp write command.
        /// </summary>
        /// <returns></returns>
        public Task<int> OnExecuteAsync()
        {
            _logger.LogDebug("OnExecuteAsync()");

            if (CheckOptions())
            {
                try
                {
                    using (SerialPort serialport = new SerialPort(Parent.SerialPort,
                                                                  Parent.Baudrate,
                                                                  Parent.Parity,
                                                                  Parent.DataBits,
                                                                  Parent.StopBits))
                    {
                        serialport.Open();

                        if (serialport.IsOpen)
                        {
                            var adapter = new SerialPortAdapter(serialport);
                            var factory = new ModbusFactory();
                            IModbusMaster modbus = factory.CreateRtuMaster(adapter);
                            modbus.Transport.SlaveBusyUsesRetryCount = true;
                            modbus.Transport.ReadTimeout = Parent.ReadTimeout;
                            modbus.Transport.WriteTimeout = Parent.WriteTimeout;

                            // Writing coils.
                            if (OptionC)
                            {
                                List<bool> values = JsonConvert.DeserializeObject<List<bool>>(Values);

                                if (values.Count == 0)
                                {
                                    _logger.LogWarning($"No values specified.");
                                }
                                else
                                {
                                    if (values.Count == 1)
                                    {
                                        Console.WriteLine($"Write single coil[{Offset}] = {values[0]}");
                                        modbus.WriteSingleCoil(Parent.SlaveID, Offset, values[0]);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Writing {values.Count} coils starting at {Offset}");

                                        for (int index = 0; index < values.Count; ++index)
                                            Console.WriteLine($"Value of coil[{Offset + index}] = {values[index]}");

                                        modbus.WriteMultipleCoils(Parent.SlaveID, Offset, values.ToArray());
                                    }
                                }
                            }

                            // Writing holding registers.
                            if (OptionH)
                            {
                                if ((Type.Length > 0) && string.IsNullOrEmpty(Values))
                                {
                                    _logger.LogWarning($"No values of type '{Type}' specified.");
                                }

                                if (string.Equals(Type, "string", StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine($"Writing an ASCII string at offset = {Offset}");
                                    modbus.WriteString(Parent.SlaveID, Offset, Values);
                                }
                                else if (string.Equals(Type, "bits", StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine($"Writing a 16 bit array at offset = {Offset}");
                                    modbus.WriteBits(Parent.SlaveID, Offset, Values.ToBitArray());
                                }
                                else if (string.Equals(Type, "byte", StringComparison.OrdinalIgnoreCase))
                                {
                                    List<byte> bytes = JsonConvert.DeserializeObject<List<byte>>(Values);
                                    Console.WriteLine($"Writing {bytes.Count} bytes at offset = {Offset}");
                                    modbus.WriteBytes(Parent.SlaveID, Offset, bytes.ToArray());
                                }
                                else if (string.Equals(Type, "short", StringComparison.OrdinalIgnoreCase))
                                {
                                    List<short> values = JsonConvert.DeserializeObject<List<short>>(Values);

                                    if (values.Count == 1)
                                    {
                                        Console.WriteLine($"Writing a single short value at offset = {Offset}");
                                        modbus.WriteShort(Parent.SlaveID, Offset, values[0]);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Writing {values.Count} short values at offset = {Offset}");
                                        modbus.WriteShortArray(Parent.SlaveID, Offset, values.ToArray());
                                    }
                                }
                                else if (string.Equals(Type, "ushort", StringComparison.OrdinalIgnoreCase))
                                {
                                    List<ushort> values = JsonConvert.DeserializeObject<List<ushort>>(Values);

                                    if (values.Count == 1)
                                    {
                                        Console.WriteLine($"Writing a single unsigned short value at offset = {Offset}");
                                        modbus.WriteUShort(Parent.SlaveID, Offset, values[0]);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Writing {values.Count} unsigned short values at offset = {Offset}");
                                        modbus.WriteUShortArray(Parent.SlaveID, Offset, values.ToArray());
                                    }
                                }
                                else if (string.Equals(Type, "int", StringComparison.OrdinalIgnoreCase))
                                {
                                    List<int> values = JsonConvert.DeserializeObject<List<int>>(Values);

                                    if (values.Count == 1)
                                    {
                                        Console.WriteLine($"Writing a single int value at offset = {Offset}");
                                        modbus.WriteInt32(Parent.SlaveID, Offset, values[0]);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Writing {values.Count} int values at offset = {Offset}");
                                        modbus.WriteInt32Array(Parent.SlaveID, Offset, values.ToArray());
                                    }
                                }
                                else if (string.Equals(Type, "uint", StringComparison.OrdinalIgnoreCase))
                                {
                                    List<uint> values = JsonConvert.DeserializeObject<List<uint>>(Values);

                                    if (values.Count == 1)
                                    {
                                        Console.WriteLine($"Writing a single unsigned int value at offset = {Offset}");
                                        modbus.WriteUInt32(Parent.SlaveID, Offset, values[0]);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Writing {values.Count} unsigned int values at offset = {Offset}");
                                        modbus.WriteUInt32Array(Parent.SlaveID, Offset, values.ToArray());
                                    }
                                }
                                else if (string.Equals(Type, "float", StringComparison.OrdinalIgnoreCase))
                                {
                                    List<float> values = JsonConvert.DeserializeObject<List<float>>(Values);

                                    if (values.Count == 1)
                                    {
                                        Console.WriteLine($"Writing a single float value at offset = {Offset}");
                                        modbus.WriteFloat(Parent.SlaveID, Offset, values[0]);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Writing {values.Count} float values at offset = {Offset}");
                                        modbus.WriteFloatArray(Parent.SlaveID, Offset, values.ToArray());
                                    }
                                }
                                else if (string.Equals(Type, "double", StringComparison.OrdinalIgnoreCase))
                                {
                                    List<double> values = JsonConvert.DeserializeObject<List<double>>(Values);

                                    if (values.Count == 1)
                                    {
                                        Console.WriteLine($"Writing a single double value at offset = {Offset}");
                                        modbus.WriteDouble(Parent.SlaveID, Offset, values[0]);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Writing {values.Count} double values at offset = {Offset}");
                                        modbus.WriteDoubleArray(Parent.SlaveID, Offset, values.ToArray());
                                    }
                                }
                                else if (string.Equals(Type, "long", StringComparison.OrdinalIgnoreCase))
                                {
                                    List<long> values = JsonConvert.DeserializeObject<List<long>>(Values);

                                    if (values.Count == 1)
                                    {
                                        Console.WriteLine($"Writing a single long value at offset = {Offset}");
                                        modbus.WriteLong(Parent.SlaveID, Offset, values[0]);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Writing {values.Count} long values at offset = {Offset}");
                                        modbus.WriteLongArray(Parent.SlaveID, Offset, values.ToArray());
                                    }
                                }
                                else if (string.Equals(Type, "ulong", StringComparison.OrdinalIgnoreCase))
                                {
                                    List<ulong> values = JsonConvert.DeserializeObject<List<ulong>>(Values);

                                    if (values.Count == 1)
                                    {
                                        Console.WriteLine($"Writing a single unsigned long value at offset = {Offset}");
                                        modbus.WriteULong(Parent.SlaveID, Offset, values[0]);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Writing {values.Count} unsigned long values at offset = {Offset}");
                                        modbus.WriteULongArray(Parent.SlaveID, Offset, values.ToArray());
                                    }
                                }
                                else
                                {
                                    List<ushort> values = JsonConvert.DeserializeObject<List<ushort>>(Values);

                                    if (values.Count == 0)
                                    {
                                        _logger.LogWarning($"No values specified.");
                                    }
                                    else
                                    {
                                        if (values.Count == 1)
                                        {
                                            Console.WriteLine($"Writing single holding register[{Offset}] = {values[0]}");
                                            modbus.WriteSingleRegister(Parent.SlaveID, Offset, values[0]);
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Writing {values.Count} holding registers starting at {Offset}");

                                            for (int index = 0; index < values.Count; ++index)
                                                Console.WriteLine($"Value of holding register[{Offset + index}] = {values[index]}");

                                            modbus.WriteMultipleRegisters(Parent.SlaveID, Offset, values.ToArray());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (JsonSerializationException jsx)
                {
                    _logger.LogError(jsx, $"Exception parsing data values.");
                }
                catch (JsonReaderException jrx)
                {
                    _logger.LogError(jrx, $"Exception writing data values.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"RtuWriteCommand - {ex.Message}");
                }
            }

            return Task.FromResult(0);
        }

        #endregion

        #region Private Methods

        private bool CheckOptions()
        {
            if (OptionC && OptionH)
            {
                Console.WriteLine($"Specify only a single write option (coils or holding registers).");
                return false;
            }

            if (Type.Length > 0)
            {
                if (OptionH)
                {
                    switch (Type.ToLower(CultureInfo.CurrentCulture))
                    {
                        case "bits":
                        case "string":
                        case "byte":
                        case "short":
                        case "ushort":
                        case "int":
                        case "uint":
                        case "float":
                        case "double":
                        case "long":
                        case "ulong":
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
