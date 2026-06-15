using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Thermo.Kronos.Instrument.Camera.Contracts.Data;

namespace Thermo.Kronos.Instrument.Camera.Interface
{
    /// <summary> Provide communications and functionality between the Host software and the SCM5821AX1 camera firmware. </summary>
    public class SCM5821AX1
    {

        /// <summary> Length of time command transactions wait for reply. </summary>
        private const int MAXIMUM_TIMEOUT_MS = 500;

        /// <summary>
        /// Information retruned from the camera such as serial number for most boards and software version.
        /// </summary>
        private CameraInformation cameraInfo = new CameraInformation();

        /// <summary>
        /// Configuration information retruned from the cameras firmware such as voltages.
        /// </summary>
        private SCM5821AX1DataTypes.ConfigurationStructure camerasConfigInfo = new SCM5821AX1DataTypes.ConfigurationStructure();

        /// <summary> These values decode packet formats. </summary>
        /// <remarks> This structure is synchronized with camera firmware.  </remarks>
        public enum PacketType : uint
        {
            /// <summary>value used to mark packet sent to camera requesting execution of a command. </summary>
            Command = 0x50444d43,

            /// <summary> value used to identify response to execution of command by camera. </summary>
            Response = 0x50534552,

            /// <summary> value used to identify packet containing camera status information. </summary>
            Status = 0x54415453,

            /// <summary>value used to identify packet containing ROI header information. </summary>
            ImageStart = 0x53444956,

            /// <summary> value used to identify packet containing ROI video data. </summary>
            ImageData = 0x44444956,

            /// <summary> value used to identify packet containing Camera Voltage data. </summary>
            CameraVoltages = 0x53444957,

            /// <summary>
            /// Camera log from the FW
            /// </summary>
            Clog = 0x474F4C43,

            /// <summary> used to identify when the exposure has completed from the FW. </summary>
            ExposureComplete = 0x454e4f44,

            /// <summary> used to identify when the Camera meta-data is received from the FW. </summary>
            CameraInfo = 0x4f464e49,

            /// <summary> used to identify when the Camera configuration data is received from the FW. </summary>
            CameraConfig = 0x47464343
        }

        /// <summary> This structure is used to enumerate command and status information conveyed in Command and Response Packets. </summary>
        /// <remarks> This enumeration is synchronized with camera firmware. </remarks>
		[Flags]
        public enum CommandCode : uint
        {

            /* Special Commands. */
            Connect = 0x00000001,
            Disconnect = 0x00000002,
            ConnectionStatus = 0x00000003,
            Abort = 0x00000004,

            /* Normal Commands. */
            AcquisitionSettings = 0x00000005,

            /* Long Running Commands. */
            FixedExposure = 0x00000006,

            /* Stops all messages from FW during Disconnect */
            AbortMessages = 0x00000007,

            // Camera Commands 
            Delay = 0x00000011,
            Expose = 0x00000013,
            Start = 0x00000014,
            SetConfig = 0x00000015,
            GetVoltages = 0x00000016,
            GI = 0x00000017,
            Command = 0x00000018,
            TargetTemp = 0x00000019,
            TECEnabled = 0x00000020,
            LogLevel = 0x00000021,
            Reboot = 0x00000022,
            SetIpAddress = 0x00000023,
            GetCameraInfo = 0x00000024,
            GetConfigData = 0x00000025,
            GetCameraInfoMAC = 0x00000028,
            SetCamaraSerialNumbers = 0x00000029,
            HeartBeat = 0x00000030,

            UpdateFirmware = 0x00000031,
            UpdateFPGA = 0x00000032,
            UpdateLinux = 0x00000033,
            Restart = 0x00000034,

            CalibrateThresholds = 0x00000035, // Use Flash Up LED to find Full Well Counts  
            FlashToPercent = 0x00000036, // Use Flash Up LED to find Full Well Counts  
            FlashToAdu = 0x00000037, // Use Flash Up LED to find Full Well Counts  

            SetShutterOpenDelay = 0x00000038,
            SetShutterCloseDelay = 0x00000039,
            CalibrateShutter = 0x00000040,

            // Emulation Mode if any of these have a nonzero payload.
            EmulateMETS = 0x00000041, // Multi-Element Test Solution flux table for 153 subarrays
            EmulateScale = 0x00000042, // Linear Scale * Subarray ID
            EmulateOffset = 0x00000043, // Offset + Scale * Subarray ID
            EmulateNonlinear = 0x00000044, // 1 = Nonlinear, 0 = linear response 
            TestImagerControl = 0x00000045,

            TestTecCtlVDiv2      = 0x00000046, // float payload, Test feature to apply TEC Control voltage in 1.6 to 4.8 V range. 
            TecCtlStepmV         = 0x00000047, // uint  payload, 1 to 100. causes 2x step in V across TEC.
            RHLimit              = 0x00000048, // uint payload, 15% default req by Bremen. Allow up to 50% for Liverpool Nitrogen.
            TecTimeConstant      = 0x00000049, // Tau in seconds where temperature decays to exp(-1) = 0.3678 of initial value.
            CalibrateColumnGains = 0x00000050, // will generate gains for up to 1000 weak columns. Gains will be temporary.
            CorrectDeadPixels    = 0x00000051,
            UpdateEEPROM         = 0x00000052, // Write all local Config data to EEPROM.
            DarkPixelThresholdPercent = 0x00000053,
            PixelResponse        = 0x00000054, // Pixel Response 0xXXXYYYDN D=dX N=2**N NDROS
            CameraTempLimit      = 0x00000055, // Camera Temperature Limit when driving TEC. Protect camera from insufficient external cooling.
            FactoryDefaults      = 0x00000056, // Override custom settings. Issue UpdateEEPROM to make them persist.

            /* Status Values - Bits. */
            OK = 0x00000000,
            Error = 0x00001000,

            BadCommand = 0x00002000,
            BadArgument = 0x00004000,
            SettingsMismatch = 0x00008000,
            FWQueueFull = 0x00010000,

            /* Fields and Constants. */
            CommandMask = 0x00000FFF,
            StatusMask = 0x00FFF000,
            ResponseType = 0x80000000,
        }

        /// <summary> Used to coordinate transitions in the connection state between threads. </summary>
		private enum ConnectionState
        {
            Disconnected,
            Connecting,
            Connected,
            Disconnecting,
        }

        /// <summary> This structure is used to transmit commands to the camera and receive a response in turn. </summary>
        /// <remarks> This structure is synchronized with camera firmware. </remarks>
		private struct Command
        {
            /// <summary>
            /// Indicates the command being sent or the response being received.  This field also includes status values for
            /// the response packed into select bits.
            /// </summary>
			public CommandCode commandCode;

            /// <summary> Optional field that holds command parameters or response data serialized into a byte array. </summary>
			public byte[] payload;
        }


