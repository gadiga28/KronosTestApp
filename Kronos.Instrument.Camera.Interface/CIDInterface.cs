// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Renci.SshNet;
using Thermo.Kronos.Instrument.Camera.Contracts;
using Thermo.Kronos.Instrument.Camera.Contracts.Data;
using Thermo.Kronos.Instrument.Camera.Contracts.Enums;

namespace Thermo.Kronos.Instrument.Camera.Interface
{
    /// <summary>
    /// This class provides an implementation of the ICameraInterface.
    /// It is used as API between the camera communication on the Kronos Research App/Qtegra side and the SCM5821Ax1 side.
    /// HINT: API comments
    /// </summary>
    /// <seealso cref="ICIDInterface" />
    [Export(typeof(ICIDInterface))]
    public class CIDInterface : ICIDInterface
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CIDInterface" /> class.
        /// </summary>
        public CIDInterface()
        {
            m_scm5821Ax1 = new SCM5821AX1();
            m_scm5821Ax1.ReceivedStatus += Scm5821Ax1OnReceivedStatus;
            
            // this is the image receive method for Qtegra
            m_scm5821Ax1.ReceivedImage += OnReceivedImage;

            //TODO[liverpool]: this is the new image getting method from liverpool, clean up to one
            //m_scm5821Ax1.ReceivedImage += Scm5821Ax1OnReceivedImage;
            m_scm5821Ax1.ReceivedImageStream += Scm5821Ax1ReceiveImageStream;
            m_scm5821Ax1.ReceivedIntegratedImage += Scm5821Ax1ReceiveImageStream;
            m_scm5821Ax1.ReceivedLogData += Scm5821Ax1OnReceivedLogData;
            m_scm5821Ax1.ReceivedExposureComplete += Scm5821Ax1OnExposureComplete;
            m_scm5821Ax1.ReceivedCameraInfo += Scm5821Ax1OnReceivedCameraInfo;
            m_scm5821Ax1.ReceivedCameraVoltages += Scm5821Ax1OnReceivedVoltages;
            m_scm5821Ax1.ReceivedCameraConfig += Scm5821Ax1OnReceivedConfigData;
            //TODO[bill]: m_scm5821Ax1.ReceivedNotification += Scm5821Ax1OnReceivedNotification;

            m_cancellationTokenSource = new CancellationTokenSource();
        }

        #region ICIDInterface Members
        /// <summary>
        /// Gets or sets the received camera connection status.
        /// </summary>
        public Action<CameraConnectionStatus> ReceivedCameraConnectionStatus { get; set; }

        public Action<CameraInformation> ReceivedCameraInformation { get; set; }

        public Action<Voltages> ReceivedCameraVoltages { get; set; }

        public Action<CameraConfigData> ReceivedCameraConfig { get; set; }

