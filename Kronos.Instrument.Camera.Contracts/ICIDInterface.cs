// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Thermo.Kronos.Instrument.Camera.Contracts.Data;
using Thermo.Kronos.Instrument.Camera.Contracts.Enums;

namespace Thermo.Kronos.Instrument.Camera.Contracts
{
    /// <summary>
    /// This is the application programming interface (API) of a Liverpool CID camera.
    /// HINT: API version 9 (count up if breaking changes occur)
    /// HINT: If this is working properly, consider the usage of asynchronous operators.
    /// </summary>
    public interface ICIDInterface
    {
        /// <summary>
        /// Occurs when camera status information was received.
        /// </summary>
        Action<CameraStatus> ReceivedStatus { get; set; }

        /// <summary>
        /// Occurs when the camera or the underlying controller class issued a log entry.
        /// </summary>
        Action<LogEntry> ReceivedLogData { get; set; }

        /// <summary>
        /// Occurs when an image was received.
        /// </summary>
        Action<IEnumerable<Subarray>, CameraExposureSettings> ReceivedSubarrays { get; set; }

        /// <summary>
        /// Gets or sets the received exposure complete.
        /// </summary>
        Action<Subarray> ReceivedImage { get; set; }

        /// <summary>
        /// Gets or sets the received camera connection status.
        /// </summary>
        Action<CameraConnectionStatus> ReceivedCameraConnectionStatus { get; set; }

        /// <summary>
        /// Gets or sets the received camera information.
        /// </summary>
        Action<CameraInformation> ReceivedCameraInformation { get; set; }

        /// <summary>
        /// Gets or sets the received camera voltages.
        /// </summary>
        Action<Voltages> ReceivedCameraVoltages { get; set; }

        /// <summary>
        /// Gets or sets the received camera config and voltages from the firmware.
        /// </summary>
        Action<CameraConfigData> ReceivedCameraConfig { get; set; }

        /// <summary>
        /// Gets or sets the received heartbeat.
        /// </summary>
        Action<Heartbeat> ReceivedHeartbeat { get; set; }

        /// <summary>
        /// Gets or sets the state of the received.
        /// </summary>
        Action<State> ReceivedState { get; set; }

        /// <summary>
        /// Gets or sets the received notification.
        /// </summary>
        Action<CameraNotificationTypes> ReceivedNotification { get; set; }

        /// <summary>
        /// Gets a value indicating whether the camera is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if the camera is connected; otherwise, <c>false</c>.
        /// </value>
        bool IsConnected();

        /// <summary>
        /// Gets or sets a value indicating whether [development mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [development mode]; otherwise, <c>false</c>.
        /// </value>
        bool DevelopmentMode { get; set; }

        /// <summary>
        /// This attempts to connects to the camera on the specified IP address.
        /// HINT: Should only allow one connection.
        /// HINT: Should not deadlock, but return a meaningful log entry.
        /// HINT: Manage a safe and self maintained connection in the underlying controller class.
        /// HINT: Do not throw an exception.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="port">The port.</param>
        /// <returns><c>true</c>, when the camera connected successfully; otherwise <c>false</c>.</returns>
        bool Connect(string ipAddress, int port);

        /// <summary>
        /// Disconnects from the camera if currently connected.
        /// HINT: Calling <c>Disconnect</c> whilst already disconnected, should have no effect and should not cause an error, but should log an entry about this.
        /// HINT: Should not deadlock, but return a meaningful log entry.
        /// HINT: Do not throw an exception.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Gets information about the camera such as serial numbers and firmware versions.
        /// </summary>
        /// <returns>Returns true for success of the message request sent.</returns>
        bool GetCameraInformation();

        /// <summary>
        /// Update the current connected camera with a new version of firmware (*.exe file)
        /// Secure FTP the file to the LINUX based camera
        /// </summary>
        /// <param name="file">The folder/filename.exe where the new firmware resides.</param>
        /// <param name="username">The username to log into the camera.</param>
        /// <param name="password">The user password to log into the camera.</param>
        /// <param name="hostAddress">The TCP/IP Address to log into the camera.</param>
        /// <returns>returns a bool indicating the success of the operation.</returns>
        bool UpdateFirmware(string file, string username, string password, string hostAddress);