        /// <summary> This structure encapsulates raw image data from the CID821 camera. </summary>
        /// <remarks>
        /// This structure is synchronized with camera firmware.  This structure can be quite large if
        /// large images are acquired using high NDRO settings as none of the data is compressed or processed.
        /// The  current approach is to allow the host software the flexibility of logging _all_ raw video data
        /// and allowing the host to process (and/or reprocess) data if needed.
        /// The host can also monitor the acquisition in realtime using this data.
        /// </remarks>
        public struct CID821Image
        {
            /// <summary> Indicates that a given image from a given Subarray was taken under certain circumstances. </summary>
            /// <remarks>
            /// These fields are synchronized with camera firmware.  These fields are used to determine if a given
            /// image was taken with a diagnostic counter replacing the digitized values, if the image was taken using
            /// a destructive read and can be used to infer the camera operations applied to the Subarray under different
            /// acquisition algorithms.
            /// </remarks>
            ///   Use ImageTYpe from Enums
            //[Flags]
            //public enum ImageAttributes : ushort
            //{
            //    none,           // Default to none
            //    warmUp,         // warm up image - discard
            //    darkFrame,      // store dark frame
            //    calibrateAutoBias, // calibrate AutoBias image
            //    saveAutoBias,      // store AutoBias image
            //    FullFrame,      // store dark frame                                                         [x] Exposed Frame
            //    FixedExposure,  // Single frame exposure
            //    Examine,        // Subarray Threshold region
            //    SAFPN,          // save Subarray FPN image after GI,AB or SI
            //    Subarray,       // Subarray sent to host PC.                                            [x] Read 
            //    SubInject,      // clear region, save FPN image                                    [x] SubInject
            //    PostRead,       // Last read after end of exposure. Integrate with SubInject images.    [x] PostRead

            //}
            /// <summary> Timestamp of this data using the units specified in microseconds. </summary>
            /// <remarks> This timestamp is the start of integration for this image. Most recent of OpenShutter, GI or SI.
            /// </remarks>
            public UInt32 initialTimestamp;

            /// <summary>
            /// Earliest of image read or CloseShutter. Could be a Non-Destructive or Destructive Read.
            /// </summary>
            /// <remarks>IntegrationTime = timestampRead – initialTimestamp; </remarks>
            public UInt32 timestampRead;

            /// <summary> This is the tag field provided as a unique subarray descriptor. </summary>
            public ushort subarrayTag;

            /// <summary> This field indicates the circumstances underwhich this video data was taken. </summary>
            /// <remarks>
            /// This field can help the host process the video data properly and may be expanded to provide additional
            /// context on how the image was taken during readout.
            /// </remarks>
           // public ImageAttributes attributes;
            public byte imageType;

            /// <summary> This is the tag field provided as a unique exposure descriptor id. </summary>
            //public byte exposureTag;
            public UInt32 exposureTag;

            /// <summary>The upper left pixel coordinate(Column) of the Subarray. </summary>
            public ushort x0;

            /// <summary>The upper left pixel coordinate(Row) of the Subarray. </summary>
            public ushort y0;

            /// <summary> The number of pixels, Columns, offset from x0, to create a Subarray. </summary>
            public ushort dx;

            /// <summary> The number of pixels, Rows, offset from y0, to create a Subarray. </summary>
            public ushort dy;

            /// <summary> Number of Non destructive reads used to acquire this data. </summary>
            /// <remarks> This is used to interpret the data field. </remarks>
            public ushort ndros;

            /// <summary> Saturation bit for subarray. </summary>
            public ushort saturation;

            /// <summary> Number of Subinjects to subarray. </summary>
            public UInt32 subinjects;

            /// <summary>
            /// Pixel data in frame order. If multiple NDROS are used, the data is in frame or Subarray
            /// interleave order.
            /// </summary>
            public Int32[] data;
        }

        /// <summary> Callback signature used whenever the host receives a status packet from the camera. </summary>
        /// <param name="status">Structure to indicate Status and TEC Cooler state of the SCM5821AX1 camera.</param>
        public delegate void StatusEventHandler(SCM5821AX1DataTypes.StatusStructure status);

        /// <summary> Callback signature used whenever the host receives a Camera Info packet from the firmware. </summary>
        /// <param name="cameraInfo">Structure represents meta-data of the SCM5821AX1 camera.</param>
        //TODO[bdo]: changed to cameraInfo for consistency
        public delegate void CameraInfoEventHandler(CameraInformation cameraInfo);

        /// <summary> Callback signature used whenever the host receives a voltage message packet from the camera. </summary>
        /// <param name="voltages">Structure to indicate all voltages from the SCM5821AX1 camera.</param>
        public delegate void VoltageEventHandler(Voltages voltages);

        /// <summary> Callback signature used whenever the host receives a camera config message packet from the firmware. </summary>
        /// <param name="configData">Structure to indicate all config and voltages from the SCM5821AX1 camera.</param>
        public delegate void CameraConfigEventHandler(CameraConfigData configData);

        /// <summary> Callback signature used whenever the host receives a log data packet from the camera. </summary>
        /// <param name="logData">String to indicate error events of the SCM5821AX1 camera firmware.</param>
        public delegate void LogDataEventHandler(string logData);

        /// <summary> Callback signature used whenever the host receives a completed CID821Image from the camera. </summary>
		public delegate void ImageEventHandler();

 
        /// <summary> Callback signature used whenever the host receives an Exposure Complete packet from the camera. </summary>
        /// <param name="exposureId">Defines the completed exposure from the SCM5821AX1 camera.</param>
        public delegate void ExposureCompleteEventHandler(uint exposureId);

        /// <summary> Called each time the host receives a status packet from the camera. </summary>
        public event StatusEventHandler ReceivedStatus;

        /// <summary> Called each time the host receives a Camera Info packet from the firmware/camera. </summary>
        public event CameraInfoEventHandler ReceivedCameraInfo;

        /// <summary> Called each time the host receives a log data packet from the camera. </summary>
        public event LogDataEventHandler ReceivedLogData;

        /// <summary> Called each time the host receives a completed image from the camera. </summary>
		public event ImageEventHandler ReceivedImage;

        /// <summary> Called each time the host receives a completed image from the camera. </summary>
        public event ImageEventHandler ReceivedImageStream;

        /// <summary> Called each time the host receives an integrated image from the camera. </summary>
        public event ImageEventHandler ReceivedIntegratedImage;

        /// <summary> Called each time the host receives a voltage reading from the camera. </summary>
        public event VoltageEventHandler ReceivedCameraVoltages;

        /// <summary> Called each time the host receives a config message from the firmware. </summary>      
        public event CameraConfigEventHandler ReceivedCameraConfig;

        /// <summary> Called each time the host receives an Exposure Complete packet from the camera. </summary>
        public event ExposureCompleteEventHandler ReceivedExposureComplete;

        /// <summary> Used to serialize camera operations. </summary>
		private object lock_object;

        /// <summary> Used to coordinate connection transitions and state. </summary>
		private ConnectionState connection_state;

        /// <summary> This is the method used to pass commands to the camera. </summary>
		private BinaryWriter camera_writer;

        /// <summary> This thread manages the receive traffic from the camera. </summary>
		private Thread camera_thread;

        /// <summary> Pass response to a command from one thread to the next. </summary>
        /// <remarks> This may be overkill as currently only one response is expected at a time. </remarks>
		private BlockingCollection<Command> response_queue;

        /// <summary>
        /// Store image data in a thread-safe queue so that the application can process it
        /// asynchronously without blocking the event notifier.
        /// </summary>
		private BlockingCollection<CID821Image> image_queue;

        /// <summary>
        /// Video data arrives from the camera in chunks.  This member is used to coallesce the chunks into a final image.
        /// </summary>
		private CID821Image image_in_progress;

        /// <summary>
        /// Video data arrives from the camera in chunks.  This member is used to coallesce the chunks into a final image.
        /// </summary>
		private int image_in_progress_offset;

        /// <summary> Default constructor. </summary>
		public SCM5821AX1()
        {
            camera_writer = null;
            lock_object = new object();
            connection_state = ConnectionState.Disconnected;
            camera_thread = null;
            response_queue = null;
            image_queue = new BlockingCollection<CID821Image>();
        }

