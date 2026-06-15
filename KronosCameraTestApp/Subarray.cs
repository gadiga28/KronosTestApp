using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIDSoftwareApplication
{
    /// <summary>
    /// Helper Class to define the Region of a Subarray
    /// </summary>
    class SubarrayRegion
    {
      //  public string Caption { get; set; }

        /// <summary>
        /// Structure definition
        /// </summary>
        public struct SubarrayRegionStructure
        {
            /// <summary>
            /// The start x
            /// </summary>
            public double StartX;
            /// <summary>
            /// The start y
            /// </summary>
            public double StartY;
            /// <summary>
            /// The end x
            /// </summary>
            public double EndX;
            /// <summary>
            /// The end y
            /// </summary>
            public double EndY;

            public uint subarrayNumber;
        }
       
        public SubarrayRegion()
        {
            //SubarraysubarrayRegionList = new List<KeyValuePair<string, SubarrayRegion>>();
        }
    }
}
