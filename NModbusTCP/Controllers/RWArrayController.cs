// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RWArrayController.cs" company="DTV-Online">
//   Copyright(c) 2017 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace ModbusTCP.Controllers
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;
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
    ///     ReadBoolArray       Reads an array of boolean values (multiple coils)
    ///     ReadBytes           Reads 8 bit values (multiple holding register)
    ///     ReadShortArray      Reads an array of 16 bit integers (multiple holding register)
    ///     ReadUShortArray     Reads an array of unsigned 16 bit integer (multiple holding register)
    ///     ReadInt32Array      Reads an array of 32 bit integers (multiple holding registers)
    ///     ReadUInt32Array     Reads an array of unsigned 32 bit integers (multiple holding registers)
    ///     ReadFloatArray      Reads an array of 32 bit IEEE floating point numbers (multiple holding registers)
    ///     ReadDoubleArray     Reads an array of 64 bit IEEE floating point numbers (multiple holding registers)
    ///     ReadLongArray       Reads an array of 64 bit integers (multiple holding registers)
    ///     ReadULongArray      Reads an array of unsigned 64 bit integers (multiple holding registers)
    /// </para>
    /// <para>
    ///     WriteBoolArray      Writes an array of boolean values (multiple coils)
    ///     WriteBytes          Writes 8 bit values (multiple holding register)
    ///     WriteShortArray     Writes an array of 16 bit integers (multiple holding register)
    ///     WriteUShortArray    Writes an array of unsigned 16 bit integer (multiple holding register)
    ///     WriteInt32Array     Writes an array of 32 bit integers (multiple holding registers)
    ///     WriteUInt32Array    Writes an array of unsigned 32 bit integers (multiple holding registers)
    ///     WriteFloatArray     Writes an array of 32 bit IEEE floating point numbers (multiple holding registers)
    ///     WriteDoubleArray    Writes an array of 64 bit IEEE floating point numbers (multiple holding registers)
    ///     WriteLongArray      Writes an array of 64 bit integers (multiple holding registers)
    ///     WriteULongArray     Writes an array of unsigned 64 bit integers (multiple holding registers)
    /// </para>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RWArrayController : ControllerBase
    {
        #region Private Data Members

        private readonly ILogger _logger;
        private readonly AppSettings _settings = new AppSettings();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RWArrayController"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="logger">The application looger.</param>
        public RWArrayController(IConfiguration configuration, ILogger<RWArrayController> logger)
        {
            _logger = logger;
            configuration.GetSection("AppSettings")?.Bind(_settings);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads an array of boolean values (multiple coils) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of Modbus data values.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of boolean data values.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Bool/{offset}")]
        [SwaggerOperation(Tags = new[] { "Boolean Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<bool>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadBoolArrayAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadBoolArrayAsync");
        }

        /// <summary>
        /// Reads 8-bit values (multiple holding register) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of Modbus data values.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of 8-bit data values.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Bytes/{offset}")]
        [SwaggerOperation(Tags = new[] { "8-Bit Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<byte>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadBytesAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadBytesAsync");
        }

        /// <summary>
        /// Reads an array of 16 bit integers (multiple holding register) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of short data values.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Short/{offset}")]
        [SwaggerOperation(Tags = new[] { "Short Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<short>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadShortArrayAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadShortArrayAsync");
        }

        /// <summary>
        /// Reads an array of unsigned 16 bit integer (multiple holding register) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of ushort data values.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("UShort/{offset}")]
        [SwaggerOperation(Tags = new[] { "UShort Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<ushort>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadUShortArrayAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadUShortArrayAsync");
        }

        /// <summary>
        /// Reads an array of 32 bit integers (multiple holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of int data values.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Int32/{offset}")]
        [SwaggerOperation(Tags = new[] { "Int32 Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<int>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadInt32ArrayAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadInt32ArrayAsync");
        }

        /// <summary>
        /// Reads an array of unsigned 32 bit integers (multiple holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of uint data values.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("UInt32/{offset}")]
        [SwaggerOperation(Tags = new[] { "UInt32 Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<uint>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadUInt32ArrayAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadUInt32ArrayAsync");
        }

        /// <summary>
        /// Reads an array of 32 bit IEEE floating point numbers (multiple holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of float data values.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Float/{offset}")]
        [SwaggerOperation(Tags = new[] { "Float Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<float>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadFloatArrayAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadFloatArrayAsync");
        }

        /// <summary>
        /// Reads an array of 64 bit IEEE floating point numbers (multiple holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of double data values.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Double/{offset}")]
        [SwaggerOperation(Tags = new[] { "Double Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<double>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadDoubleArrayAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadDoubleArrayAsync");
        }

        /// <summary>
        /// Reads an array of 64 bit integers (multiple holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of long data values.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Long/{offset}")]
        [SwaggerOperation(Tags = new[] { "Long Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<long>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadLongArrayAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadLongArrayAsync");
        }

        /// <summary>
        /// Reads an array of unsigned 64 bit integers (multiple holding registers) from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of ulong data values.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("ULong/{offset}")]
        [SwaggerOperation(Tags = new[] { "ULong Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<ulong>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadULongArrayAsync(ushort offset, ushort number, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, number);
            return await this.ModbusReadRequest(_logger, request, "ReadULongArrayAsync");
        }

        /// <summary>
        /// Writes an array of boolean values (multiple coils) to a Modbus slave.
        /// </summary>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        public async Task<IActionResult> WriteBoolArrayAsync([FromBody, Required] ModbusDataValues<bool> data, ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Values.Length);
            return await this.ModbusWriteArrayRequest(_logger, request, data.Values, "WriteBoolArrayAsync");
        }

        /// <summary>
        /// Writes 8-bit values (multiple holding register) to a Modbus slave.
        /// </summary>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="address">The IP address of the Modbus TCP slave.</param>
        /// <param name="port">The IP port number of the Modbus TCP slave.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway has invalid arguments.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("Bytes/{offset}")]
        [SwaggerOperation(Tags = new[] { "8-Bit Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteBytesAsync([FromBody, Required] ModbusDataValues<byte> data, ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Values.Length);
            return await this.ModbusWriteArrayRequest(_logger, request, data.Values, "WriteBytesAsync");
        }

        /// <summary>
        /// Writes an array of 16 bit integers (multiple holding register) to a Modbus slave.
        /// </summary>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        public async Task<IActionResult> WriteFloatArrayAsync([FromBody, Required] ModbusDataValues<short> data, ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Values.Length);
            return await this.ModbusWriteArrayRequest(_logger, request, data.Values, "WriteFloatArrayAsync");
        }

        /// <summary>
        /// Writes an array of unsigned 16 bit integer (multiple holding register) to a Modbus slave.
        /// </summary>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        public async Task<IActionResult> WriteFloatArrayAsync([FromBody, Required] ModbusDataValues<ushort> data, ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Values.Length);
            return await this.ModbusWriteArrayRequest(_logger, request, data.Values, "WriteFloatArrayAsync");
        }

        /// <summary>
        /// Writes an array of 32 bit integers (multiple holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        public async Task<IActionResult> WriteFloatArrayAsync([FromBody, Required] ModbusDataValues<int> data, ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Values.Length);
            return await this.ModbusWriteArrayRequest(_logger, request, data.Values, "WriteFloatArrayAsync");
        }

        /// <summary>
        /// Writes an array of unsigned 32 bit integers (multiple holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        public async Task<IActionResult> WriteFloatArrayAsync([FromBody, Required] ModbusDataValues<uint> data, ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Values.Length);
            return await this.ModbusWriteArrayRequest(_logger, request, data.Values, "WriteFloatArrayAsync");
        }

        /// <summary>
        /// Writes an array of 32 bit IEEE floating point numbers (multiple holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        public async Task<IActionResult> WriteFloatArrayAsync([FromBody, Required] ModbusDataValues<float> data, ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Values.Length);
            return await this.ModbusWriteArrayRequest(_logger, request, data.Values, "WriteFloatArrayAsync");
        }

        /// <summary>
        /// Writes an array of 64 bit IEEE floating point numbers (multiple holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        public async Task<IActionResult> WriteDoubleArrayAsync([FromBody, Required] ModbusDataValues<double> data, ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Values.Length);
            return await this.ModbusWriteArrayRequest(_logger, request, data.Values, "WriteDoubleArrayAsync");
        }

        /// <summary>
        /// Writes an array of 64 bit integers (multiple holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        public async Task<IActionResult> WriteLongArrayAsync([FromBody, Required] ModbusDataValues<long> data, ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Values.Length);
            return await this.ModbusWriteArrayRequest(_logger, request, data.Values, "WriteLongArrayAsync");
        }

        /// <summary>
        /// Writes an array of unsigned 64 bit integers (multiple holding registers) to a Modbus slave.
        /// </summary>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
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
        public async Task<IActionResult> WriteULongArrayAsync([FromBody, Required] ModbusDataValues<ulong> data, ushort offset, string address = null, int? port = null, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData(new ModbusSlaveData(_settings, address, port, slave), offset, data.Values.Length);
            return await this.ModbusWriteArrayRequest(_logger, request, data.Values, "WriteULongArrayAsync");
        }

        #endregion
    }
}
