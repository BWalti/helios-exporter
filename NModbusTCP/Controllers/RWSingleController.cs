// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RWSingleController.cs" company="DTV-Online">
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
    /// The Modbus Gateway MVC Controller for reading and writing various data values.
    /// </summary>
    /// <para>
    ///     ReadString          Reads an ASCII string (multiple holding registers)
    ///     ReadHexString       Reads an HEX string (multiple holding registers)
    ///     ReadBool            Reads a boolean value (single coil)
    ///     ReadBits            Reads a 16-bit bit array value (single holding register)
    ///     ReadShort           Reads a 16 bit integer (single holding register)
    ///     ReadUShort          Reads an unsigned 16 bit integer (single holding register)
    ///     ReadInt32           Reads a 32 bit integer (two holding registers)
    ///     ReadUInt32          Reads an unsigned 32 bit integer (two holding registers)
    ///     ReadFloat           Reads a 32 bit IEEE floating point number (two holding registers)
    ///     ReadDouble          Reads a 64 bit IEEE floating point number (four holding registers)
    ///     ReadLong            Reads a 64 bit integer (four holding registers)
    ///     ReadULong           Reads an unsigned 64 bit integer (four holding registers)
    /// </para>
    /// <para>
    ///     WriteString         Writes an ASCII string (multiple holding registers)
    ///     WriteHexString      Writes an HEX string (multiple holding registers)
    ///     WriteBool           Writes a boolean value (single coil)
    ///     WriteBits           Writes a 16-bit bit array value (single holding register)
    ///     WriteShort          Writes a 16 bit integer (single holding register)
    ///     WriteUShort         Writes an unsigned 16 bit integer (single holding register)
    ///     WriteInt32          Writes a 32 bit integer (two holding registers)
    ///     WriteUInt32         Writes an unsigned 32 bit integer (two holding registers)
    ///     WriteFloat          Writes a 32 bit IEEE floating point number (two holding registers)
    ///     WriteDouble         Writes a 64 bit IEEE floating point number (four holding registers)
    ///     WriteLong           Writes a 64 bit integer (four holding registers)
    ///     WriteULong          Writes an unsigned 64 bit integer (four holding registers)
    /// </para>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RWSingleController : ControllerBase
    {
        #region Private Data Members

        private readonly ILogger _logger;
        private readonly AppSettings _settings = new AppSettings();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RWSingleController"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="logger">The application looger.</param>
        public RWSingleController(IConfiguration configuration, ILogger<RWSingleController> logger)
        {
            _logger = logger;
            configuration.GetSection("AppSettings")?.Bind(_settings);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads an ASCII string (multiple holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="number">The number of characters in the string.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the string data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("String/{offset}")]
        [SwaggerOperation(Tags = new[] { "String Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseStringData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadStringAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadStringAsync");
        }

        /// <summary>
        /// Reads an HEX string (multiple holding registers) from a Modbus slave.
        /// </summary>
        /// <remarks>Note that the resulting HEX string length is twice the number parameter.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="number">The number of 2-bytes HEX substrings in the string.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the HEX string data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("HexString/{offset}")]
        [SwaggerOperation(Tags = new[] { "HEX String Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseStringData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadHexStringAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadHexStringAsync");
        }

        /// <summary>
        /// Reads a boolean value (single coil) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the boolean data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Bool/{offset}")]
        [SwaggerOperation(Tags = new[] { "Boolean Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<bool>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadBoolAsync(ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadBoolAsync");
        }

        /// <summary>
        /// Reads a 16-bit bit array value from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the 16-bit data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Bits/{offset}")]
        [SwaggerOperation(Tags = new[] { "Bits Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseStringData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadBitsAsync(ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadBitsAsync");
        }

        /// <summary>
        /// Reads a 16 bit integer (single holding register) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the short data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Short/{offset}")]
        [SwaggerOperation(Tags = new[] { "Short Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<short>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadShortAsync(ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadShortAsync");
        }

        /// <summary>
        /// Reads an unsigned 16 bit integer (single holding register) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the ushort data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("UShort/{offset}")]
        [SwaggerOperation(Tags = new[] { "UShort Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<ushort>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadUShortAsync(ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadUShortAsync");
        }

        /// <summary>
        /// Reads a 32 bit integer (two holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the int data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Int32/{offset}")]
        [SwaggerOperation(Tags = new[] { "Int32 Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<int>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadInt32Async(ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadInt32Async");
        }

        /// <summary>
        /// Reads an unsigned 32 bit integer (two holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the uint data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("UInt32/{offset}")]
        [SwaggerOperation(Tags = new[] { "UInt32 Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<uint>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadUInt32Async(ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadUInt32Async");
        }

        /// <summary>
        /// Reads a 32 bit IEEE floating point number (two holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the float data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Float/{offset}")]
        [SwaggerOperation(Tags = new[] { "Float Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<float>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadFloatAsync(ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadFloatAsync");
        }

        /// <summary>
        /// Reads a 64 bit IEEE floating point number (four holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the double data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Double/{offset}")]
        [SwaggerOperation(Tags = new[] { "Double Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<double>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadDoubleAsync(ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadDoubleAsync");
        }

        /// <summary>
        /// Reads a 64 bit integer (four holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the long data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Long/{offset}")]
        [SwaggerOperation(Tags = new[] { "Long Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<long>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadLongAsync(ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadLongAsync");
        }

        /// <summary>
        /// Reads an unsigned 64 bit integer (four holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the ulong data value.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("ULong/{offset}")]
        [SwaggerOperation(Tags = new[] { "ULong Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<ulong>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadULongAsync(ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusReadRequest(_logger, request, "ReadULongAsync");
        }

        /// <summary>
        /// Writes an ASCII string (multiple holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus string data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("String/{offset}")]
        [SwaggerOperation(Tags = new[] { "String Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteStringAsync(ushort offset, string data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Length);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteStringAsync");
        }

        /// <summary>
        /// Writes an HEX string (multiple holding registers) to a Modbus slave.
        /// </summary>
        /// <remarks>The HEX string is limited to pairs of HEX digits (0..9A..F).</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus HEX string data value.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("HexString/{offset}")]
        [SwaggerOperation(Tags = new[] { "HEX String Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteHexStringAsync(ushort offset, string data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Length);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteHexStringAsync");
        }

        /// <summary>
        /// Writes a boolean value (single coil) to a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        [HttpPut("Bool/{offset}")]
        [SwaggerOperation(Tags = new[] { "Boolean Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteBoolAsync(ushort offset, bool data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteBoolAsync");
        }

        /// <summary>
        /// Writes a 16-bit bit array value to a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        [HttpPut("Bits/{offset}")]
        [SwaggerOperation(Tags = new[] { "Bits Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteBitsAsync(ushort offset, string data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteBitsAsync");
        }

        /// <summary>
        /// Writes a 16 bit integer (single holding register) to a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        [HttpPut("Short/{offset}")]
        [SwaggerOperation(Tags = new[] { "Short Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteShortAsync(ushort offset, short data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteShortAsync");
        }

        /// <summary>
        /// Writes an unsigned 16 bit integer (single holding register) to a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        [HttpPut("UShort/{offset}")]
        [SwaggerOperation(Tags = new[] { "UShort Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteUShortAsync(ushort offset, ushort data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteUShortAsync");
        }

        /// <summary>
        /// Writes a 32 bit integer (two holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        [HttpPut("Int32/{offset}")]
        [SwaggerOperation(Tags = new[] { "Int32 Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteInt32Async(ushort offset, int data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteInt32Async");
        }

        /// <summary>
        /// Writes an unsigned 32 bit integer (two holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        [HttpPut("UInt32/{offset}")]
        [SwaggerOperation(Tags = new[] { "UInt32 Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteUInt32Async(ushort offset, uint data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteUInt32Async");
        }

        /// <summary>
        /// Writes a 32 bit IEEE floating point number (two holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        [HttpPut("Float/{offset}")]
        [SwaggerOperation(Tags = new[] { "Float Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteFloatAsync(ushort offset, float data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteFloatAsync");
        }

        /// <summary>
        /// Writes a 64 bit IEEE floating point number (four holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        [HttpPut("Double/{offset}")]
        [SwaggerOperation(Tags = new[] { "Double Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteDoubleAsync(ushort offset, double data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteDoubleAsync");
        }

        /// <summary>
        /// Writes a 64 bit integer (four holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        [HttpPut("Long/{offset}")]
        [SwaggerOperation(Tags = new[] { "Long Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteLongAsync(ushort offset, long data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteLongAsync");
        }

        /// <summary>
        /// Writes an unsigned 64 bit integer (four holding registers) to a Modbus slave.
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
        [HttpPut("ULong/{offset}")]
        [SwaggerOperation(Tags = new[] { "ULong Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteULongArrayAsync(ushort offset, ulong data, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, 1);
            return await this.ModbusWriteSingleRequest(_logger, request, data, "WriteULongArrayAsync");
        }

        #endregion
    }
}
