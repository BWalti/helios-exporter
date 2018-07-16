// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ROSingleController.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Controllers
{
    #region Using Directives

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
    /// The Modbus Gateway MVC Controller for reading various data values.
    /// </summary>
    /// <para>
    ///     ReadOnlyString      Reads an ASCII string (multiple input registers)
    ///     ReadOnlyHexString   Reads an HEX string (multiple input registers)
    ///     ReadOnlyBool        Reads a boolean value (single discrete input)
    ///     ReadOnlyBits        Reads a 16-bit bit array value (single input register)
    ///     ReadOnlyShort       Reads a 16 bit integer (single input register)
    ///     ReadOnlyUShort      Reads an unsigned 16 bit integer (single input register)
    ///     ReadOnlyInt32       Reads a 32 bit integer (two input registers)
    ///     ReadOnlyUInt32      Reads an unsigned 32 bit integer (two input registers)
    ///     ReadOnlyFloat       Reads a 32 bit IEEE floating point number (two input registers)
    ///     ReadOnlyDouble      Reads a 64 bit IEEE floating point number (four input registers)
    ///     ReadOnlyLong        Reads a 64 bit integer (four input registers)
    ///     ReadOnlyULong       Reads an unsigned 64 bit integer (four input registers)
    /// </para>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ROSingleController : ControllerBase
    {
        #region Private Data Members

        private readonly ILogger _logger;
        private readonly AppSettings _settings = new AppSettings();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ROSingleController"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="logger">The application logger.</param>
        public ROSingleController(IConfiguration configuration, ILogger<ROSingleController> logger)
        {
            _logger = logger;
            configuration.GetSection("AppSettings")?.Bind(_settings);
        }

        #endregion

        /// <summary>
        /// Reads an ASCII string from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="size">The size of the string.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the string data values.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("String/{offset}")]
        [SwaggerOperation(Tags = new[] { "String Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseStringData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyStringAsync(ushort offset, ushort size, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, size);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyStringAsync");
        }

        /// <summary>
        /// Reads an HEX string from a Modbus slave.
        /// </summary>
        /// <remarks>Note that the resulting HEX string length is twice the number parameter
        /// and reads multiple input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of 2-bytes HEX substrings in the string.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the string data values.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("HexString/{offset}")]
        [SwaggerOperation(Tags = new[] { "HEX String Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseStringData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyHexStringAsync(ushort offset, ushort number, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyHexStringAsync");
        }

        /// <summary>
        /// Reads a boolean value from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this is equivalent to read a single discrete input.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the boolean data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Bool/{offset}")]
        [SwaggerOperation(Tags = new[] { "Boolean Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<bool>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyBoolAsync(ushort offset, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyBoolAsync");
        }

        /// <summary>
        /// Reads a 16-bit bit array value from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads a single input register.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the boolean data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Bits/{offset}")]
        [SwaggerOperation(Tags = new[] { "Bits Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseStringData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyBitsAsync(ushort offset, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyBitsAsync");
        }

        /// <summary>
        /// Reads a 16 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads a single input register.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the short data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Short/{offset}")]
        [SwaggerOperation(Tags = new[] { "Short Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<short>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyShortAsync(ushort offset, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyShortAsync");
        }

        /// <summary>
        /// Reads an unsigned 16 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads a single input register.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the ushort data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("UShort/{offset}")]
        [SwaggerOperation(Tags = new[] { "UShort Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<ushort>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyUShortAsync(ushort offset, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyUShortAsync");
        }

        /// <summary>
        /// Reads a 32 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads two input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the int data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Int32/{offset}")]
        [SwaggerOperation(Tags = new[] { "Int32 Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<int>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyInt32Async(ushort offset, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyInt32Async");
        }

        /// <summary>
        /// Reads an unsigned 32 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads two input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the uint data values.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("UInt32/{offset}")]
        [SwaggerOperation(Tags = new[] { "UInt32 Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<uint>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyUInt32Async(ushort offset, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyUInt32Async");
        }

        /// <summary>
        /// Reads a 32 bit IEEE floating point number from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads two input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the float data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Float/{offset}")]
        [SwaggerOperation(Tags = new[] { "Float Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<float>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyFloatAsync(ushort offset, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyFloatAsync");
        }

        /// <summary>
        /// Reads a 64 bit IEEE floating point number from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads four input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the double data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Double/{offset}")]
        [SwaggerOperation(Tags = new[] { "Double Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<double>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyDoubleAsync(ushort offset, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyDoubleAsync");
        }

        /// <summary>
        /// Reads a 64 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads four input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the long data values.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Long/{offset}")]
        [SwaggerOperation(Tags = new[] { "Long Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<long>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyLongAsync(ushort offset, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyLongAsync");
        }

        /// <summary>
        /// Reads an unsigned 64 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads four input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the ulong data values.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("ULong/{offset}")]
        [SwaggerOperation(Tags = new[] { "ULong Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<ulong>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyULongAsync(ushort offset, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusMasterData(_settings), new ModbusSlaveData(_settings, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadOnlyULongAsync");
        }
    }
}
