using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Thermo.Kronos.Instrument.Camera.Interface;
using Thermo.Kronos.Instrument.Camera.Contracts.Enums;

namespace CIDSoftwareApplication
{
    /// <summary>
    ///  CID821 data class defines the video data returned from the detector
    /// </summary>
    [Serializable()]
    public class CID821_Data //: ISerializable
    {
        // Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CID821_Data"/> class.
        /// </summary>
        public CID821_Data ()
        {
        }

        // Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CID821_Data"/> class.
        /// </summary>
        public CID821_Data(SCM5821AX1DataTypes.VideoStruct videoStructure)
        {
            subarrayTimeStamp = videoStructure.timeStampUs;
            dc = (int)videoStructure.region.dY;
            dr = (int)videoStructure.region.dX;
            row = (int)videoStructure.region.Xo;
            column = (int)videoStructure.region.Yo;
            // Convert the 2D list, for now.
            int dataIndex = 0;
            for (Int32 indexX = row; indexX < dr + row; indexX++)
            {
                for (Int32 indexY = column; indexY < dc + column; indexY++)
                {
                    videoDataList[indexX, indexY] = videoStructure.data[dataIndex];
                    dataIndex++;
                }
            }
            //videoDataList = videoStructure.data;
        }

        /// <summary>
        /// Gets or sets the file version number.
        /// </summary>
        /// <value>
        /// The file version.
        /// </value>
        public int fileVersion { get; set; }

        /// <summary>
        /// Gets or sets the data description.
        /// </summary>
        /// <value>
        /// The data description.
        /// </value>
        /// 
        public string dataDescription { get; set; }
 
        /// <summary>
        /// Gets or sets the raw plot data.
        /// </summary>
        /// <value>
        /// The light data list.
        /// </value>
        public Int32[,] videoDataList { get; set; }

        /// <summary>
        /// Gets or sets the integrated data plots.
        /// </summary>
        public Int32[,] videoIntegratedDataList { get; set; }

        /// <summary>
        /// Gets or sets the offset voltages.
        /// </summary>
        /// <value>
        /// The offset voltages.
        /// </value>
        public double[] voltages { get; set; }

        /// <summary>
        /// Gets or sets the subarray time stamp.
        /// </summary>
        /// <value>
        /// The subarray time stamp.
        /// </value>
        public double subarrayTimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the subarray sequence number.
        /// </summary>
        /// <value>
        /// The subarray sequence number.
        /// </value>
        public int subarraySequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the subarray number.
        /// </summary>
        /// <value>
        /// The subarray number.
        /// </value>
        public int subarrayNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the subarray.
        /// </summary>
        /// <value>
        /// The name of the subarray.
        /// </value>
        public string subarrayName { get; set; }

        /// <summary>
        /// Gets or sets the name of the exposure.
        /// </summary>
        /// <value>
        /// The name/number of the exposure.
        /// </value>
        public int exposureNumber { get; set; }


        /// <summary>
        /// The new int
        /// </summary>
        /// When a new field has to be added use VTS
        //[OptionalField(VersionAdded = 2)]
        // public int newInt;
        public TreeNode treeNode { get; set; }

        /// <summary>
        /// Gets or sets the row.
        /// </summary>
        /// <value>
        /// The row.
        /// </value>
        public int row {get;set;}

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>
        /// The column.
        /// </value>
        public int column {get;set;}

        /// <summary>
        /// Gets or sets the dr.
        /// </summary>
        /// <value>
        /// The dr.
        /// </value>
        public int dr{get;set;}

        /// <summary>
        /// Gets or sets the dc.
        /// </summary>
        /// <value>
        /// The dc.
        /// </value>
        public int dc{get;set;}

        /// <summary>
        /// Gets or sets the index of the ndro.
        /// </summary>
        /// <value>
        /// The index of the ndro.
        /// </value>
        public int ndroIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of ndros.
        /// </summary>
        /// <value>
        /// The number of ndros.
        /// </value>
        public int numberOfNdros { get; set; }

        /// <summary>
        /// Gets or sets the status structure.
        /// </summary>
        /// <value>
        /// The structure
        /// </value>
   //     public SCM5821AX1DataTypes.StatusStructure statusStruct { get; set; }


        public ImageType imageType { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("fileVersion", this.fileVersion);
            info.AddValue("dataDescription", this.dataDescription);
            info.AddValue("videoDataList", this.videoDataList);

            // test
            //  info.AddValue("newInt", this.newInt);
        }
    }
}