        /// <summary>
        /// Reboots the Linux OS/CPU board where the Firmware resides. 
        /// Used after a Firmware Update or Set IP Address command.
        /// </summary>    
        void RebootFirmware();

        /// <summary>
        /// Restart the Firmware like a warmstart. 
        /// Used after a Fimrware upload command.
        /// </summary>    
        void RestartFirmware();

        /// <summary>
        /// Sends the log level to the camera.
        /// </summary>
        bool SendLogLevel(LogLevel logLevel);

        /// <summary>
        /// Sets a new TCP/IP Address for the camera.
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        bool SetIpAddress(string ipAddress);

        /// <summary>
        /// Sends the voltage request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        bool SendVoltageRequest(string request);

        /// <summary>
        /// Sends the Configuration data request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        bool SendConfigDataRequest(string request);

        /// <summary>
        /// Sends the Update Firmware message directy after the new file is FTP'ed
        /// </summary>
        /// <returns>Returns a boolean indicating the success of the operation</returns>
        bool SendUpadateFirmware(string filename);

        /// <summary>
        /// Sends the Update FPGA message directy after the new file is FTP'ed
        /// </summary>
        /// <returns>Returns a boolean indicating the success of the operation</returns>
        bool SendUpadateFPGA();

        /// <summary>
        /// Sends the Update Linux message directy after the new file is FTP'ed
        /// </summary>
        /// <returns>Returns a boolean indicating the success of the operation</returns>
        bool SendUpadateLinux();

        /// <summary>
        /// Sends the Set Serial Numbers message to the camera.
        /// </summary>
        /// <param name="serialNumbers">string.</param>
        /// <returns></returns>
        bool SendSerialNumbers(CameraSerialNumbers serialNumbers);

        /// <summary>
        /// This requests the camera to perform an exposure.
        /// </summary>
        /// <param name="cameraExposureSetting">The camera exposure setting.</param>
        /// <returns>
        ///   <c>true</c>, when the request to perform an exposure was successful; otherwise, <c>false</c>.
        /// </returns>
        /// HINT: Should not deadlock, but return a meaningful log entry.
        /// HINT: Do not throw an exception.
        bool PerformExposure(CameraExposureSettings cameraExposureSetting);

        /// <summary>
        /// Aborts any currently active exposure.
        /// A meaningful log message should be issued during this call, also when no exposure was currently active.
        /// HINT: Should not deadlock, but return a meaningful log entry.
        /// HINT: Do not throw an exception.
        /// </summary>
        /// <returns><c>true</c>, when an active exposure was aborted, or when there was no active exposure in process; otherwise, <c>false</c>.</returns>
        bool AbortExposure();

        /// <summary>
        /// Enables the active control of the thermoelectric cooler.
        /// </summary>
        /// HINT: Should not deadlock, but return a meaningful log entry.
        /// HINT: Do not throw an exception.
        /// HINT: Issue a meaningful log message when an error occurred.
        /// <returns><c>true</c>, when the control loop of the thermoelectric cooler was activated successfully; otherwise, <c>false</c>.</returns>
        bool EnableThermoElectricCooler();

        /// <summary>
        /// Disables the active control of the thermoelectric cooler.
        /// </summary>
        /// HINT: Should not deadlock, but return a meaningful log entry.
        /// HINT: Do not throw an exception.
        /// HINT: Issue a meaningful log message when an error occurred.
        /// <returns><c>true</c>, when the control loop of the thermoelectric cooler was deactivated successfully; otherwise, <c>false</c>.</returns>
        bool DisableThermoElectricCooler();

        /// <summary>
        /// Sets the control loop's temperature set point of the thermoelectric cooler.
        /// The set point value should always be accepted, even when the thermoelectric cooler is disabled. When enabled thereafter, the last specified set point shall be used.
        /// </summary>
        /// HINT: Should not deadlock, but return a meaningful log entry.
        /// HINT: Do not throw an exception.
        /// <param name="temperatureCelsius">The set point temperature in degrees Celsius.</param>
        /// <returns><c>true</c>, when set point temperature was successfully set; otherwise, <c>false</c>.</returns>
        bool SetThermoElectricCoolerTemperature(float temperatureCelsius);
    }
}