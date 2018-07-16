// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCommand.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace CommandLine.Core.CommandLineUtils
{
    #region Using Directives

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using CommandLine.Core.Hosting.Abstractions;

    #endregion

    /// <summary>
    /// Base class for application commands setting common environment, configuration and logger data members.
    /// </summary>
    public class BaseCommand
    {
        #region Protected Data Members

        protected readonly IHostingEnvironment _environment;
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<BaseCommand> _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class using dependency injection.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        public BaseCommand(ILogger<BaseCommand> logger, IHostingEnvironment environment, IConfiguration configuration)
        {
            _logger = logger;
            _environment = environment;
            _configuration = configuration;
            _logger.LogDebug("BaseCommand()");
        }

        #endregion
    }

    /// <summary>
    /// Generic base class for application commands setting common environment, configuration and logger data members.
    /// This class allows to specify an application setting class type to instanciate the settings data member.
    /// <typeparamref name="TSettings"/>
    /// </summary>
    public class BaseCommand<TSettings> where TSettings : class
    {
        #region Protected Data Members

        protected readonly IHostingEnvironment _environment;
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<BaseCommand<TSettings>> _logger;
        protected readonly TSettings _settings;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand<T>"/> class using dependency injection.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        public BaseCommand(ILogger<BaseCommand<TSettings>> logger, IHostingEnvironment environment, IConfiguration configuration)
        {
            _logger = logger;
            _environment = environment;
            _configuration = configuration;
            _settings = _configuration.GetSection("AppSettings").Get<TSettings>();
            _logger.LogDebug("BaseCommand()");
        }

        #endregion
    }
}
