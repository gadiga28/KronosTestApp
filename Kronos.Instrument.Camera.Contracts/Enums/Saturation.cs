// ------------------------------------------------------------------
// © Copyright 2019 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
namespace Thermo.Kronos.Instrument.Camera.Contracts.Enums
{
    /// <summary>
    /// Saturation enum shows the saturation state which is reported by the camera.
    /// </summary>
    public enum Saturation
    {
        /// <summary>
        /// No saturation detected by the camera.
        /// </summary>
        No = 0,

        /// <summary>
        /// if the threshold is saturated, this flag is used.
        /// </summary>
        Threshold = 1,

        /// <summary>
        /// if the subarray is saturated, this flag is used.
        /// </summary>
        Subarray = 2,
        
        /// <summary>
        /// if both, the threshold and the subarray is saturated, this flag is used.
        /// </summary>
        SubarrayAndThreshold = 3
    }
}