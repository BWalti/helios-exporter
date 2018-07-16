// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusApp
{
    #region Using Directives

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;

    using Serilog;

    using CommandLine.Core.Hosting;
    using McMaster.Extensions.CommandLineUtils;

    #endregion

    /// <summary>
    /// Class providing the main entry point of the application.
    /// </summary>
    class Program
    {
        #region Entry Point

        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The integer return code.</returns>
        static async Task<int> Main(string[] args)
        {
            // Read the application settings file containing the Serilog configuration.
            var configuration = new ConfigurationBuilder()
                                        .SetBasePath(AppContext.BaseDirectory)
                                        .AddJsonFile("appsettings.json", false, false)
                                        .AddEnvironmentVariables()
                                        .Build();

            // Setting up the static Serilog logger.
            Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(configuration)
                                .CreateLogger();

            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            try
            {
                // Create the application host.
                var app = CommandLineHost.CreateBuilder(args)
                                         .ConfigureAppConfiguration((config) =>
                                         {
                                             config.SetBasePath(AppContext.BaseDirectory);
                                             config.AddJsonFile("appsettings.json", false, false);
                                             config.AddEnvironmentVariables();
                                             config.Build();
                                         })
                                         .ConfigureLogging((logger) =>
                                         {
                                             logger.AddSerilog();
                                         })
                                         .UseStartup<Startup>()
                                         .Build();

                int code = 0;

                // Start the execution.
                Log.ForContext<Program>().Debug($"Executing '{Assembly.GetEntryAssembly().GetName().Name}{(args?.Length > 0 ? " " : "")}{string.Join(" ", args)}'.");
                Stopwatch stopWatch = new Stopwatch();

                // Start the timeer.
                stopWatch.Start();
                code = await app.RunAsync();

                // Display the timing info.
                stopWatch.Stop();
                Log.ForContext<Program>().Debug($"Done.");
                Log.ForContext<Program>().Verbose($"Time elapsed {stopWatch.Elapsed}");
                Console.WriteLine($"Time elapsed {stopWatch.Elapsed}");

                return code;
            }
            catch (CommandParsingException cpx)
            {
                Log.ForContext<Program>().Error(cpx.Message);
                return -1;
            }
            catch (Exception ex)
            {
                Log.ForContext<Program>().Fatal(ex, "Application terminated unexpectedly");
                return -1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        #endregion
    }
}