        /// <summary> Used internally to transmit commands to camera as various actions are invoked by the host. </summary>
        /// <remarks>
        /// Most of the API will utilize this function as it takes the associated lock.  However some functions
        /// require taking the lock over a wider scope and will call SendCommandUnlocked() instead.
        /// </remarks>
        private Command SendCommand(CommandCode code, byte[] payload = null, int receive_timeout = MAXIMUM_TIMEOUT_MS)
        {
            lock (lock_object) { return SendCommandUnlocked(code, payload, receive_timeout); }
        }

        /// <summary> Used internally to transmit commands to the camera and receive the associated response. </summary>
        /// <remarks> This is the raw function which must only be called with the lock taken. </remarks>
        private Command SendCommandUnlocked(CommandCode code, byte[] payload, int receive_timeout)
        {

            try
            {
                if (camera_writer != null)
                {
                    camera_writer.Write((uint)PacketType.Command);
                    camera_writer.Write((uint)code);
                    if (payload != null)
                    {
                        camera_writer.Write(payload.Length);

                        // Send message body/payload
                        if (payload.Length != 0)
                            camera_writer.Write(payload);
                    }
                    else
                        camera_writer.Write(0);
                }

                Command response = new Command();
                if (response_queue != null)
                    response_queue.TryTake(out response, receive_timeout);
                return response;
            }
            catch (Exception)
            {

                image_in_progress_offset = 0;
                Command response = new Command();
                response.commandCode = CommandCode.Error;
                return response;
            }

        }

        /// <summary> This function implements the Abort data command to the Firmware/FPGA. </summary>
        public void Abort()
        {
            camera_writer.Write((uint)PacketType.Command);
            camera_writer.Write((uint)CommandCode.Abort);
            camera_writer.Write(0);
        }

        /// <summary> This function implements the Abort all message data command to the Firmware/FPGA. </summary>
        public void AbortMessages()
        {
            camera_writer.Write((uint)PacketType.Command);
            camera_writer.Write((uint)CommandCode.AbortMessages);
            camera_writer.Write(0);
        }

        /// <summary> This function implements the heartbeat command to the Firmware/FPGA. </summary>
        public CommandCode SendHeartbeat()
        {
            var response = new Command();
            response = SendCommand(CommandCode.HeartBeat);

            return response.commandCode;
        }


        /// <summary> This function implements the Set New IP Address command to the Firmware. </summary>
        public bool SetNewIpAddress(string ipAddress)
        {
            // Firmware can't process strings so break address into 4 bytes -octets
            IPAddress address = IPAddress.Parse(ipAddress);
            byte[] addressInBytes = address.GetAddressBytes();
            var payload = new byte[4];
            payload = addressInBytes;
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            var response = SendCommand(CommandCode.SetIpAddress, payload);

            if ((response.commandCode & SCM5821AX1.CommandCode.StatusMask) == SCM5821AX1.CommandCode.OK)
                return true;
            else
                return false;
        }

        /// <summary> This function sends a request to get camera EPROM information from the Firmware. </summary>
        public bool GetCameraInformation()
        {
            var response = SendCommand(CommandCode.GetCameraInfoMAC);
            if ((response.commandCode & SCM5821AX1.CommandCode.StatusMask) == SCM5821AX1.CommandCode.OK)
                return true;
            else
                return false;
        }

        /// <summary> This function sends a request to get camera configuration information from the Firmware. </summary>
        public bool GetCameraConfigData()
        {
            var response = SendCommand(CommandCode.GetConfigData);
            if ((response.commandCode & SCM5821AX1.CommandCode.StatusMask) == SCM5821AX1.CommandCode.OK)
                return true;
            else
                return false;
        }

