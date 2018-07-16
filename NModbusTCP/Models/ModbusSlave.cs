// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModbusSlave.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusTCP.Models
{
    /// <summary>
    /// Helper class holding Modbus TCP slave data.
    /// </summary>
    public class ModbusSlave
    {
        #region Public Properties

        public string Address { get; set; } = "1.0.0.127";
        public int Port { get; set; } = 502;
        public byte SlaveID { get; set; } = 1;

        #endregion
    }
}
