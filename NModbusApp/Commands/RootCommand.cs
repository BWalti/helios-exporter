// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootCommand.cs" company="DTV-Online">
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
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;
    using Newtonsoft.Json;

    using CommandLine.Core.Hosting.Abstractions;
    using CommandLine.Core.CommandLineUtils;

    #endregion

    /// <summary>
    /// This is the root command of the application. Several arguments, options and sub commands are defined using attributes.
    /// </summary>
    [Command(Name = "NModbusApp",
             FullName = "NModbus Application",
             Description = "Allows to read and write Modbus data using Modbus TCP or Modbus RTU.",
             ExtendedHelpText = "\nCopyright (c) 2018 Dr. Peter Trimmel - All rights reserved.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption("-?|--help")]
    [Subcommand("rtu", typeof(RtuCommand))]
    [Subcommand("tcp", typeof(TcpCommand))]
    public class RootCommand : BaseCommand<AppSettings>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RootCommand"/> class.
        /// The RootCommand sets default values for some properties using the application settings.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        public RootCommand(ILogger<RootCommand> logger, IHostingEnvironment environment, IConfiguration configuration)
            : base(logger, environment, configuration)
        {
            _logger.LogDebug("RootCommand()");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The version is determined using the assembly.
        /// </summary>
        /// <returns></returns>
        private static string GetVersion() => Assembly.GetEntryAssembly().GetName().Version.ToString();

        #endregion

        #region Public Properties

        /// <summary>
        /// This is a string option property.
        /// </summary>
        [Option("-s|--settings", "Show settings.", CommandOptionType.NoValue)]
        public bool Show { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called processing root commands.
        /// </summary>
        /// <returns></returns>
        public Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            _logger.LogDebug("OnExecuteAsync()");

            if (Show)
            {
                Console.WriteLine($"Settings: {JsonConvert.SerializeObject(_settings, Formatting.Indented)}");
            }
            else
            {
                app.ShowHelp();
            }

            return Task.FromResult(0);
        }

        #endregion
    }
}