        /// <summary> This function implements the deserialization and dispatch operations of the receive thread. </summary>
        /// <param name="socket">The Socket class provides a rich set of methods and properties for network communications</param> 
        private void ReceivePacket(Socket socket)
        {
            var packet_type = ReadUInt32(socket);
            switch (packet_type)
            {
                case (uint)PacketType.Response:
                    {
                        var resp = new Command();
                        resp.commandCode = (CommandCode)ReadUInt32(socket);
                        if ((resp.commandCode & CommandCode.CommandMask) == CommandCode.Disconnect)
                        {
                            connection_state = ConnectionState.Disconnecting;
                            SetConnectionState(ConnectionState.Disconnected);
                            camera_thread.Abort();
                            camera_thread = null;  
                        }

                        int payload_length = ReadInt32(socket);
                        if (payload_length > 0)
                        {
                            resp.payload = new byte[payload_length];
                            int bytes_read = 0;
                            while (bytes_read < payload_length)
                            {
                                int num = socket.Receive(resp.payload, bytes_read, payload_length - bytes_read, SocketFlags.None);
                                bytes_read += num;
                            }
                        }
                        response_queue.Add(resp);

                        // Signal to calling code we've been asked to disconnect.					
                        if ((resp.commandCode & CommandCode.CommandMask) == CommandCode.Disconnect)
                        {
                            connection_state = ConnectionState.Disconnecting;
                        }
                        break;
                    }

                case (uint)PacketType.ImageStart:
                    {
                        uint length_bytes = ReadUInt32(socket);
                        image_in_progress = new CID821Image();
                        image_in_progress.data = new Int32[length_bytes / sizeof(ushort)];
                        image_in_progress.initialTimestamp = ReadUInt32(socket);
                        image_in_progress.timestampRead = ReadUInt32(socket);
                        image_in_progress.subarrayTag = (ushort)ReadUInt16(socket);

                        ushort byteHolder = (ushort)ReadUInt16(socket);
                        byte[] byteArray = BitConverter.GetBytes(byteHolder);

                        //byte spareByte = byteArray[0];
                        image_in_progress.saturation = byteArray[0];
                        image_in_progress.imageType = byteArray[1];

                        image_in_progress.x0 = (ushort)ReadUInt16(socket);
                        image_in_progress.y0 = (ushort)ReadUInt16(socket);
                        image_in_progress.dx = (ushort)ReadUInt16(socket);
                        image_in_progress.dy = (ushort)ReadUInt16(socket);                     

                        // In case exposureTag is > 256, read as an UInt32
                        image_in_progress.exposureTag = ReadUInt32(socket);
                        if (cameraInfo.FirmwareRevision > 180917)
                        {
                            image_in_progress.subinjects = ReadUInt32(socket);
                        }else
                        {
                            image_in_progress.subinjects = 0;
                        }

                        image_in_progress_offset = 0;
                        break;
                    }

                case (uint)PacketType.ImageData:
                    {
                        int length_bytes = ReadInt32(socket);
                        var data = new byte[length_bytes];
                        int bytes_read = 0;
                        if (image_in_progress.data != null)
                        {
                            while (bytes_read < length_bytes)
                            {
                                try
                                {
                                    int num = socket.Receive(data, bytes_read, length_bytes - bytes_read, SocketFlags.None);
                                    bytes_read += num;
                                }catch(Exception)
                                {
                                    image_in_progress_offset = 0;
                                }
                            }
                            try
                            {
                                Buffer.BlockCopy(data, 0, image_in_progress.data, image_in_progress_offset, length_bytes);

                            }
                            catch (Exception)
                            {
                                // Console.WriteLine("Block Copy error", e);
                                image_in_progress_offset = 0;
                            }

                            image_in_progress_offset += length_bytes;
                            if (image_in_progress_offset == (image_in_progress.data.Length * sizeof(ushort)))
                            {
                                image_queue.Add(image_in_progress);

                                //TODO[bdo][dko]: this is the new way which will get a full image sized array containing one subarray
                                if (ReceivedImageStream != null)
                                    ReceivedImageStream();

                                //TODO[bdo][dko]: this was added to support 2 ways of get the images, this is the previous way which will get multiple subarrays
                                if (ReceivedImage != null)
                                {
                                    ReceivedImage();
                                }
                                image_in_progress_offset = 0;
                            }
                        }
                        break;
                    }

                case (uint)PacketType.Status:
                    {
                        uint version = ReadUInt32(socket);
                        var status = new SCM5821AX1DataTypes.StatusStructure();
                        status.sequenceNumber = ReadSingle(socket);

                        status.diodeTemp = ReadSingle(socket);
                        status.tecTherm1v = ReadSingle(socket);
                        status.tecTherm2v = ReadSingle(socket);
                        status.isiTemp = ReadSingle(socket);
                        status.cspTemp = ReadSingle(socket);
                        status.pwrLocalTemp0 = ReadSingle(socket);
                        status.pwrLocalTemp1 = ReadSingle(socket);
                        status.cpuTemp = ReadSingle(socket);
                        status.tecEn = ReadUInt32(socket);
                        status.tecVdiv2 = ReadSingle(socket);

                        status.tecCtlV = ReadSingle(socket);
                        status.cameraV = ReadSingle(socket);
                        status.cameraA = ReadSingle(socket);

                        if (cameraInfo.FirmwareRevision > 230809)
                        {
                            status.interlocks = ReadInt32(socket);
                        }
                       

                        // Validate contents of status packet.
                        if (ReceivedStatus != null)
                            ReceivedStatus(status);
                        break;
                    }

                case (uint)PacketType.CameraInfo:
                    {
                        cameraInfo.FirmwareRevision = ReadInt32(socket);
                        cameraInfo.CameraVersion = ReadInt32(socket);             
                        cameraInfo.BoardVersion = ReadInt32(socket);
                        cameraInfo.CameraApiVersion = ReadInt32(socket);
                        cameraInfo.CameraType = ReadInt32(socket);
                        cameraInfo.FPGAVersion = ReadInt32(socket);
                        cameraInfo.BoardSupportPackageVersion = ReadInt32(socket);
                        cameraInfo.MACAddress = ReadString(socket, 20).ToCharArray();
                        
                        CameraSerialNumbers serialNumbers = new CameraSerialNumbers();
                        serialNumbers.CPUSerialNumber = ReadString(socket, 20).ToCharArray();
                        serialNumbers.CSPSerialNumber = ReadString(socket, 20).ToCharArray();
                        serialNumbers.PWRBoardSerialNumber = ReadString(socket, 20).ToCharArray();
                        serialNumbers.ImagerSerialNumber = ReadString(socket, 20).ToCharArray();
                        serialNumbers.CameraSerialNumber = ReadString(socket, 20).ToCharArray();
                        serialNumbers.ISISerialNumber = ReadString(socket, 20).ToCharArray();                     

                        cameraInfo.SerialNumbers = serialNumbers;

                        // Validate contents of status packet.
                        if (ReceivedCameraInfo != null)
                            ReceivedCameraInfo(cameraInfo);
                        break;
                    }

                case (uint)PacketType.Clog:
                    {
                        uint lengthInBytes = ReadUInt32(socket);
                        string logData = ReadString(socket, lengthInBytes);
                        if (ReceivedLogData != null)
                            ReceivedLogData(logData);
                        break;
                    }

                case (uint)PacketType.ExposureComplete:
                    {
                        uint exposureId = ReadUInt32(socket);
                        if (ReceivedExposureComplete != null)
                            ReceivedExposureComplete(exposureId);
                        break;
                    }

                case (uint)PacketType.CameraVoltages:
                    {
                        uint headerLength = ReadUInt32(socket);
                        uint word = ReadUInt32(socket);
                        var voltages = new Voltages();

                        voltages.ad9826Gain = ReadSingle(socket);
                        voltages.ad9826Offset = ReadSingle(socket);
                        voltages.imrefcdsVa = ReadSingle(socket);
                        voltages.selinjectVb = ReadSingle(socket);
                        voltages.selinjectVc = ReadSingle(socket);
                        voltages.selinjectVd = ReadSingle(socket);

                        voltages.impxlbiasVa = ReadSingle(socket);
                        voltages.unselinjectVc = ReadSingle(socket);
                        voltages.selsenseVa = ReadSingle(socket);
                        voltages.selsenseVb = ReadSingle(socket);
                        voltages.selsenseVc = ReadSingle(socket);
                        voltages.unselsenseVc = ReadSingle(socket);

                        voltages.selstoreVa = ReadSingle(socket);
                        voltages.selstoreVb = ReadSingle(socket);
                        voltages.selstoreVc = ReadSingle(socket);
                        voltages.unselstoreVc = ReadSingle(socket);

                        voltages.selresetVb = ReadSingle(socket);
                        voltages.selresetVc = ReadSingle(socket);
                        voltages.outbiasVd = ReadSingle(socket);
                        voltages.unselresetVc = ReadSingle(socket);
                        voltages.seltgVc = ReadSingle(socket);
                        voltages.unseltgVc = ReadSingle(socket);

                        voltages.pixelvddVa = ReadSingle(socket);
                        voltages.videobiasVb = ReadSingle(socket);
                        voltages.incdsVc = ReadSingle(socket);
                        voltages.imcdsbiasVd = ReadSingle(socket);

                        if (ReceivedCameraVoltages != null)
                            ReceivedCameraVoltages(voltages);
                        break;
                    }

                case (uint)PacketType.CameraConfig:
                    {
                        var config = new CameraConfigData();
                        config.Voltages = new Voltages();

                        uint headerLength = ReadUInt32(socket);
                        uint word = ReadUInt32(socket);

                        config.Voltages.ad9826Gain = ReadSingle(socket);
                        config.Voltages.ad9826Offset = ReadSingle(socket);
                        config.Voltages.imrefcdsVa = ReadSingle(socket);
                        config.Voltages.selinjectVb = ReadSingle(socket);
                        config.Voltages.selinjectVc = ReadSingle(socket);
                        config.Voltages.selinjectVd = ReadSingle(socket);

                        config.Voltages.impxlbiasVa = ReadSingle(socket);
                        config.Voltages.unselinjectVc = ReadSingle(socket);
                        config.Voltages.selsenseVa = ReadSingle(socket);
                        config.Voltages.selsenseVb = ReadSingle(socket);
                        config.Voltages.selsenseVc = ReadSingle(socket);
                        config.Voltages.unselsenseVc = ReadSingle(socket);

                        config.Voltages.selstoreVa = ReadSingle(socket);
                        config.Voltages.selstoreVb = ReadSingle(socket);
                        config.Voltages.selstoreVc = ReadSingle(socket);
                        config.Voltages.unselstoreVc = ReadSingle(socket);

                        config.Voltages.selresetVb = ReadSingle(socket);
                        config.Voltages.selresetVc = ReadSingle(socket);
                        config.Voltages.outbiasVd = ReadSingle(socket);
                        config.Voltages.unselresetVc = ReadSingle(socket);
                        config.Voltages.seltgVc = ReadSingle(socket);
                        config.Voltages.unseltgVc = ReadSingle(socket);

                        config.Voltages.pixelvddVa = ReadSingle(socket);
                        config.Voltages.videobiasVb = ReadSingle(socket);
                        config.Voltages.incdsVc = ReadSingle(socket);
                        config.Voltages.imcdsbiasVd = ReadSingle(socket);

                        config.TecEnabled = ReadInt32(socket);
                        config.TecTargetTemp = (int)ReadSingle(socket);
                        config.OpenShutterSpeed = ReadInt32(socket);
                        config.CloseShutterSpeed = ReadInt32(socket);

                        if (ReceivedCameraConfig != null)
                            ReceivedCameraConfig(config);
                        break;
                    }

                default:
                    // Drop into an error state and disconnect.
                    System.Diagnostics.Debug.WriteLine("ERROR: {0}", packet_type);
                    string errorString = "ERROR: {0}" + packet_type.ToString();
                    break;
            }
        }

