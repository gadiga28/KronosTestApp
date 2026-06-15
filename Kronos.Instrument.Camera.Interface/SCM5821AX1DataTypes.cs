using System;
using Thermo.Kronos.Instrument.Camera.Contracts.Data;

namespace Thermo.Kronos.Instrument.Camera.Interface
{
    /// <summary> These Data Types provide the data needed for communication to and from the SCM5821AX1 camera. </summary>
    public struct SCM5821AX1DataTypes
    {
        /// <summary> Status and TEC Cooler state from the SCM5821AX1 camera. </summary>
        [Serializable()]
        public struct StatusStructure
        {
            ///
            /// <summary>
            /// Status packet sequence number since Connect
            /// </summary>
            public float sequenceNumber;

            /// <summary>
            /// Image Sensor temperature (C) via on-chip diode
            /// </summary>
            public float diodeTemp;

            /// <summary>
            /// Image Sensor temperature (C) via nearby thermistor
            /// </summary>
            public float tecTherm1v;

            /// <summary>
            /// Legacy thermistor for analog TEC control. Future Humidity Sensor.
            /// </summary>
            public float tecTherm2v;

            /// <summary>
            /// Image Sensor Interface board temparatue (C)
            /// </summary>
            public float isiTemp;

            /// <summary>
            /// Camera Signal Processor board temparatue (C)
            /// </summary>
            public float cspTemp;

            /// <summary>
            /// Temparatue (C) near Thermo Electric Control Circuit
            /// </summary>
            public float pwrLocalTemp0;

            /// <summary>
            /// Temparature (C) near regulator for digital electronics
            /// </summary>
            public float pwrLocalTemp1;

            /// <summary>
            /// i.MX6 Rex ARM CPU temperature
            /// </summary>
            public float cpuTemp;

            /// <summary>
            /// Voltage across TE Cooler via divide by 2 resistors
            /// </summary>
            public float tecVdiv2;

            /// <summary>
            /// Flag that enables and disables the TEC cooler
            /// </summary>
            public UInt32 tecEn;

            /// <summary>
            /// Sets the target temperature of the TEC cooler
            /// </summary>
            public float tecTTemp;

            /// <summary>
            /// Sets TEC control voltage 0 to 4.0 V half of TEC range
            /// </summary>
            public float tecCtlV;

            /// <summary>
            /// Sets the Camera Supply Volts min/typ/max 20/24/36* *for Space Station NanoRack
            /// </summary>
            public float cameraV;

            /// <summary>
            /// Sets the Camera Supply Amps  0.48 TE off, 2.0 to 3.0 TE on
            /// </summary>
            public float cameraA;

            /// <summary>
            /// Sets the Firmwares interlocks status
            /// </summary>
            public Int32 interlocks;

        }

 
        /// <summary>User configured Voltages and Bias settings sent to the SCM5821AX1 camera. </summary>
        /// <remarks>Structure may not be available in final API</remarks>
        public struct ConfigurationStructure
        {
            /// <summary>
            /// Voltage structure for all avialble voltage readings from camera
            /// </summary>
            public Voltages voltages;

            /// <summary>
            /// Flag that enables and disables the TE Cooler
            /// </summary>
            public UInt32   tecEnabled;

            /// <summary>
            /// Target temperature to set the TE Cooler in C
            /// </summary>
            public UInt32   tecTargetTemp;

            /// <summary>
            /// Expected delay in exposure time created by opening the shutter
            /// </summary>
            public UInt32   openShutterSpeed;

            /// <summary>
            /// Expected delay in exposure time created by closeing the shutter
            /// </summary>
            public UInt32   closeShutterSpeed;
        }

        /// <summary>Structure to define a region(rectangle) on the imager using the (Xo,Yo) starting point. </summary>
        public struct RegionStructure
        {
            /// <summary>
            /// X pixel index of image sensor array
            /// </summary>
            public UInt16 Xo;

            /// <summary>
            /// Y pixel index of image sensor array
            /// </summary>
            public UInt16 Yo;

            /// <summary>
            /// Width of image from Xo in pixels
            /// </summary>
            public UInt16 dX;

            /// <summary>
            /// Height of image from Yo in pixels
            /// </summary>
            public UInt16 dY;
        }

        /// <summary>User configured Subarray(Region Of Interest) settings sent to the SCM5821AX1 camera. </summary>
        public struct SubarrayStructure
        {
            /// <summary>
            /// Unique string name for referencing subarray
            /// </summary>
            public string subarrayName;

            /// <summary>
            /// Index into subarray list
            /// </summary>
            public uint subarrayId;

            /// <summary>
            /// Pixel value threshold to trigger a sub-inject, if enabled
            /// </summary>
            public uint thresholdValuePercent;

            /// <summary>
            /// Interval between reads in microseconds
            /// </summary>   
            public uint readInterval;

            /// <summary>
            /// Interval between time resolved reads in microseconds
            /// </summary>
            public uint timeResolvedInterval;

