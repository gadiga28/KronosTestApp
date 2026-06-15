// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
namespace Thermo.Kronos.Instrument.Camera.Contracts.Enums
{
    /// <summary>
    /// The image types from the camera.
    /// </summary>
    public enum ImageType : short
    {
        /// <summary>
        /// Default to none
        /// </summary>
        None,

        /// <summary>
        /// warm up image - discard
        /// </summary>
        WarmUp,

        /// <summary>
        /// store dark frame
        /// </summary>
        DarkFrame,      

        /// <summary>
        /// calibrate AutoBias image
        /// </summary>
        CalibrateAutoBias, 

        /// <summary>
        /// store AutoBias image
        /// </summary>
        SaveAutoBias,
         
        /// <summary>
        /// store full frame [x] Exposed Frame
        /// </summary>
        FullFrame,

        /// <summary>
        /// Single frame exposure
        /// </summary>
        FixedExposure,

        /// <summary>
        /// Subarray Threshold region
        /// </summary>
        Examine,

        /// <summary>
        /// save Subarray FixedPatternNoiseReduction image after GI,AB or SI
        /// </summary>
        SAFPN,

        /// <summary>
        /// Subarray sent to host PC. [x] Read 
        /// </summary>
        Subarray, 

        /// <summary>
        /// clear region, save FixedPatternNoiseReduction image [x] SubInject
        /// </summary>
        SubInject,

        /// <summary>
        /// Last read after end of exposure. Integrate with SubInject images. [x] PostRead
        /// </summary>
        PostRead,

        /// <summary>
        /// Integrated subarray(s) at the end of exposure.
        /// </summary>
        Integrated
    }
}