        /// <summary> Helper function to derserialize data from network receive thread. </summary>
        private ushort ReadByte(Socket s)
        {
            var bytes = new byte[sizeof(ushort)];
            int bytes_read = 0;
            while (bytes_read < bytes.Length)
            {
                int num = s.Receive(bytes, bytes_read, bytes.Length - bytes_read, SocketFlags.None);
                bytes_read += num;
            }
            return BitConverter.ToChar(bytes, 0);
        }

        /// <summary> Helper function to derserialize data from network receive thread. </summary>
        private ushort ReadUInt16(Socket s)
        {
            var bytes = new byte[sizeof(ushort)];
            int bytes_read = 0;
            while (bytes_read < bytes.Length)
            {
                int num = s.Receive(bytes, bytes_read, bytes.Length - bytes_read, SocketFlags.None);
                bytes_read += num;
            }
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary> Helper function to derserialize data from network receive thread. </summary>
        private int ReadInt32(Socket s)
        {
            var bytes = new byte[sizeof(int)];
            int bytes_read = 0;
            while (bytes_read < bytes.Length)
            {
                try
                {
                    int num = s.Receive(bytes, bytes_read, bytes.Length - bytes_read, SocketFlags.None);
                    bytes_read += num;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary> Helper function to derserialize data from network receive thread. </summary>
        private uint ReadUInt32(Socket s)
        {
            var bytes = new byte[sizeof(uint)];
            int bytes_read = 0;
            while (bytes_read < bytes.Length)
            {
                try
                {
                    int num = s.Receive(bytes, bytes_read, bytes.Length - bytes_read, SocketFlags.None);
                    bytes_read += num;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary> Helper function to derserialize data from network receive thread. </summary>
        private ulong ReadUInt64(Socket s)
        {
            var bytes = new byte[sizeof(ulong)];
            int bytes_read = 0;
            while (bytes_read < bytes.Length)
            {
                int num = s.Receive(bytes, bytes_read, bytes.Length - bytes_read, SocketFlags.None);
                bytes_read += num;
            }
            return BitConverter.ToUInt64(bytes, 0);
        }

        /// <summary> Helper function to derserialize data from network receive thread. </summary>
        private float ReadSingle(Socket s)
        {
            var bytes = new byte[sizeof(float)];
            int bytes_read = 0;
            while (bytes_read < bytes.Length)
            {
                int num = s.Receive(bytes, bytes_read, bytes.Length - bytes_read, SocketFlags.None);
                bytes_read += num;
            }
            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary> Helper function to derserialize data from network receive thread. </summary>
        private string ReadString(Socket s, UInt32 length)
        {
            var bytes = new byte[length];
            int bytes_read = 0;
            while (bytes_read < bytes.Length)
            {
                int num = s.Receive(bytes, bytes_read, bytes.Length - bytes_read, SocketFlags.None);
                bytes_read += num;
            }
            string result = System.Text.Encoding.UTF8.GetString(bytes);

            return result;// BitConverter.ToString(bytes, 0);
        }


        /// <summary> Core receive thread. </summary>
        private void ReceiveThreadWorker(Socket camera_socket, ManualResetEvent initialized_event)
        {
            var camera_stream = new NetworkStream(camera_socket);
            camera_writer = new BinaryWriter(camera_stream);
            response_queue = new BlockingCollection<Command>();

            // Here to prevent commands from being transmitted until the networking variables are properly established.
            // TODO: Initialize those same variables in 'Connect' and get rid of this.
            initialized_event.Set();

            // Use Time to determine if Camera Firmware has stopped sending messages after a sucessful connection
            DateTime startTime = DateTime.Now;
            DateTime endtime = DateTime.Now;
            TimeSpan duration = startTime - endtime;

            while (connection_state == ConnectionState.Connected || connection_state == ConnectionState.Connecting)
            {
                try
                {
                    startTime = DateTime.Now;

                    if (!(camera_socket.Poll(-1, SelectMode.SelectRead) && camera_socket.Available == 0))
                    {
                        ReceivePacket(camera_socket);
                        endtime = DateTime.Now; 
                    }
                    else
                    {
                       // break;
                    }
                    duration = startTime - endtime;
                 //   if (duration.Seconds > 5)
                  //  {
                        // I dont think the FW is ready for this logic yet.
                  //      SetConnectionState(ConnectionState.Disconnected);
                  //      break;
                  //  }
                }

                catch (IOException)
                {
                    break;
                }
            }
            camera_writer.Close();
            camera_stream.Close();
            camera_socket.Close();
            camera_writer = null;

            // This field is essentially serialized due to behavior and interactions with commands and responses
            // being serialized and well ordered.
            response_queue = null;
        }

        /// <summary> Begin a TCP/IP session with the camera and its firmware at the specified address and port. </summary>
        /// <param name="ipaddress">Dot notated string for the IP Address</param>
        /// <param name="port">Generic port number greater than 4000</param>
        public int Connect(string ipaddress, string port)
        {
            int fwId = 0;

            lock (lock_object)
            {
                if (!IsDisconnected)
                    throw new InvalidOperationException("Camera is not in disconnected state.");

                var initializedEvent = new ManualResetEvent(false);
                var ipEndpoint = new IPEndPoint(IPAddress.Parse(ipaddress), int.Parse(port));
                var cameraSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                cameraSocket.NoDelay = true;

                cameraSocket.ReceiveTimeout = 1000;
                cameraSocket.SendTimeout = 1000;
                cameraSocket.LingerState = new LingerOption(true, 5);


                try
                {
                    cameraSocket.Connect(ipEndpoint);
                }
                catch (SocketException)
                {
                    return 0;
                }

                SetConnectionState(ConnectionState.Connecting);

                camera_thread = new Thread(delegate () {
                    ReceiveThreadWorker(cameraSocket, initializedEvent);
                });
                camera_thread.Start();

                initializedEvent.WaitOne();

                try
                {
                    if (cameraSocket.Connected)
                    {
                        // Send Firmware the Year-Month-Day of the last Major build of CID API during connection                     
                        byte[] byteArray = BitConverter.GetBytes(Thermo.Kronos.Instrument.Camera.Contracts.Constants.ApiVersion);
                        var response = SendCommandUnlocked(CommandCode.Connect, byteArray, MAXIMUM_TIMEOUT_MS);

                        if (response.payload != null)
                            // Convert byte array to FW Id field
                            fwId = (BitConverter.ToInt32(response.payload, 0));

                        // Set Connect state and FW ID
                        SetConnectionState(ConnectionState.Connected);
                    }
                }
                catch (TimeoutException)
                {
                    SetConnectionState(ConnectionState.Disconnecting);
                    camera_thread.Join();
                    camera_thread = null;

                    SetConnectionState(ConnectionState.Disconnected);
                }
            }
            return fwId;
        }

        /// <summary> Terminates a session with the camera. </summary>
        public void Disconnect()
        {
            lock (lock_object)
            {               
                //var response = SendCommandUnlocked(CommandCode.Disconnect, null, MAXIMUM_TIMEOUT_MS);	
                // Reduce timeout to 0 for disconnect
                var response = SendCommandUnlocked(CommandCode.Disconnect, null, 0);
                SetConnectionState(ConnectionState.Disconnected);

            }
        }

        /// <summary> Check the validity of the exposure acquisition parameters to the camera. </summary>
        /// <param name="exposureParameters">Container holding exposure parameters populated by the host operator</param>
        /// <returns>TRUE if all parameters are excepted, FALSE otherwise</returns>
        public Boolean ValidExposure(ExposureData.ExposureDataContainer exposureParameters)
        {
            if ((exposureParameters.exposureData.exposureRegion.dX < 0) ||
                (exposureParameters.exposureData.exposureRegion.dY < 0))
            {
                return false;
            }

            if ((exposureParameters.exposureData.exposureRegion.dX > 2048) ||
                (exposureParameters.exposureData.exposureRegion.dY > 2048))
            {
                return false;
            }

            if ((exposureParameters.exposureData.exposureRegion.Xo < 0) ||
                (exposureParameters.exposureData.exposureRegion.Yo < 0))
            {
                return false;
            }

            if ((exposureParameters.exposureData.exposureRegion.Xo > 2048) ||
                (exposureParameters.exposureData.exposureRegion.Yo > 2048))
            {
                return false;
            }

            if (exposureParameters.exposureData.numberOfSubarrays > 600)
            {
                return false;
            }

            if (((exposureParameters.exposureData.exposureRegion.dX +
                  exposureParameters.exposureData.exposureRegion.Xo) > 2048) ||

                 (exposureParameters.exposureData.exposureRegion.dY +
                  exposureParameters.exposureData.exposureRegion.Yo) > 2048)
            {
                return false;
            }

            // Exposure time under 1 hour 11 minutes or 71 * 60,000
            if (exposureParameters.exposureData.exposureInterval > 4260000)
            {
                return false;
            }

            // Check subarray parameters also
            for (int subarrayIndex = 0; subarrayIndex < exposureParameters.exposureData.numberOfSubarrays; subarrayIndex++)
            {
                int sumX = (exposureParameters.exposureData.subarrayList[subarrayIndex].subarrayRegion.dX +
                  exposureParameters.exposureData.subarrayList[subarrayIndex].subarrayRegion.Xo);
                int sumY = (exposureParameters.exposureData.subarrayList[subarrayIndex].subarrayRegion.dY +
                  exposureParameters.exposureData.subarrayList[subarrayIndex].subarrayRegion.Yo);

                if (((sumX) > 2048) || (sumY) > 2048)
                {
                    return false;
                }
            }
            // else return true, no invalid parameter conditions were met
            return true;
        }

        /// <summary> Check the validity of the exposure acquisition parameters to the camera. </summary>
        /// <param name="exposureParameters">Container holding exposure parameters populated by the host operator</param>
        /// <returns>String containing why parameter is invalid</returns>
        public string CheckExposureParameter(ExposureData.ExposureDataContainer exposureParameters)
        {
            if ((exposureParameters.exposureData.exposureRegion.dX < 0) ||
                (exposureParameters.exposureData.exposureRegion.dY < 0))
            {
                return "Check dX or dY for out of range.";
            }

            if ((exposureParameters.exposureData.exposureRegion.dX > 2048) ||
                (exposureParameters.exposureData.exposureRegion.dY > 2048))
            {

                return "Check dX or dY for out of range.";
            }

            if ((exposureParameters.exposureData.exposureRegion.Xo < 0) ||
                (exposureParameters.exposureData.exposureRegion.Yo < 0))
            {
                return "Check Xo or Yo for out of range.";
            }

            if ((exposureParameters.exposureData.exposureRegion.Xo > 2048) ||
                (exposureParameters.exposureData.exposureRegion.Yo > 2048))
            {
                return "Check Xo or Yo for out of range.";
            }

            if (exposureParameters.exposureData.numberOfSubarrays > 600)
            {
                return "Maximum number of Subarrays is 600.";
            }

            if (((exposureParameters.exposureData.exposureRegion.dX +
                  exposureParameters.exposureData.exposureRegion.Xo) > 2048) ||

                 (exposureParameters.exposureData.exposureRegion.dY +
                  exposureParameters.exposureData.exposureRegion.Yo) > 2048)
            {
                return "Check dX + Xo or dY + Yo for out of range, must be less than 2048";
            }

            // Exposure time under 1 hour 11 minutes or 71 * 60,000
            if (exposureParameters.exposureData.exposureInterval > 4260000)
            {
                return "Maximum time is 1 hour and 11 minutes.";
            }

            // Check subarray parameters also
            for (int subarrayIndex = 0; subarrayIndex < exposureParameters.exposureData.numberOfSubarrays; subarrayIndex++)
            {
                int sumX = (exposureParameters.exposureData.subarrayList[subarrayIndex].subarrayRegion.dX +
                  exposureParameters.exposureData.subarrayList[subarrayIndex].subarrayRegion.Xo);
                int sumY = (exposureParameters.exposureData.subarrayList[subarrayIndex].subarrayRegion.dY +
                  exposureParameters.exposureData.subarrayList[subarrayIndex].subarrayRegion.Yo);

                if (((sumX) > 2048) || (sumY) > 2048)
                {
                    return "Check dX + Xo or dY + Yo for out of range, must be less than 2048";
                }
            }

            // else valid exposure, no invalid parameter conditions were found
            return "Exposure Valid";
        }

        /// <summary> Transmit the exposure acquisition parameters to the camera. </summary>
        /// <param name="exposureParameters">Container holding exposure parameters populated by the host operator</param>
        public bool SendExposureSettings(ExposureData.ExposureDataContainer exposureParameters)
        {
            var length = exposureParameters.GetNumBytes((int)exposureParameters.
                exposureData.numberOfSubarrays);

            // Check validity of Exposure data first
            if (!ValidExposure(exposureParameters) || (camera_thread == null))
            {
                // If one of the parameters are invalid, return
                return false;
            }

            // Build the payload for the message to the FPGA using a binary stream
            var payload = new byte[length];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            Command response;
            exposureParameters.WriteExposure(binaryWriter);

            // Send the message to the firmware         
            response = SendCommand(CommandCode.Expose, payload,0);

            // Check the firmwares response to sending the exposure
            if ((response.commandCode & SCM5821AX1.CommandCode.StatusMask) == SCM5821AX1.CommandCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }              
        }

        /// <summary>
        /// Sends a command from host to camera defining the length in microseconds of a global inject/clear
        /// </summary>
        /// <param name="duration"></param>
        public void SendGlobalInject(UInt32 duration)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(duration)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(duration);
            var response = SendCommand(CommandCode.GI, payload,0);
        }


        /// <summary> Transmit the acquisition parameters to the camera. </summary>
        public void SendCameraConfiguration(SCM5821AX1DataTypes.ConfigurationStructure configStruct)
        {        
           // var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(configStruct)];
            var payload = new byte[120];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(configStruct.voltages.ad9826Gain);
            binaryWriter.Write(configStruct.voltages.ad9826Offset);
            binaryWriter.Write(configStruct.voltages.imrefcdsVa);
            binaryWriter.Write(configStruct.voltages.selinjectVb);
            binaryWriter.Write(configStruct.voltages.selinjectVc);
            binaryWriter.Write(configStruct.voltages.selinjectVd);

            binaryWriter.Write(configStruct.voltages.impxlbiasVa);
            binaryWriter.Write(configStruct.voltages.unselinjectVc);
            binaryWriter.Write(configStruct.voltages.selsenseVa);
            binaryWriter.Write(configStruct.voltages.selsenseVb);
            binaryWriter.Write(configStruct.voltages.selsenseVc);
            binaryWriter.Write(configStruct.voltages.unselsenseVc);
            binaryWriter.Write(configStruct.voltages.selstoreVa);
            binaryWriter.Write(configStruct.voltages.selstoreVb);
            binaryWriter.Write(configStruct.voltages.selstoreVc);
            binaryWriter.Write(configStruct.voltages.unselstoreVc);
            binaryWriter.Write(configStruct.voltages.selresetVb);
            binaryWriter.Write(configStruct.voltages.selresetVc);

            binaryWriter.Write(configStruct.voltages.outbiasVd);
            binaryWriter.Write(configStruct.voltages.unselresetVc);
            binaryWriter.Write(configStruct.voltages.seltgVc);
            binaryWriter.Write(configStruct.voltages.unseltgVc);
            binaryWriter.Write(configStruct.voltages.pixelvddVa);
            binaryWriter.Write(configStruct.voltages.videobiasVb);
            binaryWriter.Write(configStruct.voltages.incdsVc);
            binaryWriter.Write(configStruct.voltages.imcdsbiasVd);
            binaryWriter.Write(configStruct.tecEnabled);
            binaryWriter.Write(configStruct.tecTargetTemp);
            binaryWriter.Write(configStruct.openShutterSpeed);
            binaryWriter.Write(configStruct.closeShutterSpeed);
            var response = SendCommand(CommandCode.SetConfig, payload);
        }

        /// <summary>
        /// Sends a command from host to camera defining the configuration timing table and other registers.
        /// </summary>
        public void SendCameraConfiguration()
        {
            var payload = new byte[0];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            var response = SendCommand(CommandCode.SetConfig, payload);
        }

        /// <summary>
        /// Sends a command from host to camera Enabling or Disabling the TE Cooler.
        /// </summary>
        /// <param name="enableTEC"></param>
        public void SendTECEnable(UInt32 enableTEC)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(enableTEC)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(enableTEC);

            // Send the message to the FPGA
            var response = SendCommand(CommandCode.TECEnabled, payload);
        }

        /// <summary>
        /// Sends a command from host to camera setting the shutter open delay time.
        /// </summary>
        /// <param name="delay"></param>
        public void SetShutterOpenDelay(UInt32 delay)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(delay)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(delay);

            // Send the message to the FPGA
            var response = SendCommand(CommandCode.SetShutterOpenDelay, payload);
        }

        /// <summary>
        /// Sends a command from host to camera setting the shutter close delay time.
        /// </summary>
        /// <param name="delay"></param>
        public void SetShutterCloseDelay(UInt32 delay)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(delay)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(delay);

            // Send the message to the FPGA
            var response = SendCommand(CommandCode.SetShutterCloseDelay, payload);
        }

        /// <summary>
        /// Sends a command from host to camera setting the Calibrate Thresholds flag.    
        /// </summary>
        public void SetCalibrateThresholds()
        {     
            // Send the message to the FPGA
            var response = SendCommand(CommandCode.CalibrateThresholds);
        }

        /// <summary>
        /// Sends a command from host to camera setting the Flash To Percent.
        /// </summary>
        public void SetFlashToPercent()
        {       
            // Send the message to the FPGA
            var response = SendCommand(CommandCode.FlashToPercent);
        }

        /// <summary>
        /// Sends a command from host to camera setting the Flash To Adu.
        /// </summary>
        public void SetFlashToAdu()
        {
            // Send the message to the FPGA
            var response = SendCommand(CommandCode.FlashToAdu);
        }

        /// <summary>
        /// Sends a command from host to camera setting the Calibrate Shutter flag.
        /// </summary>

        public void SetCalibrateShutter()
        {
            // Send the message to the FPGA
            var response = SendCommand(CommandCode.CalibrateShutter);
        }

        /// <summary>
        /// Sends a command from host to camera setting the target temperature in degrees C for the TE Cooler
        /// </summary>
        /// <param name="tecTargetTemp">Target Temperature in degrees C</param>
        public void SendTargetTemp(float tecTargetTemp)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(tecTargetTemp)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(tecTargetTemp);

            // Send the message to the FPGA
            var response = SendCommand(CommandCode.TargetTemp, payload);
        }

        /// <summary>
        /// Executes the Fixed Readout acquisition using the previously
        /// transmitted exposure parameters.
        /// </summary>
        public void RunFixed()
        {
            var response = SendCommand(CommandCode.FixedExposure);
        }


        /// <summary>
        /// Command message from host to camera descibing the length of time the camera does nothing, delays. 
        /// </summary>
        /// <param name="duration">Delay time in microseconds</param>
        public void SendDelay(UInt32 duration)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(duration)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(duration);
            var response = SendCommand(CommandCode.Delay, payload,0);

        }

        /// <summary>
        /// Sends a command from host to camera requesting the voltage settings.
        /// </summary>
        /// <param name="filename"></param>
        public void SendVoltagesRequest(string filename)
        {
            var payload = new byte[filename.Length + 10];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(filename);
           // var response = SendCommand(CommandCode.GetVoltages, payload);
            var response = SendCommand(CommandCode.GetVoltages);
        }

        /// <summary>
        /// Sends a command from host to camera requesting the config data settings.
        /// </summary>
        /// <param name="filename"></param>
        public void SendConfigDataRequest(string filename)
        {
            var payload = new byte[filename.Length + 10];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(filename);
            var response = SendCommand(CommandCode.GetConfigData, payload);
        }

        /// <summary>
        /// Sends a command from host to camera requesting to Upadate the Firmware.
        /// </summary>
        public void SendUpdateFirmware(string filename)
        {
            var payload = new byte[filename.Length + 10];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(filename);

            // Send the message to the FPGA          
            var response = SendCommand(CommandCode.UpdateFirmware, payload);
        }

        /// <summary>
        /// Sends a command from host to camera requesting to Upadate the FPGA.
        /// </summary>
        public void SendUpdateFPGA()
        {          
            var response = SendCommand(CommandCode.UpdateFPGA);
        }

        /// <summary>
        /// Sends a command from host to camera requesting to Upadate the BSP.
        /// </summary>
        public void SendUpdateLinux()
        {
            var response = SendCommand(CommandCode.UpdateLinux);
        }

        /// <summary>
        /// Sends a command from host to camera setting the target Log Level for the Firwmare
        /// </summary>
        /// <param name="logLevel">Log Level (Fatal=0, Error, Warning, Debug, Info, Camera)</param>
        public void SendLogLevel(int logLevel)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(logLevel)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(logLevel);

            // Send the message to the FPGA
            var response = SendCommand(CommandCode.LogLevel, payload);
        }

        /// <summary>
        /// Sends a command from host to camera setting all of the cameras boards serial numbers.
        /// </summary>
        /// <param name="serialNumbers">String</param>
        public void SendSerialNumbers(CameraSerialNumbers serialNumbers)
        {
       //     var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(CameraSerialNumbers.SERIAL_NUMBER_LENGTH * 6)];
            var payload = new byte[(CameraSerialNumbers.SERIAL_NUMBER_LENGTH * 6)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(serialNumbers.CPUSerialNumber);
            binaryWriter.Write(serialNumbers.CSPSerialNumber);
            binaryWriter.Write(serialNumbers.PWRBoardSerialNumber);
            binaryWriter.Write(serialNumbers.ImagerSerialNumber);
            binaryWriter.Write(serialNumbers.CameraSerialNumber);
            binaryWriter.Write(serialNumbers.ISISerialNumber);

            // Send the message to the FPGA
            var response = SendCommand(CommandCode.SetCamaraSerialNumbers, payload);
        }

        /// <summary>
        /// Sends a command from host to camera Starting all other commands that are queued.
        /// </summary>
        [Obsolete("Method SendStart is deprecated.")]
        public void SendStart()
        {
            var response = SendCommand(CommandCode.Start);
        }

        /// <summary>
        /// Sends a command from host to camera rebooting the CPU board the Firwmare resides.
        /// Can be used after a Download Firmware command from the host
        /// </summary>
        public void SendReboot()
        {
            // Send the message to the FPGA
            var response = SendCommand(CommandCode.Reboot);
        }

        /// <summary>
        /// Sends a command from host to camera to restart the Firwmare.
        /// Can be used after a Download Firmware command from the host
        /// </summary>
        public void SendRestart()
        {
            // Send the message to the FPGA
            var response = SendCommand(CommandCode.Restart);
        }


        /// <summary>
        /// Helper function to set connection state should a notifier be needed.
        /// </summary>
        private void SetConnectionState(ConnectionState state)
        {
            connection_state = state;

        }

        /// <summary> Accessor to determine connection/session state of camera. </summary>
        public bool IsConnected
        {
            get { return (connection_state == ConnectionState.Connected); }
        }

        /// <summary> Accessor to determine connection/session state of camera. </summary>
		public bool IsDisconnected
        {
            get { return (connection_state == ConnectionState.Disconnected); }
        }

        /// <summary>
        /// Retrieve an image from the image queue. Don't retrieve more images than specified by callbacks
        /// otherwise this will block.
        /// </summary>
        public CID821Image TakeImage()
        {
            return image_queue.Take();
        }

        /// <summary>
        /// Retrieve an image from the image queue.
        /// </summary>
        /// <param name="image">Returned image data from camera to host</param>
        /// 
        public bool TryTakeImage(out CID821Image image)
        {
            return image_queue.TryTake(out image);
        }

        /// <summary>
        /// Clear Image Queue.
        /// </summary>
        public void ClearImageQueue()
        {
            /* Only call this function when not connected. */
            //if (!IsDisconnected)
             //  throw new InvalidOperationException("Cannot clear image queue while connected.");
            while (image_queue.Count != 0)
            {
                CID821Image _;
                image_queue.TryTake(out _);
            }
        }

        /// <summary>
        /// Sends a command from host to camera setting the emulation of the METS concentration solution.
        /// </summary>
        /// <param name="parameter"></param>
        public void SetEmulateMETS(UInt32 parameter)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(parameter)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(parameter);
            if (cameraInfo.FirmwareRevision >= 200212)
            {
                // Send the message to the Firmware
                var response = SendCommand(CommandCode.EmulateMETS, payload);
            }
        }

        /// <summary>
        /// Sends a command from host to camera setting the emulation of the Scale.
        /// </summary>
        /// <param name="parameter"></param>
        public void SetEmulateScale(UInt32 parameter)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(parameter)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(parameter);

            if (cameraInfo.FirmwareRevision >= 200212)
            {
                // Send the message to the Firmware
                var response = SendCommand(CommandCode.EmulateScale, payload);
            }
        }

