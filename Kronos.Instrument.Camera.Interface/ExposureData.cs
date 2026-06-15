using System.IO;
using System.Runtime.InteropServices;

namespace Thermo.Kronos.Instrument.Camera.Interface
{
    /// <summary>
    /// Class to control the layout of exposure objects when exported to unmanaged code.
    /// Structures are sized to match the SCM5821AX1 Software API document.
    /// </summary>
    public class ExposureData
    {
        /// <summary>
        /// Structure that contains all of the Exposure data fields defined by the host GUI
        /// </summary>
        public struct ExposureDataContainer
        {
            public SCM5821AX1DataTypes.ExposureStructure exposureData;

            /// <summary>
            /// Method to return size of the Exposure structure in bytes
            /// </summary>
            /// <param name="numberOfSubarrays"></param>
            /// <returns>Number of bytes in the exposure structure including size of subarray list</returns>
            public int GetNumBytes(int numberOfSubarrays)
            {
                int subarraySize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(SCM5821AX1DataTypes.SubarrayStructure));
                subarraySize = subarraySize * numberOfSubarrays;

                int exposureSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(SCM5821AX1DataTypes.ExposureStructure));
                return exposureSize + subarraySize;
            }

            /// <summary> Serialize this Exposure structure using the provided BinaryWriter. </summary>
            /// <remarks> This code is synchronized with camera firmware. </remarks>
            public void WriteExposure(BinaryWriter binaryWriter)
            {
                //binaryWriter.Write(exposureName);
                //exposureData.exposureId = 1;
                binaryWriter.Write(exposureData.exposureId);
                binaryWriter.Write(exposureData.numberOfNDROs);
                binaryWriter.Write(exposureData.exposureInterval);
                binaryWriter.Write(exposureData.exposureRegion.Xo);
                binaryWriter.Write(exposureData.exposureRegion.Yo);
                binaryWriter.Write(exposureData.exposureRegion.dX);
                binaryWriter.Write(exposureData.exposureRegion.dY);

                binaryWriter.Write(exposureData.exposureFPN);
                binaryWriter.Write(exposureData.autoBiasEnabled);
                binaryWriter.Write(exposureData.exposureAttributes);
                binaryWriter.Write(exposureData.spareByte2);

                binaryWriter.Write(exposureData.ledOffTime);
                binaryWriter.Write(exposureData.ledOnTime);
                binaryWriter.Write(exposureData.ledFlashes);
                binaryWriter.Write(exposureData.led1Enabled);
                binaryWriter.Write(exposureData.led2Enabled);
                binaryWriter.Write(exposureData.led3Enabled);
                binaryWriter.Write(exposureData.shutterEnabled);
                binaryWriter.Write(exposureData.fullFrameEnabled);
                binaryWriter.Write(exposureData.darkFrameEnabled);
              
                binaryWriter.Write(exposureData.numberOfSubarrays);
                for (uint subarrayIndex = 0; subarrayIndex < exposureData.numberOfSubarrays; subarrayIndex++)
                {
                    //binaryWriter.Write(subarrayList[subarrayIndex].subarrayName);

                    // If the subarray has Not been supplied by user, write the subarray list index
                    if (!exposureData.subarrayList[subarrayIndex].subarrayIdSupplied)
                    {              
                        exposureData.subarrayList[subarrayIndex].subarrayId = subarrayIndex;                  
                    }
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].subarrayId);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].thresholdValuePercent);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].readInterval);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].timeResolvedInterval);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].subInjectInterval);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].postreadNDROs);

                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].subarrayFPN);

                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].subarrayRegion.Xo);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].subarrayRegion.Yo);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].subarrayRegion.dX);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].subarrayRegion.dY);

                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].thresholdRegion.Xo);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].thresholdRegion.Yo);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].thresholdRegion.dX);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].thresholdRegion.dY);

                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].postreadEnabled);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].subInjEnabled);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].readEnabled);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].thresholdEnabled);

                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].timeResolvedEnabled);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].adaptiveEnabled);

                    // padd with spares
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].subarrayIdSupplied);
                    binaryWriter.Write(exposureData.subarrayList[subarrayIndex].spare2);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct BufferType
        {
            /// <summary>
            /// The buffer
            /// </summary>
            public byte[] buffer;
        }
    }
}
