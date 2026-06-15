// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
namespace Thermo.Kronos.Instrument.Camera.Contracts.Enums
{
    /// <summary>
    /// These are the different log levels, the camera should use.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// The fatal log flag.
        /// </summary>
        Fatal,

        /// <summary>
        /// The error log flag.
        /// </summary>
        Error,

        /// <summary>
        /// The warning log flag.
        /// </summary>
        Warning,

        /// <summary>
        /// The information log flag.
        /// </summary>
        Info,

        /// <summary>
        /// The debug log flag.
        /// </summary>
        Debug,
        
        /// <summary>
        /// The camera log flag.
        /// </summary>
        Camera
    }
}