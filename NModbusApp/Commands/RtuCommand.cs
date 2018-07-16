// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RtuCommand.cs" company="DTV-Online">
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
    using System.IO.Ports;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using CommandLine.Core.Hosting.Abstractions;
    using CommandLine.Core.CommandLineUtils;

    #endregion

    /// <summary>
    /// This class implements the RTU command.
    /// </summary>
    [Command(Name = "rtu", Description = "Supporting standard Modbus RTU operations.")]
    [HelpOption("-?|--help")]
    [Subcommand("read", typeof(RtuReadCommand))]
    [Subcommand("write", typeof(RtuWriteCommand))]
    public class RtuCommand : BaseCommand<AppSettings>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RtuCommand"/> class.
        /// Selected properties are initialized with data from the AppSettings instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        public RtuCommand(ILogger<RtuCommand> logger, IHostingEnvironment environment, IConfiguration configuration)
            : base(logger, environment, configuration)
        {
            _logger.LogDebug("RtuCommand()");
            SerialPort = _settings.Master.SerialPort;
            Baudrate = _settings.Master.Baudrate;
            Parity = _settings.Master.Parity;
            DataBits = _settings.Master.DataBits;
            StopBits = _settings.Master.StopBits;
            ReadTimeout = _settings.Master.ReadTimeout;
            WriteTimeout = _settings.Master.WriteTimeout;
            SlaveID = _settings.Slave.SlaveID;
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand Parent { get; set; }

        #endregion

        #region Public Properties

        [Option("--com <string>", "Sets the Modbus master COM port.", CommandOptionType.SingleValue, Inherited = true)]
        public string SerialPort { get; set; } = string.Empty;

        [Option("--baudrate <number>", "Sets the Modbus COM port baud rate.", CommandOptionType.SingleValue, Inherited = true)]
        public int Baudrate { get; set; } = 9600;

        [Option("--parity <string>", "Sets the Modbus COM port parity.", CommandOptionType.SingleValue, Inherited = true)]
        public Parity Parity { get; set; } = Parity.None;

        [Option("--databits <number>", "Sets the Modbus COM port data bits.", CommandOptionType.SingleValue, Inherited = true)]
        public int DataBits { get; set; } = 8;

        [Option("--stopbits <string>", "Sets the Modbus COM port stop bits.", CommandOptionType.SingleValue, Inherited = true)]
        public StopBits StopBits { get; set; } = StopBits.One;

        public int ReadTimeout { get; set; }

        public int WriteTimeout { get; set; }

        [Option("-i|--slaveid <number>", "Sets the Modbus slave ID.", CommandOptionType.SingleValue, Inherited = true)]
        public byte SlaveID { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called processing the tcp command.
        /// </summary>
        /// <returns></returns>
        public Task<int> OnExecuteAsync()
        {
            _logger.LogDebug("OnExecuteAsync()");

            try
            {
                using (SerialPort serialport = new SerialPort(SerialPort,
                                                              Baudrate,
                                                              Parity,
                                                              DataBits,
                                                              StopBits))
                {
                    serialport.Open();

                    if (serialport.IsOpen)
                    {
                        Console.WriteLine($"RTU serial port found at {SerialPort}.");
                        return Task.FromResult(1);
                    }
                    else
                    {
                        Console.WriteLine($"RTU serial port not found at {SerialPort}.");
                        return Task.FromResult(0);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RtuCommand - {ex.Message}");
                return Task.FromResult(-1);
            }
        }

        #endregion
    }
}
