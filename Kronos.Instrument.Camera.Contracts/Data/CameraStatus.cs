// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary>
    /// Status and ThermoElectricCooler states from the SCM5821AX1 camera.
    /// </summary>
    public class CameraStatus
    {
        /// <summary>
        /// Status packet sequence number since Connect
        /// </summary>
        public double SequenceNumber { get; set; }

        /// <summary>
        /// The Image Sensor temperature in Celsius via on-chip diode, formerly known as DiodeTemp.
        /// </summary>
        public double ImageSensorTemperatureDiodeInC { get; set; }

        /// <summary>
        /// The Image Sensor temperature in Celsius via nearby thermistor, formerly known as TecTherm1v.
        /// </summary>
        public double ImageSensorTemperatureThermistorInC { get; set; }

        /// <summary>
        /// Legacy thermistor for analog TEC control, will be used as Future Humidity Sensor. Formerly known as TecTherm2v.
        /// </summary>
        public double RelativeHumidityInProcent { get; set; }

        /// <summary>
        /// The Image Sensor Interface board temperature in Celsius, formerly known as IsiTemp.
        /// </summary>
        public double ImageSensorBoardTemperatureInC { get; set; }

        /// <summary>
        /// The Camera Signal Processor board temperature in Celsius, formerly known as CspTemp.
        /// </summary>
        public double CameraSignalProcessorBoardTemperatureInC { get; set; }

        /// <summary>
        /// The temperature in Celsius near Thermo Electric Control Circuit, formerly known as PwrLocalTemp0.
        /// </summary>
        public double ThermoElectricControlCircuitTemperatureInC { get; set; }

        /// <summary>
        /// The temperature in Celsius near regulator for digital electronics, formerly known as PwrLocalTemp1.
        /// </summary>
        public double DigitalElectronicsRegulatorTemperatureInC { get; set; }

        /// <summary>
        /// The i.MX6 Rex ARM CPU temperature, formerly known as CpuTemp.
        /// </summary>
        public double ArmCpuTemperatureInC { get; set; }

        /// <summary>
        /// TODO[liverpool]
        /// </summary>
        public double TecVdiv2 { get; set; }

        /// <summary>
        /// A flag that enables and disables the ThermoElectricCooler, formerly known as TecEn.
        /// </summary>
        public int ThermoElectricCoolerEnabled { get; set; }

        /// <summary>
        /// The target temperature of the ThermoElectricCooler, formerly known as TecTTemp.
        /// </summary>
        public double ThermoElectricCoolerTemperatureInC { get; set; }

        /// <summary>
        /// The TEC control voltage 0 to 4.0 V half of TEC range.
        /// </summary>
        public double ThermoElectricCoolerCtlV { get; set; }

        /// <summary>
        /// The Camera Supply Volts min/typ/max 20/24/36* *for Space Station NanoRack.
        /// </summary>
        public double CameraV { get; set; }

        /// <summary>
        /// The Camera Supply Amps  0.48 TE off, 2.0 to 3.0 TE on..
        /// </summary>
        public double CameraA { get; set; }
		
        /// <summary>
        /// Information on the currently active interlocks; multiple can be active at the same time. 
		/// If the value is null, then it's not supported by the current firmware.
        /// </summary>
        public InterlockFlags? Interlocks { get; set; }

    }

    /// <summary>
    /// A definition of the possible interlock conditions preventing certain operations, like enabling the TEC.
    /// </summary>
    [System.Flags]
    public enum InterlockFlags
    {
        /// <summary>
        /// No interlocks active.
        /// </summary>
        None = 0,
        /// <summary>
        /// TEC can't be enabled because of the humidity being too high.
        /// </summary>
        RelativeHumidityTooHigh = 0x0001,
        /// <summary>
        /// TEC can't be enabled because of the image sensor board temperature being too high.
        /// </summary>
        ImageSensorBoardTemperatureTooHigh = 0x0002,
        /// <summary>
        /// TEC can't be enabled because of the digital electronics temperature being too high.
        /// </summary>
        ThermoElectricControlCircuitTemperatureTooHigh = 0x0004
    }

}