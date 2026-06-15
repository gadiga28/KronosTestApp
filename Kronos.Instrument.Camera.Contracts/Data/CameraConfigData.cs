using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary>
    /// Camera Config Data
    /// </summary>
    [Serializable]
    public class CameraConfigData
    {      
            /// <summary>
            /// Voltage structure for all avialble voltage readings from camera
            /// </summary>
            public Voltages Voltages{ get; set; }

            /// <summary>
            /// Flag that enables and disables the TE Cooler
            /// </summary>
            public int TecEnabled{ get; set; }

            /// <summary>
            /// Target temperature to set the TE Cooler in C
            /// </summary>
            public int TecTargetTemp { get; set; }

            /// <summary>
            /// Expected delay in exposure time created by opening the shutter
            /// </summary>
            public int OpenShutterSpeed { get; set; }

            /// <summary>
            /// Expected delay in exposure time created by closeing the shutter
            /// </summary>
            public int CloseShutterSpeed { get; set; }
        
    }
}