        /// <summary>
        /// Get or sets the received notification.
        /// </summary>
        public Action<CameraNotificationTypes> ReceivedNotification { get; set; }


        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        public bool IsConnected()
        {
            return m_scm5821Ax1.IsConnected;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [development mode]. This will change the way images are received.
        /// </summary>
        public bool DevelopmentMode { get; set; }

        /// <summary>
        /// Occurs when an image was received.
        /// </summary>
        public Action<CameraExposureResult> ReceivedCameraExposureResult { get; set; }

        //TODO[liverpool]: this is the new image getting method from liverpool, clean up to one
        /// <summary>
        /// Occurs when [received image].
        /// </summary>
        public Action<Subarray> ReceivedImage { get; set; }

        //TODO[liverpool]: this is the new image getting method from liverpool, clean up to one
        /// <summary>
        /// Occurs when [received image].
        /// </summary>
        public Action<IEnumerable<Subarray>, CameraExposureSettings> ReceivedSubarrays { get; set; }

        //TODO[liverpool]: this is the new image getting method from liverpool, clean up to one
        /// <summary>
        /// Occurs when [received image].
        /// </summary>
        public Action<Subarray> ReceivedIntegratedImage { get; set; }

        /// <summary>
        /// Occurs when [received log data].
        /// </summary>
        public Action<LogEntry> ReceivedLogData { get; set; }

        /// <summary>
        /// Occurs when [received status].
        /// </summary>
        public Action<CameraStatus> ReceivedStatus { get; set; }

        /// <summary>
        /// Occurs when [received exposure complete].
        /// </summary>
        public Action<uint> ReceivedExposureComplete { get; set; }

        /// <summary>
        /// Gets or sets the received heartbeat.
        /// </summary>
        public Action<Heartbeat> ReceivedHeartbeat { get; set; }

        /// <summary>
        /// Gets or sets the state of the received.
        /// </summary>
        public Action<State> ReceivedState { get; set; }

        /// <summary>
        /// Aborts the exposure.
        /// If a user wants to abort the measurement process, this method should abort all underlying running methods on camera side.
        /// HINT: should not crash, but return a meaningful log entry
        /// HINT: do not throw an exception
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool AbortExposure()
        {
            if (!IsConnected()) return false;

            m_cancellationTokenSource.Cancel();

            m_scm5821Ax1.Abort();

            //TODO[liverpool]: removed by API update at 18-03-16
            // clear the image queue
            //m_scm5821Ax1.ClearImageQueue();

            return true;
        }

        /// <summary>
        /// Reboots the Linux OS/CPU board where the Firmware resides.
        /// Host is responsible for disconnecting interface just before reboot.
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public void RebootFirmware()
        {
            if (!IsConnected()) return;

            m_scm5821Ax1.SendReboot();
            m_scm5821Ax1.Disconnect();
       
        }

        /// <summary>
        /// Restart the Firmware on the CPU board.
        /// Host is responsible for disconnecting interface just before restarting.
        /// </summary>
        public void RestartFirmware()
        {
            if ((!IsConnected()) || (m_fwId < 190515))
                return;           

            m_scm5821Ax1.SendRestart();
            m_scm5821Ax1.Disconnect();        
        }

 
        /// <summary>
        /// Sends the log level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        public bool SendLogLevel(LogLevel logLevel)
        {
            if (!IsConnected()) return false;

            m_scm5821Ax1.SendLogLevel((int)logLevel);

            return true;
        }

        /// <summary>
        /// Sends a command from host to camera setting the target Log Level for the firmware.
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SendLogLevel(int logLevel)
        {
            m_scm5821Ax1.SendLogLevel(logLevel);
            return true;
        }

        /// <summary>
        /// Sends a command from host to camera setting the target Log Level for the firmware.
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SendSerialNumbers(CameraSerialNumbers serialNumbers)
        {
            m_scm5821Ax1.SendSerialNumbers(serialNumbers);
            return true;
        }


        /// <summary>
        /// Sets a new TCP/IP Address for the camera.
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SetIpAddress(string ipAddress)
        {
            bool success = false;

            if (!string.IsNullOrEmpty(ipAddress))
            {
                IPAddress ip;
                IPAddress.TryParse(ipAddress, out ip);

                if (ip != null)
                {
                    success = m_scm5821Ax1.SetNewIpAddress(ip.ToString());
                }
            }

            return success;
        }

        /// <summary>
        /// Sends a command for a voltage request
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SendVoltageRequest(string request)
        {
            m_scm5821Ax1.SendVoltagesRequest(request);
           // m_scm5821Ax1.SendConfigDataRequest(request);
            return true;
        }

        /// <summary>
        /// Sends a command for a voltage request
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SendConfigDataRequest(string request)
        {
            m_scm5821Ax1.SendConfigDataRequest(request);
            return true;
        }

        /// <summary>
        /// Sends a command for a FPGA file type for upload to camera
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SendUpadateFPGA()
        {
            m_scm5821Ax1.SendUpdateFPGA();
            return true;
        }

        /// <summary>
        /// Sends a command for a Linux/Board Support Package file type for upload to camera
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SendUpadateLinux()
        {
            m_scm5821Ax1.SendUpdateLinux();
            return true;
        }

        /// <summary>
        /// Sends a command for a firmware file type for upload to camera
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SendUpadateFirmware(string filename)
        {
            m_scm5821Ax1.SendUpdateFirmware(filename);
            return true;
        }

        /// <summary>
        /// Connects to the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="port">The port.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool Connect(string ipAddress, int port)
        {
            bool success = false;

            if (!string.IsNullOrEmpty(ipAddress))
            {
                IPAddress ip;
                IPAddress.TryParse(ipAddress, out ip);

                if (ip != null)
                {
                    m_ipAddress = ipAddress;

                    if (!IsConnected())
                    {
                        SendLogMessage(string.Format("INFO: Connect to Kronos Camera at {0}:{1}", ipAddress, port));
                        m_fwId = m_scm5821Ax1.Connect(ipAddress, port.ToString());
                        success = m_fwId > 0;
                    }
                    else
                    {
                        SendLogMessage(string.Format("INFO: Kronos Camera is already connected at {0}:{1}", ipAddress, port));
                    }
                }
                else
                {
                    SendLogMessage(string.Format("ERROR: IP address {0} is not valid.", ipAddress));
                }
            }
            
            //TODO: return meaningful status after response from camera is implemented
            return success;
        }

        /// <summary>
        /// Disables the ThermoElectricCooler.
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool DisableThermoElectricCooler()
        {
            if (IsConnected())
            {
                SendLogMessage("INFO: TEC is now disabled...");
                m_scm5821Ax1.SendTECEnable(0);
            }
            else
            {
                // log error message
                SendLogMessage("ERROR: TEC could not be disabled... Check if TEC is functioning properly.");
                return false;
            }

            //TODO: return meaningful status after response from camera is implemented
            return true;
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected())
            {
                SendLogMessage("INFO: Disconnect from Kronos Camera.");
                m_scm5821Ax1.Disconnect();

                m_suppressCameraTemperatureData = false;

                if (m_cancellationTokenSource != null)
                {
                    m_cancellationTokenSource.Cancel();
                }
            }
            else
            {
                SendLogMessage("WARNING: Kronos Camera could not be disconnected.");
            }
        }

