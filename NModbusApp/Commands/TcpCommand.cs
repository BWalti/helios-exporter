// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcpCommand.cs" company="DTV-Online">
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
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using CommandLine.Core.Hosting.Abstractions;
    using CommandLine.Core.CommandLineUtils;

    #endregion

    /// <summary>
    /// This class implements the TCP command.
    /// </summary>
    [Command(Name = "tcp", Description = "Supporting standard Modbus TCP operations.")]
    [HelpOption("-?|--help")]
    [Subcommand("read", typeof(TcpReadCommand))]
    [Subcommand("write", typeof(TcpWriteCommand))]
    public class TcpCommand : BaseCommand<AppSettings>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpCommand"/> class.
        /// Selected properties are initialized with data from the AppSettings instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        public TcpCommand(ILogger<TcpCommand> logger, IHostingEnvironment environment, IConfiguration configuration)
            : base(logger, environment, configuration)
        {
            _logger.LogDebug("TcpCommand()");
            Address = _settings.Slave.Address;
            Port = _settings.Slave.Port;
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

        [Option("-a|--address <string>", "Sets the Modbus slave IP address.", CommandOptionType.SingleValue, Inherited = true)]
        public string Address { get; set; } = string.Empty;

        [Option("-p|--port <number>", "Sets the Modbus slave port number.", CommandOptionType.SingleValue, Inherited = true)]
        public int Port { get; set; }

        [Option("-i|--slaveid", "Sets the Modbus slave ID.", CommandOptionType.SingleValue, Inherited = true)]
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
                TcpClient client = new TcpClient();
                client.Client.Connect(Address, Port);

                if (client.Connected)
                {
                    Console.WriteLine($"Modbus TCP slave found at {Address}:{Port}.");
                    return Task.FromResult(1);
                }
                else
                {
                    Console.WriteLine($"Modbus TCP slave not found at {Address}:{Port}.");
                    return Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TcpCommand - {ex.Message}");
                return Task.FromResult(-1);
            }
        }

        #endregion
    }
}
