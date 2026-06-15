// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using System;

namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary>
    /// Camera Serial Numbers
    /// </summary>
    [Serializable]
    public struct CameraSerialNumbers
    {
        /// <summary>
        /// Length of every serial number.
        /// </summary>
        public const int SERIAL_NUMBER_LENGTH = 20;

        /// <summary>
        /// Gets or sets the cameras CPU iREX board serial number.
        /// </summary>
        public char[] CPUSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the camera CSP board serial number.
        /// </summary>
        public char[] CSPSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the camera Power board serial number.
        /// </summary>
        public char[] PWRBoardSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the camera imager serial number.
        /// </summary>
        public char[] ImagerSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the camera serial number.
        /// </summary>
        public char[] CameraSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the camera ISI board serial number.
        /// </summary>
        public char[] ISISerialNumber { get; set; }

    }
}