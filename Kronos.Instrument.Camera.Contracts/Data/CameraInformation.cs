// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using System;

namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary>
    /// CameraInformation
    /// </summary>
    [Serializable]
    public class CameraInformation
    {
        /// <summary>
        /// Gets or sets the camera revision.
        /// </summary>
        public int CameraVersion { get; set; }

        /// <summary>
        /// Gets or sets the type of the camera.
        /// </summary>
        public int CameraType { get; set; }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        public int FirmwareRevision { get; set; }

        /// <summary>
        /// Gets or sets the camera API version.
        /// </summary>
        public int CameraApiVersion { get; set; }

        /// <summary>
        /// Gets or sets the board version.
        /// </summary>
        public int BoardVersion { get; set; }

        /// <summary>
        /// Length of every serial number.
        /// </summary>
        public const int MAC_ADDRESS_LENGTH = 20;

        /// <summary>
        /// Gets or sets the cameras CPU iREX board serial number.
        /// </summary>
        public char[] MACAddress { get; set; }

        /// <summary>
        /// Gets or sets the camera FPGA version id.
        /// </summary>
        public int FPGAVersion { get; set; }

        /// <summary>
        /// Gets or sets the camera Board Support Package (BSP) version id.
        /// </summary>
        public int BoardSupportPackageVersion { get; set; }

        /// <summary>
        /// Gets or sets the all of the camera realted serial numbers.
        /// </summary>
        public CameraSerialNumbers SerialNumbers { get; set; }      

    }
}