        /// <summary>
        /// Sends a command from host to camera setting the emulation of the Offset.
        /// </summary>
        /// <param name="parameter"></param>
        public void SetEmulateOffset(UInt32 parameter)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(parameter)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(parameter);

            if (cameraInfo.FirmwareRevision >= 200212)
            {
                // Send the message to the Firmware
                var response = SendCommand(CommandCode.EmulateOffset, payload);
            }
        }

        /// <summary>
        /// Sends a command from host to camera setting the emulation of the Non Linear paramerter.
        /// </summary>
        /// <param name="parameter"></param>
        public void SetEmulateNonlinear(UInt32 parameter)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(parameter)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(parameter);
            if (cameraInfo.FirmwareRevision >= 200212)
            {
                // Send the message to the Firmware
                var response = SendCommand(CommandCode.EmulateNonlinear, payload);
            }      
        }

        public void SetTestImagerControl(UInt32 imagerControl)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(imagerControl)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(imagerControl);
            
            // Send the message to the Firmware
            var response = SendCommand(CommandCode.TestImagerControl, payload);    
        }

        public void SetCalibrateColumnGains()
        {
  
            // Send the message to the Firmware
            var response = SendCommand(CommandCode.CalibrateColumnGains);
        }

        public void SetCorrectDeadpixels(UInt32 control)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(control)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(control);

            // Send the message to the Firmware
            var response = SendCommand(CommandCode.CorrectDeadPixels, payload);
        }

        public void SetDarkPixelThreshold(UInt32 thresholdPercent)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(thresholdPercent)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(thresholdPercent);

            // Send the message to the Firmware
            var response = SendCommand(CommandCode.DarkPixelThresholdPercent, payload);
        }

        /// <summary>
        /// Test feature to apply TEC Control voltage in 1.6 to 4.8 V range.
        /// </summary>
        /// <param name="parameter">The voltage parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public void SetTecCtlVDiv2(float parameter)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(parameter)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(parameter);

            // Send the message to the FPGA
            var response = SendCommand(CommandCode.TestTecCtlVDiv2, payload);
        }

        /// <summary>
        /// Sends a command from host to camera setting 1 to 100, causes 2x step in V across TEC.
        /// </summary>
        /// <param name="parameter">The step parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public void SetTecCtlStepmV(UInt32 parameter)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(parameter)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(parameter);

            // Send the message to the Firmware
            var response = SendCommand(CommandCode.TecCtlStepmV, payload);
        }

        /// <summary>
        /// Sends a command from host to camera setting the RH Limit for the firmware.
        /// </summary>
        /// <param name="parameter">The emulation parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public void SetRHLimit(UInt32 parameter)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(parameter)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(parameter);

            // Send the message to the Firmware
            var response = SendCommand(CommandCode.RHLimit, payload);
        }

        /// <summary>
        /// Sends a command from host to camera getting the TEC Time constant form the firmware.
        /// </summary>  
        /// <returns>returns a time indicating the success of the operation.</returns>
        public int GetTecTimeConstant()
        {                
            var command = SendCommand(CommandCode.TecTimeConstant);
            if (command.payload != null)
                return BitConverter.ToInt32(command.payload, 0);
            else
                return 0;
        }

        /// <summary>
        /// Sends a command from host to camera setting the TEC Time Constant for the firmware.
        /// </summary>
        /// <param name="parameter">The time in seconds parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public void SetTecTimeConstant(UInt32 parameter)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(parameter)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(parameter);

            // Send the message to the Firmware
            var response = SendCommand(CommandCode.TecTimeConstant, payload);
        }

        public void SetUpdatedEEProm()
        {
            // Send the message to the Firmware
            var response = SendCommand(CommandCode.UpdateEEPROM);
        }

        public void SetPixelResponse(UInt32 parameter)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(parameter)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(parameter);

            // Send the message to the Firmware
            var response = SendCommand(CommandCode.PixelResponse,payload);
        }

        /// <summary>
        /// Sends a command from host to camera setting the TEC Time Constant for the firmware.
        /// </summary>
        /// <param name="parameter">The time in seconds parameter.</param>
        /// <returns>returns a boolean indicating the success of the operation.</returns>
        public void SetCameraTempLimit(UInt32 parameter)
        {
            var payload = new byte[System.Runtime.InteropServices.Marshal.SizeOf(parameter)];
            var memoryStream = new MemoryStream(payload);
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(parameter);

            // Send the message to the Firmware
            var response = SendCommand(CommandCode.CameraTempLimit, payload);
        }

        public void SetFactoryDefaults()
        {
            // Send the message to the Firmware
            var response = SendCommand(CommandCode.FactoryDefaults);
        }
    }
}
