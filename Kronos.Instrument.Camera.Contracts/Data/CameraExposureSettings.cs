// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary></summary>
    [Serializable]
    public class CameraExposureSettings
    {
        /// <summary>
        /// Gets or sets the subarrays.
        /// </summary>
        public IList<CameraSubarraySettings> Subarrays
        {
            get { return m_subarrays; }
            set { m_subarrays = new List<CameraSubarraySettings>(value); }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the exposure identifier, which is unique and can be used to identify the matching Image.
        /// </summary>
        public int ExposureId { get; set; }

        /// <summary>
        /// Gets or sets the exposure interval.
        /// </summary>
        public int ExposureInterval { get; set; }

        /// <summary>
        /// Gets or sets the number of non destructive readouts.
        /// </summary>
        public int NumberOfNonDestructiveReadouts { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public CameraRegion Region { get; set; }

        /// <summary>
        /// </summary>
        public byte FixedPatternNoiseReduction { get; set; }

        /// <summary>
        /// Gets or sets the automatic bias enabled.
        /// </summary>
        /// <value>
        /// The automatic bias enabled.
        /// </value>
        public bool AutoBiasEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [fullframe enabled].
        /// </summary>
        public bool FullframeEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [darkframe enabled].
        /// </summary>
        public bool DarkframeEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use default configuration].
        /// </summary>
        public bool UseDefaultConfiguration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [led1 enabled].
        /// </summary>
        public bool Led1Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [led2 enabled].
        /// </summary>
        public bool Led2Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [led3 enabled].
        /// </summary>
        public bool Led3Enabled { get; set; }

        /// <summary>
        /// Gets or sets the led on time.
        /// </summary>
        public int LedOnTime { get; set; }

        /// <summary>
        /// Gets or sets the led off time.
        /// </summary>
        public int LedOffTime { get; set; }

        /// <summary>
        /// Gets or sets the led flashes.
        /// </summary>
        public int LedFlashes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [shutter enabled].
        /// </summary>
        public bool ShutterEnabled { get; set; }

        /// <summary>
        /// Gets or sets the global inject time.
        /// </summary>
        public int GlobalInjectTime { get; set; }    

        /// <summary>
        /// Gets or sets the delay after global inject.
        /// </summary>
        public int DelayAfterGlobalInject { get; set; }

        /// <summary>
        /// Gets or sets the Pre/Post Exposure flags.
        /// </summary>
        public byte ExposureAttributes { get; set; }

        #region private fields and constants
        private IList<CameraSubarraySettings> m_subarrays;
        #endregion
    }
}