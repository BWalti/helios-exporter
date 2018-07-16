// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusApp
{
    using System.Collections.Generic;
    using NModbusApp.Models;

    /// <summary>
    /// Helper class to provide application specific settings.
    /// </summary>
    public class AppSettings
    {
        #region Public Properties

        public ModbusMaster Master { get; set; } = new ModbusMaster();
        public ModbusSlave Slave { get; set; } = new ModbusSlave();

        #endregion
    }
}
