// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoilsController.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Controllers
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Swashbuckle.AspNetCore.Annotations;

    using NModbusRTU.Models;
    using NModbusRTU.Extensions;

    #endregion

    /// <summary>
    /// The Modbus Gateway MVC Controller for reading and writing coils.
    /// </summary>
    /// <para>
    ///     Read Coils                      (fc 1)
    ///     Write Multiple Coils            (fc 15)
    /// </para>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoilsController : ControllerBase
    {
        #region Private Data Members

        private readonly ILogger _logger;
        private readonly AppSettings _settings = new AppSettings();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoilsController"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="logger">The application logger.</param>
        public CoilsController(IConfiguration configuration, ILogger<CoilsController> logger)
        {
            _logger = logger;
            configuration.GetSection("AppSettings")?.Bind(_settings);
        }

        #endregion

        /// <summary>
        /// Reading multiple coils from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="number">The number of the Modbus data items.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of coils.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("{offset}")]
        [SwaggerOperation(Tags = new[] { "Coils & Discrete Inputs" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<bool>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadCoilsAsync(int offset, int number, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadCoilsAsync");
        }

        /// <summary>
        /// Writing multiple coils to a Modbus slave.
        /// </summary>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("{offset}")]
        [SwaggerOperation(Tags = new[] { "Coils & Discrete Inputs" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteCoilsAsync([FromBody, Required] ModbusDataValues<bool> data, ushort offset, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, data.Values.Length);
            return await this.ModbusWriteArrayRequest(_logger, request, data.Values, "WriteCoilsAsync");
        }
    }
}

