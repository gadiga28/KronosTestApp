// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary>
    /// contains the camera image, as .net structure and raw data.
    /// </summary>
    [Serializable]
    public class CameraImage
    {
        /// <summary>
        /// Gets or sets the initial timestamp.
        /// </summary>
        public int InitialTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the read timestamp.
        /// </summary>
        public int ReadTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the exposure tag.
        /// </summary>
        public int ExposureTag { get; set; }

        /// <summary>
        /// Gets or sets the type of the image.
        /// </summary>
        public int ImageType { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public int[,] Data { get; set; }

        /// <summary>
        /// Gets or sets the subarray infos.
        /// </summary>
        public IList<SubarrayInfo> SubarrayInfos { get; set; }
    }
}