        /// <summary>
        /// Gets information about the camera.
        /// </summary>
        /// <returns>
        /// Returns information about the connected camera.
        /// </returns>
        public bool GetCameraInformation()
        {
            bool result;
            if (IsConnected())
            {
                // Send request message to Camera firmware
                result = m_scm5821Ax1.GetCameraInformation();
            }
            else
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Enables the ThermoElectricCooler.
        /// </summary>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool EnableThermoElectricCooler()
        {
            if (IsConnected())
            {
                SendLogMessage("INFO: TEC is now enabled...");
                m_scm5821Ax1.SendTECEnable(1);
            }
            else
            {
                // log error message
                SendLogMessage("ERROR: TEC could not be disabled... Check if TEC is functioning properly.");
                return false;
            }

            // Return true if enabled
            return true;
        }


        /// <summary>
        /// Update the current connected camera with a new version of firmware (*.exe file)
        /// Secure FTP the file to the LINUX based camera
        /// </summary>
        /// <param name="file">The file path where the new firmware resides.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool UpdateFirmware(string file)
        {
            if (IsConnected() && !string.IsNullOrEmpty(m_ipAddress) && File.Exists(file))
            {
                SendLogMessage(string.Format("INFO: Camera firmware will be updated with file {0}", file));

                //TODO: the user name and password should be changed and/or encrypted
                ConnectionInfo connectionInfo = new ConnectionInfo(m_ipAddress, 22, "root", new PasswordAuthenticationMethod("root", "cidtec"));

                using (var sftpClient = new SftpClient(connectionInfo))
                {
                    sftpClient.Connect();

                    if (m_fwId < 190515)
                        // Change directory where file will be put(Uploaded)
                        sftpClient.ChangeDirectory("/root");
                    else
                        sftpClient.ChangeDirectory("/tmp");
                        
                    using (var uplfileStream = File.OpenRead(file))
                    {
                        string filename = Path.GetFileName(file);
                        sftpClient.UploadFile(uplfileStream, filename, true);
                    }

                    // repopulate the current version list with new file
                    sftpClient.Disconnect();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Overloaded method to Update the current connected camera with a new version of firmware
        /// Secure FTP the file to the LINUX based camera
        /// </summary>
        /// <param name="file">The folder\folder where the new firmware resides.</param>
        /// <param name="username">The username to log into the camera.</param>
        /// <param name="password">The user password to log into the camera.</param>
        /// <param name="hostAddress">The TCP/IP Address to log into the camera.</param>
        /// <returns>returns a bool indicating the success of the operation.</returns>
        //TODO: this should be cleared from cleartext passwords and adresses
        public bool UpdateFirmware(
            string file = "C:\\\\",
            string username = "root",
            string password = "cidtec",
            string hostAddress = "192.168.1.2")
        {
            if (IsConnected())
            {
                if ((username.Length <= 0) || (password.Length <= 0))
                {
                    return false;
                }

                using (var sftpClient = new SftpClient(hostAddress, 22, username, password))
                {
                    sftpClient.Connect();
                    if (m_fwId < 190515)
                        // Change directory where file will be put(Uploaded)
                        sftpClient.ChangeDirectory("/root");
                    else
                        sftpClient.ChangeDirectory("/tmp");               

                    using (var uplfileStream = File.OpenRead(file))
                    {
                        string filename = Path.GetFileName(file);

                        sftpClient.UploadFile(uplfileStream, filename, true);
                    }

                    // repopulate the current version list with new file
                    sftpClient.Disconnect();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Resets the exposure id for characterization testing.
        /// </summary>   
        public void ResetExposureId()
        {
            m_exposureId = 0;
        }

        /// <summary>
        /// Performs the exposure.
        /// </summary>
        /// <param name="cameraExposureSetting">The exposure settings.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool PerformExposure(CameraExposureSettings cameraExposureSetting)
        {
            //this should return false when not connected
            if (!IsConnected()) return false;

            m_cancellationTokenSource = new CancellationTokenSource();

            // count exposure id up to identify the exposures without needing the id to go through all instances
            cameraExposureSetting.ExposureId = (int)m_exposureId++;

            try
            {
                m_exposureData = new ExposureData.ExposureDataContainer
                {
                    exposureData =
                    {
                        exposureName = cameraExposureSetting.Name,
                        exposureId = (uint)cameraExposureSetting.ExposureId,
                        exposureInterval = (uint)cameraExposureSetting.ExposureInterval,
                        numberOfNDROs = (uint)cameraExposureSetting.NumberOfNonDestructiveReadouts,
                        exposureRegion =
                        {
                            Xo = (ushort)cameraExposureSetting.Region.PositionX,
                            Yo = (ushort)cameraExposureSetting.Region.PositionY,
                            dX = (ushort)cameraExposureSetting.Region.Width,
                            dY = (ushort)cameraExposureSetting.Region.Height
                        },
                        exposureFPN = cameraExposureSetting.FixedPatternNoiseReduction,
                        autoBiasEnabled = Convert.ToByte(cameraExposureSetting.AutoBiasEnabled),
                        fullFrameEnabled = cameraExposureSetting.FullframeEnabled,
                        darkFrameEnabled = cameraExposureSetting.DarkframeEnabled,
                        exposureAttributes = cameraExposureSetting.ExposureAttributes,
                        useDefaultConfiguration = cameraExposureSetting.UseDefaultConfiguration,
                        led1Enabled = cameraExposureSetting.Led1Enabled,
                        led2Enabled = cameraExposureSetting.Led2Enabled,
                        led3Enabled = cameraExposureSetting.Led3Enabled,
                        ledOnTime = (uint)cameraExposureSetting.LedOnTime,
                        ledOffTime = (uint)cameraExposureSetting.LedOffTime,
                        ledFlashes = (uint)cameraExposureSetting.LedFlashes,
                        shutterEnabled = cameraExposureSetting.ShutterEnabled,
                        numberOfSubarrays = (ushort)cameraExposureSetting.Subarrays.Count,
                        subarrayList = new SCM5821AX1DataTypes.SubarrayStructure[(ushort)cameraExposureSetting.Subarrays.Count]
                    }
                };

                for (int subarrayIndex = 0; subarrayIndex < cameraExposureSetting.Subarrays.Count; subarrayIndex++)
                {
                    m_exposureData.exposureData.subarrayList[subarrayIndex].subarrayName = cameraExposureSetting.Subarrays[subarrayIndex].Name;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].subarrayId = (uint)cameraExposureSetting.Subarrays[subarrayIndex].Id;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].subarrayIdSupplied = cameraExposureSetting.Subarrays[subarrayIndex].SubarrayIdSupplied;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].subarrayRegion.Xo = (ushort)cameraExposureSetting.Subarrays[subarrayIndex].Region.PositionX;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].subarrayRegion.Yo = (ushort)cameraExposureSetting.Subarrays[subarrayIndex].Region.PositionY;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].subarrayRegion.dX = (ushort)cameraExposureSetting.Subarrays[subarrayIndex].Region.Width;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].subarrayRegion.dY = (ushort)cameraExposureSetting.Subarrays[subarrayIndex].Region.Height;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].readEnabled = cameraExposureSetting.Subarrays[subarrayIndex].ReadEnabled;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].readInterval = (uint)cameraExposureSetting.Subarrays[subarrayIndex].ReadInterval;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].subInjEnabled = cameraExposureSetting.Subarrays[subarrayIndex].SubInjectEnabled;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].subInjectInterval = (uint)cameraExposureSetting.Subarrays[subarrayIndex].SubInjectInterval;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].postreadEnabled = cameraExposureSetting.Subarrays[subarrayIndex].PostReadEnabled;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].postreadNDROs = (uint)cameraExposureSetting.Subarrays[subarrayIndex].PostReadNonDestructiveReadouts;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].subarrayFPN = (uint)cameraExposureSetting.Subarrays[subarrayIndex].SubarrayFixedPatternNoiseReduction;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].thresholdEnabled = cameraExposureSetting.Subarrays[subarrayIndex].TresholdEnabled;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].thresholdValuePercent = (uint)cameraExposureSetting.Subarrays[subarrayIndex].TresholdValuePercentage;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].thresholdRegion.Xo = (ushort)cameraExposureSetting.Subarrays[subarrayIndex].TresholdRegion.PositionX;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].thresholdRegion.Yo = (ushort)cameraExposureSetting.Subarrays[subarrayIndex].TresholdRegion.PositionY;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].thresholdRegion.dX = (ushort)cameraExposureSetting.Subarrays[subarrayIndex].TresholdRegion.Width;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].thresholdRegion.dY = (ushort)cameraExposureSetting.Subarrays[subarrayIndex].TresholdRegion.Height;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].timeResolvedEnabled = cameraExposureSetting.Subarrays[subarrayIndex].TimeResolvedEnabled;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].timeResolvedInterval = (uint)cameraExposureSetting.Subarrays[subarrayIndex].TimeResolvedInterval;
                    m_exposureData.exposureData.subarrayList[subarrayIndex].adaptiveEnabled = cameraExposureSetting.Subarrays[subarrayIndex].AdaptiveEnabled;
                }

                if (!m_awaitingExposures.Contains(cameraExposureSetting))
                {
                    m_awaitingExposures.Add(cameraExposureSetting);
                }

                if (cameraExposureSetting.GlobalInjectTime > 0)
                {
                    m_scm5821Ax1.SendGlobalInject((uint)cameraExposureSetting.GlobalInjectTime);
                    SendLogMessage(string.Format("INFO: Apply Global Inject for {0}ms.", cameraExposureSetting.GlobalInjectTime));
                }

                SendLogMessage(string.Format("INFO: Apply delay after Global Inject for {0}ms.", cameraExposureSetting.DelayAfterGlobalInject));
                m_scm5821Ax1.SendDelay((uint)cameraExposureSetting.DelayAfterGlobalInject);
                
                    SendLogMessage("INFO: Send exposure settings to camera and waiting for response...");
                m_scm5821Ax1.SendExposureSettings(m_exposureData);

                m_exposureSettings = cameraExposureSetting;

                return true;
            }
            catch (Exception e)
            {
                SendLogMessage(string.Format("ERROR: Something went wrong during sending the exposure settings: {0}", e));
                return false;
            }
        }

        /// <summary>
        ///  Deprecated method
        /// </summary>
        /// <param name="configurationData"></param>
        public void ProcessCameraConfigurationData(SCM5821AX1DataTypes.ConfigurationStructure configurationData)
        {
            m_scm5821Ax1.SendCameraConfiguration(configurationData);
        }

        /// <summary>
        /// Sets the ThermoElectricCooler temperature.
        /// </summary>
        /// <param name="temperatureCelsius">The temperature in degree Celsius.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SetThermoElectricCoolerTemperature(float temperatureCelsius)
        {
            if (IsConnected())
            {
                SendLogMessage(string.Format("INFO: TEC is now set to {0} degree Celsius.", temperatureCelsius));
                m_scm5821Ax1.SendTargetTemp(temperatureCelsius);
            }
            else
            {
                SendLogMessage(string.Format("ERROR: TEC temperature could not be set to {0} C... Check if TEC is functioning properly.", temperatureCelsius));
                //return false if temperature is not enabled
                return false;
            }

            //return status after response from camera is implemented
            return true;
        }
        #endregion

        private void Scm5821Ax1OnExposureComplete(uint exposureId)
        {
            SendLogMessage(string.Format("INFO: Received ExposureCompleteEvent with id={0}", exposureId));

            if (ReceivedExposureComplete != null)
            {
                ReceivedExposureComplete.Invoke(exposureId);
            }
            /* Getting a null reference error on ReceivedCameraExposureResult
                        List<CameraExposureSettings> toDelete = new List<CameraExposureSettings>();

                        CameraExposureSettings cameraExposureSettings = m_awaitingExposures.FirstOrDefault(x => x.ExposureId == exposureId);

                        if (cameraExposureSettings != null)
                        {
                            IEnumerable<CameraExposureResult> waitingSubarrays = m_waitingCameraExposureResults.Where(x => x.ExposureTag == exposureId).ToList();

                            CameraExposureResult received = waitingSubarrays.FirstOrDefault(x => x.ImageType.Equals(ImageType.Integrated) || x.ImageType.Equals(ImageType.FullFrame));
                
                            if (received != null)
                            {
                                received.CameraExposureSettings = cameraExposureSettings;

                                ReceivedCameraExposureResult(received);

                                SendLogMessage(string.Format("INFO: Received Exposure with ID={0}, Type={1} sent to data system.", exposureId, received.ImageType.ToString()));

                                toDelete.Add(cameraExposureSettings);
                                m_waitingCameraExposureResults = m_waitingCameraExposureResults.Where(x => x.ExposureTag >= exposureId).ToList();
                            }
                        }

                        foreach (CameraExposureSettings delete in toDelete)
                        {
                            m_awaitingExposures.Remove(delete);
                        }
             */
        }

        private void Scm5821Ax1OnReceivedCameraInfo(CameraInformation cameraInfo)
        {
            cameraInfo.CameraApiVersion = Constants.ApiVersion;
            ReceivedCameraInformation.Invoke(cameraInfo);
        }

        private void Scm5821Ax1OnReceivedVoltages(Voltages cameraVoltages)
        {
            ReceivedCameraVoltages.Invoke(cameraVoltages);
        }

        private void Scm5821Ax1OnReceivedConfigData(CameraConfigData cameraConfigData)
        {
            ReceivedCameraConfig.Invoke(cameraConfigData);
        }

        private void Scm5821Ax1OnReceivedLogData(string logData)
        {
            SendLogMessage(logData);

            if (ReceivedState != null)
            {
                if (logData.ToUpperInvariant().StartsWith("ERROR:"))
                {
                    // some error occurred, throw event to cancel image receiving, this should be changed later
                    ReceivedState(new State { StateLevel = StateLevel.Error, Message = string.Format("Received ERROR from camera. {0}", logData) });
                }

                if (logData.ToUpperInvariant().StartsWith("WARNING:"))
                {
                    // some error occurred, throw event to cancel image receiving, this should be changed later
                    ReceivedState(new State { StateLevel = StateLevel.Warning, Message = string.Format("Received WARNING from camera. {0}", logData) });
                }
            }
        }

        private void SendLogMessage(string logData)
        {
            if (ReceivedLogData != null)
            {
                LogEntry logEntry = new LogEntry(LogLevel.Warning, string.Format("Undefined LogLevel from Camera: {0}", logData));

                if (logData.ToUpperInvariant().StartsWith("INFO: TEMPERATURES:")
                    || logData.ToUpperInvariant().StartsWith("INFO: TEMPS:")
                    || logData.ToUpperInvariant().StartsWith("DEBUG: TEC:"))
                {
                    if (m_suppressCameraTemperatureData)
                    {
                        // we suppress the temperature info from the camera
                        return;
                    }

                    logEntry = new LogEntry(LogLevel.Info, "Suppress camera temperature logging...");
                    m_suppressCameraTemperatureData = true;
                }
                else if (logData.ToUpperInvariant().StartsWith("INFO:"))
                {
                    logEntry = new LogEntry(LogLevel.Info, logData);
                }
                else if (logData.ToUpperInvariant().StartsWith("ERROR:"))
                {
                    logEntry = new LogEntry(LogLevel.Error, logData);
                }
                else if (logData.ToUpperInvariant().StartsWith("WARNING:"))
                {
                    logEntry = new LogEntry(LogLevel.Warning, logData);
                }
                else if (logData.ToUpperInvariant().StartsWith("DEBUG:"))
                {
                    logEntry = new LogEntry(LogLevel.Debug, logData);
                }
                else if (logData.ToUpperInvariant().StartsWith("FATAL:"))
                {
                    logEntry = new LogEntry(LogLevel.Fatal, logData);
                }
                // these 2 are special for not flagged firmware messages TODO: flag correct in firmware
                else if (logData.StartsWith("Kronos"))
                {
                    logEntry = new LogEntry(LogLevel.Debug, logData);
                }
                else if (logData.StartsWith("CommandCode"))
                {
                    logEntry = new LogEntry(LogLevel.Debug, logData);
                }
                ReceivedLogData.Invoke(logEntry);
            }
        }

        private void OnReceivedImage()
        {
            if (m_cancellationTokenSource.IsCancellationRequested) return;

            SCM5821AX1.CID821Image cid821Image;

            m_scm5821Ax1.TryTakeImage(out cid821Image);

            int[,] rawData = new int[Constants.SensorDimensions, Constants.SensorDimensions];

            int index = 0;
            for (int y = 0; y < cid821Image.dy; y++)
            {
                for (int x = 0; x < cid821Image.dx; x++)
                {
                    rawData[x, y] = cid821Image.data[index];
                    index++;
                }
            }

            CameraExposureResult cameraExposureResult = new CameraExposureResult
            {
                InitialTimestamp = (int)cid821Image.initialTimestamp,
                ReadTimestamp = (int)cid821Image.timestampRead,
                ExposureTag = (int)cid821Image.exposureTag,
                ImageType = (ImageType)cid821Image.imageType,
                Tag = (short)cid821Image.subarrayTag,
                StartColumn = (short)cid821Image.x0,
                OffsetColumn = (short)cid821Image.dx,
                StartRow = (short)cid821Image.y0,
                OffsetRow = (short)cid821Image.dy,
                NumberOfNonDestructiveReads = (short)cid821Image.ndros,
                RawData = rawData
            };

            SendLogMessage(string.Format("INFO: Received CameraExposureResult: ExposureTag={0}, SubarrayTag={1}, ImageType={2}, InitialTimeStamp={3}, ReadTimeStamp={4}",
                cameraExposureResult.ExposureTag,
                cameraExposureResult.Tag,
                cameraExposureResult.ImageType,
                cameraExposureResult.InitialTimestamp,
                cameraExposureResult.ReadTimestamp));

// possible memory leak
            //if (DevelopmentMode)
            //{
            //    cameraExposureResult.CameraExposureSettings = m_exposureSettings;
            //    ReceivedCameraExposureResult(cameraExposureResult);
            //}
            //else
            //{
            //    m_waitingCameraExposureResults.Add(cameraExposureResult);
            //}
        }

        //TODO[liverpool]: this is the an image getting method from liverpool, clean up to one
        private void Scm5821Ax1OnReceivedImage()
        {
            if (m_cancellationTokenSource.IsCancellationRequested) return;

            SCM5821AX1.CID821Image cid821Image;
            bool tryTakeImage = m_scm5821Ax1.TryTakeImage(out cid821Image);

            if (tryTakeImage)
            {
                lock (m_lockObject)
                {
                    ImageType imageType = (ImageType)cid821Image.imageType;
                    Subarray subarray = new Subarray
                    {
                        InitialTimestamp = (int)cid821Image.initialTimestamp,
                        ReadTimestamp = (int)cid821Image.timestampRead,
                        ExposureTag = (int)cid821Image.exposureTag,
                        ImageType = imageType,
                        Tag = (short)cid821Image.subarrayTag,
                        StartColumn = (short)cid821Image.x0,
                        OffsetColumn = (short)cid821Image.dx,
                        StartRow = (short)cid821Image.y0,
                        OffsetRow = (short)cid821Image.dy,
                        NumberOfNonDestructiveReads = (short)cid821Image.ndros,
                    };

                    subarray.RawData = new int[subarray.OffsetColumn, subarray.OffsetRow];

                    int[] imageData = new int[cid821Image.data.Length];

                    for (int index = 0; index < cid821Image.data.Length; index++)
                    {
                        imageData[index] = cid821Image.data[index];
                    }

                    Subarray mappedCameraImage = MapSubarray(subarray, imageData);

                    m_waitingSubarrays.Add(mappedCameraImage);

                    List<CameraExposureSettings> toDelete = new List<CameraExposureSettings>();

                    foreach (CameraExposureSettings cameraExposureSettings in m_awaitingExposures)
                    {
                        int subarraysCount = cameraExposureSettings.Subarrays.Count;

                        IEnumerable<Subarray> waitingSubarrays = m_waitingSubarrays.Where(x => x.ExposureTag == cameraExposureSettings.ExposureId).ToList();

                        if (subarraysCount == waitingSubarrays.Count() || cameraExposureSettings.FullframeEnabled || mappedCameraImage.ImageType == ImageType.FullFrame)
                        {
                            ReceivedSubarrays(waitingSubarrays, cameraExposureSettings);

                            toDelete.Add(cameraExposureSettings);

                            m_waitingSubarrays = m_waitingSubarrays.Where(x => x.ExposureTag <= cameraExposureSettings.ExposureId).ToList();
                        }
                    }

                    foreach (CameraExposureSettings delete in toDelete)
                    {
                        m_awaitingExposures.Remove(delete);
                    }
                }
            }
        }

        //TODO[liverpool]: this is an image getting method from liverpool, clean up to one
        private Subarray MapSubarray(Subarray subarray, int[] newImageData)
        {
            int index = 0;
            for (int y = 0; y < subarray.OffsetRow; y++)
            {
                for (int x = 0; x < subarray.OffsetColumn; x++)
                {
                    subarray.RawData[x, y] = newImageData[index++];
                }
            }

            return subarray;
        }

        //TODO[liverpool]: this is an image getting method from liverpool, clean up to one
        // When processing requires a thousand frames/images per second or more, stream interface
        private void Scm5821Ax1ReceiveImageStream()
        {
            SCM5821AX1.CID821Image cid821Image = new SCM5821AX1.CID821Image();

            bool tryTakeImage = m_scm5821Ax1.TryTakeImage(out cid821Image);

            Int32[,] rawData = new Int32[Constants.SensorDimensions, Constants.SensorDimensions];

            int i = 0;

            for (int y = cid821Image.y0; y < cid821Image.dy + cid821Image.y0; y++)
            {
                for (int x = cid821Image.x0; x < cid821Image.dx + cid821Image.x0; x++)
                {
                    rawData[x, y] = cid821Image.data[i];
                    i++;
                }
            }

            Subarray subarray = new Subarray
            {
                InitialTimestamp = (int)cid821Image.initialTimestamp,
                ReadTimestamp = (int)cid821Image.timestampRead,
                ExposureTag = (int)cid821Image.exposureTag,
                ImageType = (ImageType)cid821Image.imageType,
                Tag = (short)cid821Image.subarrayTag,
                StartColumn = (short)cid821Image.x0,
                OffsetColumn = (short)cid821Image.dx,
                StartRow = (short)cid821Image.y0,
                OffsetRow = (short)cid821Image.dy,
                // Firmware always returns 1 for Exposure NDRO
                NumberOfNonDestructiveReads = 1,
                NumberOfSubinjects = (int)cid821Image.subinjects,
                PixelSaturated = (short)cid821Image.saturation,
                RawData = rawData
            };

            if (subarray.ImageType == ImageType.Integrated)
            {
                if (ReceivedIntegratedImage != null)
                {
                    ReceivedIntegratedImage(subarray);
                }
            }
            else
            {
                if (ReceivedImage != null)
                {
                    ReceivedImage(subarray);
                }
            }
        }

        private void Scm5821Ax1OnReceivedStatus(SCM5821AX1DataTypes.StatusStructure status)
        {
            CameraStatus cameraStatus = new CameraStatus
            {
                SequenceNumber = status.sequenceNumber,
                ImageSensorTemperatureDiodeInC = status.diodeTemp,
                ImageSensorTemperatureThermistorInC = status.tecTherm1v,
                RelativeHumidityInProcent = status.tecTherm2v,
                ImageSensorBoardTemperatureInC = status.isiTemp,
                CameraSignalProcessorBoardTemperatureInC = status.cspTemp,
                ThermoElectricControlCircuitTemperatureInC = status.pwrLocalTemp0,
                DigitalElectronicsRegulatorTemperatureInC = status.pwrLocalTemp1,
                ArmCpuTemperatureInC = status.cpuTemp,
                TecVdiv2 = status.tecVdiv2,
                ThermoElectricCoolerEnabled = (int)status.tecEn,
                ThermoElectricCoolerTemperatureInC = status.tecTTemp,
                ThermoElectricCoolerCtlV = status.tecCtlV,
                CameraV = status.cameraV,
                CameraA = status.cameraA,
                Interlocks = (InterlockFlags)status.interlocks
            };

            ReceivedStatus(cameraStatus);

            // this is used as a heartbeat from the camera, see documentation of camera
            if (ReceivedHeartbeat != null)
            {
                ReceivedHeartbeat.Invoke(new Heartbeat { Timestamp = DateTime.UtcNow });
            }
        }

        private void Scm5821Ax1OnReceivedNotification(CameraNotificationTypes notification)
        {
            switch (notification)
            {
                case CameraNotificationTypes.AutoBiasDoesNotWorkProperly:
                    ReceivedNotification?.Invoke(notification);
                    break;
                default:
                    // This should not occur and indicates a mismatch between api and firmware
                    SendLogMessage($"{LogLevel.Error.ToString().ToUpperInvariant()}: The notification {notification} is not supported.");
                    break;
            }
        }

        public async Task SendHeartbeatAsync(CancellationTokenSource cancellationTokenSource)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                // send heartbeat every second
                await Task.Delay(1000, cancellationTokenSource.Token);
                SCM5821AX1.CommandCode code = m_scm5821Ax1.SendHeartbeat();
                if ((code & SCM5821AX1.CommandCode.StatusMask) != SCM5821AX1.CommandCode.OK)          
                {
                   // if no CommandCode.Ok response, something went wrong wait for some time and then try to reconnect
                   // Trace.TraceWarning(" Received no Ok after sending heartbeat.");
                   //TODO[dko]: disconnect gracefully and try to reconnect after some tries...
                    Disconnect();
                    break;
                }
            }
        }

        public void SetShutterOpenDelay(UInt32 delay)
        {
            m_scm5821Ax1.SetShutterOpenDelay(delay);
        }

        public void SetShutterCloseDelay(UInt32 delay)
        {
            m_scm5821Ax1.SetShutterCloseDelay(delay);
        }

        public void SetCalibrateThresholds()
        {
            m_scm5821Ax1.SetCalibrateThresholds();
        }

        public void SetFlashToPercent()
        {
            m_scm5821Ax1.SetFlashToPercent();
        }

        public void SetFlashToAdu()
        {
            m_scm5821Ax1.SetFlashToAdu();
        }

        public void SetCalibrateShutter()
        {
            m_scm5821Ax1.SetCalibrateShutter();
        }

        /// <summary>
        /// Sends a command from host to camera setting the emulation mode for the firmware.
        /// </summary>
        /// <param name="parameter">The emulation parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SetEmulateMETS(UInt32 parameter)
        {
            if (m_fwId >= 200212)
            {
                m_scm5821Ax1.SetEmulateMETS(parameter);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Sends a command from host to camera setting the emulation mode for the firmware.
        /// </summary>
        /// <param name="parameter">The emulation parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SetEmulateScale(UInt32 parameter)
        {
            if (m_fwId >= 200212)
            {
                m_scm5821Ax1.SetEmulateScale(parameter);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Sends a command from host to camera setting the emulation mode for the firmware.
        /// </summary>
        /// <param name="parameter">The emulation parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SetEmulateOffset(UInt32 parameter)
        {
            if (m_fwId >= 200212)
            {
                m_scm5821Ax1.SetEmulateOffset(parameter);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Sends a command from host to camera setting the emulation mode for the firmware.
        /// </summary>
        /// <param name="parameter">The emulation parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SetEmulateNonlinear(UInt32 parameter)
        {
            if (m_fwId >= 200212)
            {
                m_scm5821Ax1.SetEmulateNonlinear(parameter);
                return true;
            }
            else
                return false;
        }

        public void SetTestImagerControl(UInt32 imagerControl)
        {
            m_scm5821Ax1.SetTestImagerControl(imagerControl);
        }

        public void SetCalibrateColumnGains()
        {
            m_scm5821Ax1.SetCalibrateColumnGains();
        }

        public void SetCorrectDeadpixels(UInt32 control)
        {
            m_scm5821Ax1.SetCorrectDeadpixels(control);
        }

        public void SetDarkPixelThreshold(UInt32 thresholdPercent)
        {
            m_scm5821Ax1.SetDarkPixelThreshold(thresholdPercent);
        }

        /// <summary>
        /// Test feature to apply TEC Control voltage in 1.6 to 4.8 V range.
        /// </summary>
        /// <param name="parameter">The voltage parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SetTecCtlVDiv2(float parameter)
        {
            if (m_fwId >= 200212)
            {
                m_scm5821Ax1.SetTecCtlVDiv2(parameter);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Sends a command from host to camera setting 1 to 100, causes 2x step in V across TEC.
        /// </summary>
        /// <param name="parameter">The step parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SetTecCtlStepmV(UInt32 parameter)
        {
            if (m_fwId >= 200212)
            {
                m_scm5821Ax1.SetTecCtlStepmV(parameter);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Sends a command from host to camera setting the RH Limit for the firmware.
        /// </summary>
        /// <param name="parameter">The emulation parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SetRHLimit(UInt32 parameter)
        {
            if (m_fwId >= 200212)
            {
                m_scm5821Ax1.SetRHLimit(parameter);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Sends a command from host to camera getting the TEC Time constant form the firmware.
        /// </summary>  
        /// <returns>returns a time indicating the success of the operation.</returns>
        public int GetTecTimeConstant()
        {      
            var time = m_scm5821Ax1.GetTecTimeConstant();
            return time;
        }

        /// <summary>
        /// Sends a command from host to camera setting the TEC Time constant for the firmware.
        /// </summary>
        /// <param name="parameter">The time parameter in seconds.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SetTecTimeConstant(UInt32 parameter)
        {     
            m_scm5821Ax1.SetTecTimeConstant(parameter);
            return true;  
        }

        public void SetUpdatedEEProm()
        {
            m_scm5821Ax1.SetUpdatedEEProm();
        }

        public void SetPixelResponse(UInt32 parameter)
        {
            m_scm5821Ax1.SetPixelResponse(parameter);
        }

        /// <summary>
        /// Sends a command from host to camera setting the Temperature Limit for the firmware.
        /// </summary>
        /// <param name="parameter">The temperature parameter s.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public bool SetCameraTempLimit(UInt32 parameter)
        {
            m_scm5821Ax1.SetCameraTempLimit(parameter);
            return true;
        }

        public void SetFactoryDefaults()
        {
            m_scm5821Ax1.SetFactoryDefaults();
        }


        #region private fields and constants
        private readonly IList<CameraExposureSettings> m_awaitingExposures = new List<CameraExposureSettings>();
        private readonly SCM5821AX1 m_scm5821Ax1;
        private IList<CameraExposureResult> m_waitingCameraExposureResults = new List<CameraExposureResult>();
        private IList<Subarray> m_waitingSubarrays = new List<Subarray>();
        private ExposureData.ExposureDataContainer m_exposureData;
        private int m_fwId;
        private CancellationTokenSource m_cancellationTokenSource;
        private uint m_exposureId;
        private CameraExposureSettings m_exposureSettings;
        private string m_ipAddress;
        private readonly object m_lockObject = new object();
        private bool m_suppressCameraTemperatureData;
        #endregion
    }
}