// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using System;
using Thermo.Kronos.Instrument.Camera.Contracts.Enums;

namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary>
    /// </summary>
    [Serializable]
    //TODO[liverpool]: this is used for the new image getting method from liverpool, clean up to one
    public class Subarray
    {
        /// <summary>
        /// Gets or sets the initial time stamp.
        /// </summary>
        public int InitialTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the read time stamp.
        /// </summary>
        public int ReadTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the exposure tag.
        /// </summary>
        public int ExposureTag { get; set; }

        /// <summary>
        /// Gets or sets the type of the image.
        /// </summary>
        public ImageType ImageType { get; set; }

        /// <summary>
        /// Gets or sets the subarray tag.
        /// </summary>
        public short Tag { get; set; }

        /// <summary>
        /// Gets or sets the start column.
        /// </summary>
        public short StartColumn { get; set; }

        /// <summary>
        /// Gets or sets the start row.
        /// </summary>
        public short StartRow { get; set; }

        /// <summary>
        /// Gets or sets the offset column.
        /// </summary>
        public short OffsetColumn { get; set; }

        /// <summary>
        /// Gets or sets the offset row.
        /// </summary>
        public short OffsetRow { get; set; }

        /// <summary>
        /// Gets or sets the number of non destructive reads.
        /// </summary>
        public short NumberOfNonDestructiveReads { get; set; }

        /// <summary>
        /// Gets or sets the flag if one or more pixels are saturated in a subarray.
        /// </summary>
        public short PixelSaturated { get; set; }

        /// <summary>
        /// Gets or sets the number of Subinjects on a subarray.
        /// </summary>
        public int NumberOfSubinjects { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public int[,] RawData { get; set; }
    }
}