            /// <summary>
            /// Interval between subarray clears
            /// </summary>
            public uint subInjectInterval;

            /// <summary>
            /// Power of 2 number of Non-Destructive Reads after shutter is closed
            /// </summary>
            public uint postreadNDROs;

            /// <summary>
            /// Fixed Pattern Noise Correction mode for each subarray
            /// </summary>
            public uint subarrayFPN;

            /// <summary>
            /// Region(rectangle) on the imager using the (Xo,Yo) starting point for a subarray
            /// </summary>
            public RegionStructure subarrayRegion;

            /// <summary>
            /// Region(ROI) on the imager using the (Xo,Yo) starting point for a threshold
            /// </summary>
            public RegionStructure thresholdRegion;

            /// <summary>
            /// Flag to enable the post exposure read
            /// </summary>
            public bool postreadEnabled;

            /// <summary>
            /// Flag to enable sub-inject fields, the interval between subarray clears
            /// </summary>
            public bool subInjEnabled;

            /// <summary>
            /// Flag to enable subarray reads
            /// </summary>
            public bool readEnabled;

            /// <summary>
            /// Flag to enable threshold (ROI), allows thresholdValue to influence decision to clear subarray region
            /// </summary>
            public bool thresholdEnabled;

            /// <summary>
            /// Flag to enable time resolved processing
            /// </summary>
            public bool timeResolvedEnabled;

            /// <summary>
            /// Flag that allows sub-inject interval to adapt to pixel value
            /// </summary>
            public bool adaptiveEnabled;

            /// <summary>
            /// padding for 32 bit alignment
            /// </summary>
            public bool subarrayIdSupplied;
            /// <summary>
            /// padding for 32 bit alignment
            /// </summary>
            public bool spare2;
        }

        /// <summary> Exposure settings sent to the SCM5821AX1 camera. </summary>
        public struct ExposureStructure
        {
            /// <summary>
            /// Unique identifier string to define an exposure
            /// </summary>
            public string exposureName;

            /// <summary>
            /// Exposure Identification Number
            /// </summary>
            public uint exposureId;

            /// <summary>
            /// Power-of-2 number of non-destructive reads after shutter closes
            /// </summary>
            public uint numberOfNDROs;

            /// <summary>
            /// Lenght of time the shutter is open, if shutter is enabled.
            /// </summary>
            public uint exposureInterval;

            /// <summary>
            /// Typically full frame, but can be reduced to save time.
            /// </summary>
            public RegionStructure exposureRegion;

            /// <summary>
            /// Fixed Pattern Noise Correction mode for the entire exposure
            /// </summary>
            public byte exposureFPN;

            /// <summary>
            /// Eanble the Auto Bias correction for the entire exposure
            /// </summary>
            public byte autoBiasEnabled;

            /// <summary>
            /// Byte used for Pre/Post Exposure settings
            /// </summary>
            public byte exposureAttributes;

            /// <summary>
            /// Byte used for 32 bit word alignment
            /// </summary>
            public byte spareByte2;

            /// <summary>
            /// LED sequencer off time in microseconds
            /// </summary>
            public uint ledOffTime;

            /// <summary>
            /// LED sequencer on time in microseconds
            /// </summary>
            public uint ledOnTime;

            /// <summary>
            /// The number of LED flashes between off and on time
            /// </summary>
            public uint ledFlashes;

            /// <summary>
            /// Enable/Disable LED 1
            /// </summary>
            public bool led1Enabled;

            /// <summary>
            /// Enable/Disable LED 2
            /// </summary>
            public bool led2Enabled;

            /// <summary>
            /// Enable/Disable LED 3
            /// </summary>
            public bool led3Enabled;

            /// <summary>
            /// Enable/Disable the shutters functionality
            /// </summary>
            public bool shutterEnabled;


            /// <summary>
            /// Read exposureRegion at end of an exposure if enabled
            /// </summary>
            public bool fullFrameEnabled;

            /// <summary>
            /// Capture a dark image for Fixed Pattern Noise correction if enabled
            /// </summary>
            public bool darkFrameEnabled;

            /// <summary>
            /// Use the cameras default configuration instead of relying on the operator defined fields
            /// </summary>
            public bool useDefaultConfiguration;

            /// <summary>
            /// The number of subarrays defined in a list for a given exposure
            /// </summary>
            public UInt16 numberOfSubarrays;
            public SubarrayStructure[] subarrayList;
        }

        /// <summary>Video(Signal) data structure used internally by host software mapped from the SCM5821AX1.CID821Image . </summary>
        public struct VideoStruct
        {
            public RegionStructure region;
            public UInt32 timeStampUs;
            public UInt32 initialTimeStampUs;
            public UInt32 numReads;
            public UInt32 fileVersion;
            public UInt32 tag;
            public UInt32 exposureNumber;
            public Thermo.Kronos.Instrument.Camera.Contracts.Enums.ImageType imageType;
            public Int32[] data;
        }   

    }
}
