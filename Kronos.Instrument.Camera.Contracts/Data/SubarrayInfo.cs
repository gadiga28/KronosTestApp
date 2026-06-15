// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class SubarrayInfo
    {
        /// <summary>
        /// Gets or sets the subarray tag.
        /// </summary>
        public short SubarrayTag { get; set; }

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
    }
}