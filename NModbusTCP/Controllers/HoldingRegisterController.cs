// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HoldingRegisterController.cs" company="DTV-Online">
//   Copyright(c) 2017 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace ModbusTCP.Controllers
{
    #region Using Directives

    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Swashbuckle.AspNetCore.Annotations;

    using NModbusTCP.Models;
    using NModbusTCP.Extensions;

    #endregion

    /// <summary>
    /// The Modbus Gateway MVC Controller for reading and writing holding registers.
    /// </summary>
    /// <para>
    ///     Read Multiple Registers         (fc 3)
    ///     Write Single Registers          (fc 6)
    /// </para>

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HoldingRegisterController : Controller
    {
        #region Private Data Members

        private readonly ILogger _logger;
        private readonly AppSettings _settings = new AppSettings();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HoldingRegisterController"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="logger">The application looger.</param>
        public HoldingRegisterController(IConfiguration configuration, ILogger<HoldingRegisterController> logger)
        {
            _logger = logger;
            configuration.GetSection("AppSettings")?.Bind(_settings);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reading a single holding register from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of holding register values.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("{offset}")]
        [SwaggerOperation(Tags = new[] { "Holding & Input Registers" })]
        [ProducesResponseType(typeof(ModbusResponseData<ushort>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadRegisterAsync(ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadHoldingRegisterAsync");
        }

        /// <summary>
        /// Writing a single holding registers to a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data item.</param>
        /// <param name="data">The Modbus data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("{offset}")]
        [SwaggerOperation(Tags = new[] { "Holding & Input Registers" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteRegisterAsync(ushort offset, ushort data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteHoldingRegisterAsync");
        }

        #endregion
    }
}
