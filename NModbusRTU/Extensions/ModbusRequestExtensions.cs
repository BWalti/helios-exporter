// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModbusRequestExtensions.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Extensions
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.IO.Ports;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using NModbus;
    using NModbus.Extensions;
    using NModbus.Serial;
    using NModbusRTU.Models;

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public static class ModbusRequestExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="logger"></param>
        /// <param name="request"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static async Task<IActionResult> ModbusReadRequest(this ControllerBase controller, ILogger logger, ModbusRequestData request, string function)
        {
            try
            {
                using (SerialPort serialport = new SerialPort(request.Master.SerialPort, request.Master.Baudrate, request.Master.Parity, request.Master.DataBits, request.Master.StopBits))
                {
                    serialport.Open();

                    if (serialport.IsOpen)
                    {
                        var adapter = new SerialPortAdapter(serialport);
                        var factory = new ModbusFactory();
                        IModbusMaster modbus = factory.CreateRtuMaster(adapter);
                        modbus.Transport.SlaveBusyUsesRetryCount = true;
                        modbus.Transport.ReadTimeout = request.Master.ReadTimeout;
                        modbus.Transport.WriteTimeout = request.Master.WriteTimeout;

                        switch (function)
                        {
                            case "ReadCoilAsync":
                                {
                                    bool[] values = await modbus.ReadCoilsAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseData<bool>(request, values[0]));
                                }
                            case "ReadCoilsAsync":
                                {
                                    bool[] values = await modbus.ReadCoilsAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<bool>(request, values));
                                }
                            case "ReadInputAsync":
                                {
                                    bool[] values = await modbus.ReadInputsAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseData<bool>(request, values[0]));
                                }
                            case "ReadInputsAsync":
                                {
                                    bool[] values = await modbus.ReadInputsAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<bool>(request, values));
                                }
                            case "ReadHoldingRegisterAsync":
                                {
                                    ushort[] values = await modbus.ReadHoldingRegistersAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseData<ushort>(request, values[0]));
                                }
                            case "ReadHoldingRegistersAsync":
                                {
                                    ushort[] values = await modbus.ReadHoldingRegistersAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<ushort>(request, values));
                                }
                            case "ReadInputRegisterAsync":
                                {
                                    ushort[] values = await modbus.ReadInputRegistersAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseData<ushort>(request, values[0]));
                                }
                            case "ReadInputRegistersAsync":
                                {
                                    ushort[] values = await modbus.ReadInputRegistersAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<ushort>(request, values));
                                }
                            case "ReadOnlyStringAsync":
                                {
                                    string value = await modbus.ReadOnlyStringAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseStringData(request, value));

                                }
                            case "ReadOnlyHexStringAsync":
                                {
                                    string value = await modbus.ReadOnlyHexStringAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseStringData(request, value));
                                }
                            case "ReadOnlyBoolAsync":
                                {
                                    bool value = await modbus.ReadOnlyBoolAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<bool>(request, value));
                                }
                            case "ReadOnlyBitsAsync":
                                {
                                    BitArray value = await modbus.ReadOnlyBitsAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseStringData(request, value.ToDigitString()));
                                }
                            case "ReadOnlyShortAsync":
                                {
                                    short value = await modbus.ReadOnlyShortAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<short>(request, value));
                                }
                            case "ReadOnlyUShortAsync":
                                {
                                    ushort value = await modbus.ReadOnlyUShortAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<ushort>(request, value));
                                }
                            case "ReadOnlyInt32Async":
                                {
                                    int value = await modbus.ReadOnlyInt32Async(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<int>(request, value));
                                }
                            case "ReadOnlyUInt32Async":
                                {
                                    uint value = await modbus.ReadOnlyUInt32Async(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<uint>(request, value));
                                }
                            case "ReadOnlyFloatAsync":
                                {
                                    float value = await modbus.ReadOnlyFloatAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<float>(request, value));
                                }
                            case "ReadOnlyDoubleAsync":
                                {
                                    double value = await modbus.ReadOnlyDoubleAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<double>(request, value));
                                }
                            case "ReadOnlyLongAsync":
                                {
                                    long value = await modbus.ReadOnlyLongAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<long>(request, value));
                                }
                            case "ReadOnlyULongAsync":
                                {
                                    ulong value = await modbus.ReadOnlyULongAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<ulong>(request, value));
                                }
                            case "ReadOnlyBoolArrayAsync":
                                {
                                    bool[] values = await modbus.ReadOnlyBoolArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<bool>(request, values));
                                }
                            case "ReadOnlyBytesAsync":
                                {
                                    byte[] values = await modbus.ReadOnlyBytesAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<byte>(request, values));
                                }
                            case "ReadOnlyShortArrayAsync":
                                {
                                    short[] values = await modbus.ReadOnlyShortArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<short>(request, values));
                                }
                            case "ReadOnlyUShortArrayAsync":
                                {
                                    ushort[] values = await modbus.ReadOnlyUShortArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<ushort>(request, values));
                                }
                            case "ReadOnlyInt32ArrayAsync":
                                {
                                    int[] values = await modbus.ReadOnlyInt32ArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<int>(request, values));
                                }
                            case "ReadOnlyUInt32ArrayAsync":
                                {
                                    uint[] values = await modbus.ReadOnlyUInt32ArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<uint>(request, values));
                                }
                            case "ReadOnlyFloatArrayAsync":
                                {
                                    float[] values = await modbus.ReadOnlyFloatArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<float>(request, values));
                                }
                            case "ReadOnlyDoubleArrayAsync":
                                {
                                    double[] values = await modbus.ReadOnlyDoubleArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<double>(request, values));
                                }
                            case "ReadOnlyLongArrayAsync":
                                {
                                    long[] values = await modbus.ReadOnlyLongArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<long>(request, values));
                                }
                            case "ReadOnlyULongArrayAsync":
                                {
                                    ulong[] values = await modbus.ReadOnlyULongArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<ulong>(request, values));
                                }
                            case "ReadStringAsync":
                                {
                                    string value = await modbus.ReadStringAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseStringData(request, value));

                                }
                            case "ReadHexStringAsync":
                                {
                                    string value = await modbus.ReadHexStringAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseStringData(request, value));
                                }
                            case "ReadBoolAsync":
                                {
                                    bool value = await modbus.ReadBoolAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<bool>(request, value));
                                }
                            case "ReadBitsAsync":
                                {
                                    BitArray value = await modbus.ReadBitsAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseStringData(request, value.ToDigitString()));
                                }
                            case "ReadShortAsync":
                                {
                                    short value = await modbus.ReadShortAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<short>(request, value));
                                }
                            case "ReadUShortAsync":
                                {
                                    ushort value = await modbus.ReadUShortAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<ushort>(request, value));
                                }
                            case "ReadInt32Async":
                                {
                                    int value = await modbus.ReadInt32Async(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<int>(request, value));
                                }
                            case "ReadUInt32Async":
                                {
                                    uint value = await modbus.ReadUInt32Async(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<uint>(request, value));
                                }
                            case "ReadFloatAsync":
                                {
                                    float value = await modbus.ReadFloatAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<float>(request, value));
                                }
                            case "ReadDoubleAsync":
                                {
                                    double value = await modbus.ReadDoubleAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<double>(request, value));
                                }
                            case "ReadLongAsync":
                                {
                                    long value = await modbus.ReadLongAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<long>(request, value));
                                }
                            case "ReadULongAsync":
                                {
                                    ulong value = await modbus.ReadULongAsync(request.Slave.ID, request.Offset);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {value}");
                                    return controller.Ok(new ModbusResponseData<ulong>(request, value));
                                }
                            case "ReadBoolArrayAsync":
                                {
                                    bool[] values = await modbus.ReadBoolArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<bool>(request, values));
                                }
                            case "ReadBytesAsync":
                                {
                                    byte[] values = await modbus.ReadBytesAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<byte>(request, values));
                                }
                            case "ReadShortArrayAsync":
                                {
                                    short[] values = await modbus.ReadShortArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<short>(request, values));
                                }
                            case "ReadUShortArrayAsync":
                                {
                                    ushort[] values = await modbus.ReadUShortArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<ushort>(request, values));
                                }
                            case "ReadInt32ArrayAsync":
                                {
                                    int[] values = await modbus.ReadInt32ArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<int>(request, values));
                                }
                            case "ReadUInt32ArrayAsync":
                                {
                                    uint[] values = await modbus.ReadUInt32ArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<uint>(request, values));
                                }
                            case "ReadFloatArrayAsync":
                                {
                                    float[] values = await modbus.ReadFloatArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<float>(request, values));
                                }
                            case "ReadDoubleArrayAsync":
                                {
                                    double[] values = await modbus.ReadDoubleArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<double>(request, values));
                                }
                            case "ReadLongArrayAsync":
                                {
                                    long[] values = await modbus.ReadLongArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<long>(request, values));
                                }
                            case "ReadULongArrayAsync":
                                {
                                    ulong[] values = await modbus.ReadULongArrayAsync(request.Slave.ID, request.Offset, request.Number);
                                    serialport.Close();
                                    logger.LogTrace($"{function}(): {values}");
                                    return controller.Ok(new ModbusResponseArrayData<ulong>(request, values));
                                }
                            default:
                                serialport.Close();
                                logger.LogError($"RTU master read request {function}() not supported.");
                                return controller.NotFound($"RTU master read request {function}() not supported.");
                        }
                    }
                    else
                    {
                        logger.LogError($"RTU master ({request.Master.SerialPort}) not open.");
                        return controller.NotFound("RTU master COM port not open.");
                    }
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                logger.LogError(uae, $"{function}() Unauthorized Access Exception.");
                return controller.NotFound($"Unauthorized Access Exception: {uae.Message}");
            }
            catch (ArgumentOutOfRangeException are)
            {
                logger.LogError(are, $"{function}() Argument out of Range Exception.");
                return controller.BadRequest($"Argument out of Range Exception: {are.Message}");
            }
            catch (ArgumentException aex)
            {
                logger.LogError(aex, $"{function}() Argument Exception.");
                return controller.BadRequest($"Argument Exception: {aex.Message}");
            }
            catch (NModbus.SlaveException mse)
            {
                logger.LogError(mse, $"{function}() Modbus SlaveException.");
                return controller.StatusCode(502, $"Modbus SlaveException: {mse.Message}");
            }
            catch (System.IO.IOException ioe)
            {
                logger.LogError(ioe, $"{function}() IO Exception.");
                return controller.StatusCode(500, $"IO Exception: {ioe.Message}");
            }
            catch (TimeoutException tex)
            {
                logger.LogError(tex, $"{function}() Timeout Exception.");
                return controller.StatusCode(500, $"Timeout Exception: {tex.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{function}() Exception.");
                return controller.StatusCode(500, $"Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="logger"></param>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static async Task<IActionResult> ModbusWriteSingleRequest<T>(this ControllerBase controller, ILogger logger, ModbusRequestData request, T data, string function)
        {
            try
            {
                using (SerialPort serialport = new SerialPort(request.Master.SerialPort, request.Master.Baudrate, request.Master.Parity, request.Master.DataBits, request.Master.StopBits))
                {
                    serialport.Open();

                    if (serialport.IsOpen)
                    {
                        var adapter = new SerialPortAdapter(serialport);
                        var factory = new ModbusFactory();
                        IModbusMaster modbus = factory.CreateRtuMaster(adapter);
                        modbus.Transport.SlaveBusyUsesRetryCount = true;
                        modbus.Transport.ReadTimeout = request.Master.ReadTimeout;
                        modbus.Transport.WriteTimeout = request.Master.WriteTimeout;

                        switch (function)
                        {
                            case "WriteCoilAsync":
                                {
                                    bool value = (bool)Convert.ChangeType(data, typeof(bool));
                                    await modbus.WriteSingleCoilAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteHoldingRegisterAsync":
                                {
                                    ushort value = (ushort)Convert.ChangeType(data, typeof(ushort));
                                    await modbus.WriteSingleRegisterAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteStringAsync":
                                {
                                    string value = (string)Convert.ChangeType(data, typeof(string));
                                    await modbus.WriteStringAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteHexStringAsync":
                                {
                                    string value = (string)Convert.ChangeType(data, typeof(string));
                                    await modbus.WriteHexStringAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteBoolAsync":
                                {
                                    bool value = (bool)Convert.ChangeType(data, typeof(bool));
                                    await modbus.WriteBoolAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteBitsAsync":
                                {
                                    BitArray value = ((string)Convert.ChangeType(data, typeof(string))).ToBitArray();
                                    await modbus.WriteBitsAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteShortAsync":
                                {
                                    short value = (short)Convert.ChangeType(data, typeof(short));
                                    await modbus.WriteShortAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteUShortAsync":
                                {
                                    ushort value = (ushort)Convert.ChangeType(data, typeof(ushort));
                                    await modbus.WriteUShortAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteInt32Async":
                                {
                                    int value = (int)Convert.ChangeType(data, typeof(int));
                                    await modbus.WriteInt32Async(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteUInt32Async":
                                {
                                    uint value = (uint)Convert.ChangeType(data, typeof(uint));
                                    await modbus.WriteUInt32Async(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteFloatAsync":
                                {
                                    float value = (float)Convert.ChangeType(data, typeof(float));
                                    await modbus.WriteFloatAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteDoubleAsync":
                                {
                                    double value = (double)Convert.ChangeType(data, typeof(double));
                                    await modbus.WriteDoubleAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteLongAsync":
                                {
                                    long value = (long)Convert.ChangeType(data, typeof(long));
                                    await modbus.WriteLongAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteULongAsync":
                                {
                                    ulong value = (ulong)Convert.ChangeType(data, typeof(ulong));
                                    await modbus.WriteULongAsync(request.Slave.ID, request.Offset, value);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            default:
                                serialport.Close();
                                logger.LogError($"RTU master write request {function}() not supported.");
                                return controller.NotFound($"RTU master write request {function}() not supported.");
                        }
                    }
                    else
                    {
                        logger.LogError($"RTU master ({request.Master.SerialPort}) not open.");
                        return controller.NotFound("RTU master COM port not open.");
                    }
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                logger.LogError(uae, $"{function}() Unauthorized Access Exception.");
                return controller.NotFound($"Unauthorized Access Exception: {uae.Message}");
            }
            catch (ArgumentOutOfRangeException are)
            {
                logger.LogError(are, $"{function}() Argument out of Range Exception.");
                return controller.BadRequest($"Argument out of Range Exception: {are.Message}");
            }
            catch (ArgumentException aex)
            {
                logger.LogError(aex, $"{function}() Argument Exception.");
                return controller.BadRequest($"Argument Exception: {aex.Message}");
            }
            catch (NModbus.SlaveException mse)
            {
                logger.LogError(mse, $"{function}() Modbus SlaveException.");
                return controller.StatusCode(502, $"Modbus SlaveException: {mse.Message}");
            }
            catch (System.IO.IOException ioe)
            {
                logger.LogError(ioe, $"{function}() IO Exception.");
                return controller.StatusCode(500, $"IO Exception: {ioe.Message}");
            }
            catch (TimeoutException tex)
            {
                logger.LogError(tex, $"{function}() Timeout Exception.");
                return controller.StatusCode(500, $"Timeout Exception: {tex.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{function}() Exception.");
                return controller.StatusCode(500, $"Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="logger"></param>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static async Task<IActionResult> ModbusWriteArrayRequest<T>(this ControllerBase controller, ILogger logger, ModbusRequestData request, T[] data, string function)
        {
            try
            {
                using (SerialPort serialport = new SerialPort(request.Master.SerialPort, request.Master.Baudrate, request.Master.Parity, request.Master.DataBits, request.Master.StopBits))
                {
                    serialport.Open();

                    if (serialport.IsOpen)
                    {
                        var adapter = new SerialPortAdapter(serialport);
                        var factory = new ModbusFactory();
                        IModbusMaster modbus = factory.CreateRtuMaster(adapter);
                        modbus.Transport.SlaveBusyUsesRetryCount = true;
                        modbus.Transport.ReadTimeout = request.Master.ReadTimeout;
                        modbus.Transport.WriteTimeout = request.Master.WriteTimeout;

                        switch (function)
                        {
                            case "WriteCoilsAsync":
                                {
                                    bool[] values = (bool[])Convert.ChangeType(data, typeof(bool[]));
                                    await modbus.WriteMultipleCoilsAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteHoldingRegistersAsync":
                                {
                                    ushort[] values = (ushort[])Convert.ChangeType(data, typeof(ushort[]));
                                    await modbus.WriteMultipleRegistersAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteBoolArrayAsync":
                                {
                                    bool[] values = (bool[])Convert.ChangeType(data, typeof(bool[]));
                                    await modbus.WriteBoolArrayAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteBytesAsync":
                                {
                                    byte[] values = (byte[])Convert.ChangeType(data, typeof(byte[]));
                                    await modbus.WriteBytesAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteShortArrayAsync":
                                {
                                    short[] values = (short[])Convert.ChangeType(data, typeof(short[]));
                                    await modbus.WriteShortArrayAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteUShortArrayAsync":
                                {
                                    ushort[] values = (ushort[])Convert.ChangeType(data, typeof(ushort[]));
                                    await modbus.WriteUShortArrayAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteInt32ArrayAsync":
                                {
                                    int[] values = (int[])Convert.ChangeType(data, typeof(int[]));
                                    await modbus.WriteInt32ArrayAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteUInt32ArrayAsync":
                                {
                                    uint[] values = (uint[])Convert.ChangeType(data, typeof(uint[]));
                                    await modbus.WriteUInt32ArrayAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteFloatArrayAsync":
                                {
                                    float[] values = (float[])Convert.ChangeType(data, typeof(float[]));
                                    await modbus.WriteFloatArrayAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteDoubleArrayAsync":
                                {
                                    double[] values = (double[])Convert.ChangeType(data, typeof(double[]));
                                    await modbus.WriteDoubleArrayAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteLongArrayAsync":
                                {
                                    long[] values = (long[])Convert.ChangeType(data, typeof(long[]));
                                    await modbus.WriteLongArrayAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            case "WriteULongArrayAsync":
                                {
                                    ulong[] values = (ulong[])Convert.ChangeType(data, typeof(ulong[]));
                                    await modbus.WriteULongArrayAsync(request.Slave.ID, request.Offset, values);
                                    serialport.Close();
                                    logger.LogTrace($"{function}() OK.");
                                    return controller.Ok(request);
                                }
                            default:
                                serialport.Close();
                                logger.LogError($"RTU master write request {function}() not supported.");
                                return controller.NotFound($"RTU master write request {function}() not supported.");
                        }
                    }
                    else
                    {
                        logger.LogError($"RTU master ({request.Master.SerialPort}) not open.");
                        return controller.NotFound("RTU master COM port not open.");
                    }
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                logger.LogError(uae, $"{function}() Unauthorized Access Exception.");
                return controller.NotFound($"Unauthorized Access Exception: {uae.Message}");
            }
            catch (ArgumentOutOfRangeException are)
            {
                logger.LogError(are, $"{function}() Argument out of Range Exception.");
                return controller.BadRequest($"Argument out of Range Exception: {are.Message}");
            }
            catch (ArgumentException aex)
            {
                logger.LogError(aex, $"{function}() Argument Exception.");
                return controller.BadRequest($"Argument Exception: {aex.Message}");
            }
            catch (NModbus.SlaveException mse)
            {
                logger.LogError(mse, $"{function}() Modbus SlaveException.");
                return controller.StatusCode(502, $"Modbus SlaveException: {mse.Message}");
            }
            catch (System.IO.IOException ioe)
            {
                logger.LogError(ioe, $"{function}() IO Exception.");
                return controller.StatusCode(500, $"IO Exception: {ioe.Message}");
            }
            catch (TimeoutException tex)
            {
                logger.LogError(tex, $"{function}() Timeout Exception.");
                return controller.StatusCode(500, $"Timeout Exception: {tex.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{function}() Exception.");
                return controller.StatusCode(500, $"Exception: {ex.Message}");
            }
        }
    }
}
