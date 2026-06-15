using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HDF5DotNet;

namespace KronosCameraTestApp
{
    class CID821HDF5
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private uint[] intDataList = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CID821HDF5"/> class.
        /// </summary>
        public CID821HDF5()
        {
            uint[] data = new uint[2048];
            uint index = 0;
            for (uint ii = 0; ii < data.Length; ii += 2) {
                data[ii] = index | (index << 18) | (index << 27);
                data[ii + 1] = index;
                ++index;
            }

            intDataList = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CID821HDF5" /> class.
        /// </summary>
        /// <param name="intDataList">The int data list.</param>
        public CID821HDF5(uint[] intDataList)
        {
            this.intDataList = intDataList;
        }

        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">File extension not recognized.</exception>
        public CID821HDF5 LoadFromFile(string filename)
        {

            switch (System.IO.Path.GetExtension(filename).ToLower()) {
                case ".h5":
                    intDataList = LoadHDF5(filename);
                    break;
               
                default:
                    throw new System.ArgumentException("File extension not recognized.");
            }

            if (intDataList != null)
                return new CID821HDF5(intDataList);

            // Future proof.
            return null;
        }

        /// <summary>
        /// Loads the HD f5.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        private  uint[] LoadHDF5(string filename)
        {
            HDF5DotNet.H5.Open();
            HDF5DotNet.H5FileId fileId = HDF5DotNet.H5F.open(filename, HDF5DotNet.H5F.OpenMode.ACC_RDONLY);
            HDF5DotNet.H5DataTypeId dataTypeId = HDF5DotNet.H5T.copy(HDF5DotNet.H5T.H5Type.STD_U32LE);
            HDF5DotNet.H5DataSetId dataSetId = HDF5DotNet.H5D.open(fileId, "CID821Data");
            HDF5DotNet.H5DataSpaceId dataSpaceId = HDF5DotNet.H5D.getSpace(dataSetId);

            int len = HDF5DotNet.H5S.getSimpleExtentNPoints(dataSpaceId);
            uint[] intDataList = new uint[len];

            HDF5DotNet.H5Array<uint> htiming = new HDF5DotNet.H5Array<uint>(intDataList);
            HDF5DotNet.H5D.read<uint>(dataSetId, dataTypeId, htiming);
            HDF5DotNet.H5D.close(dataSetId);
            HDF5DotNet.H5T.close(dataTypeId);
            HDF5DotNet.H5S.close(dataSpaceId);
            HDF5DotNet.H5F.close(fileId);
            HDF5DotNet.H5.Close();

            return intDataList;
        }

        /// <summary>
        /// Saves the data to HD f5.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="cid821DataList">The cid821 data list.</param>
        public void SaveDataToHDF5(string filename, List<CID821_Data> cid821DataList)
        {
            HDF5DotNet.H5.Open();
            HDF5DotNet.H5FileId fileId = HDF5DotNet.H5F.create(filename, HDF5DotNet.H5F.CreateMode.ACC_TRUNC);

            // create the hdf5 object
            HDF5DotNet.H5DataSpaceId dataSpaceId = HDF5DotNet.H5S.create_simple(1, new long[] { intDataList.Length });
            HDF5DotNet.H5DataTypeId dataTypeId = HDF5DotNet.H5T.copy(HDF5DotNet.H5T.H5Type.STD_U32LE);
            HDF5DotNet.H5DataSetId dataSetId = HDF5DotNet.H5D.create(fileId, "CID821Data", dataTypeId, dataSpaceId);
            HDF5DotNet.H5DataTypeId vltype = HDF5DotNet.H5T.vlenCreate(dataTypeId);
            
            // write the data, if it exist, to the file
            if (cid821DataList != null)
            {
                WriteInt(fileId, "ListLength", cid821DataList.ToArray().Length);
                WriteInt(fileId, "FileVersion_", cid821DataList[0].fileVersion);
                for (int i = 0; i < cid821DataList.ToArray().Length; i++)
                {
                    CID821_Data cid821Element = cid821DataList[i];
                  
                    //WriteString(fileId, "Description_" + i.ToString(), cid821Element.dataDescription);
                    //WriteString(fileId, "Description_Data" + i.ToString() , "Frame");
                    WriteInt(fileId, "Yo_" + i.ToString(), cid821Element.column);
                    WriteInt(fileId, "Xo_" + i.ToString(), cid821Element.row);
                    WriteInt(fileId, "dY_" + i.ToString(), cid821Element.dc);
                    WriteInt(fileId, "dX_" + i.ToString(), cid821Element.dr);
                    WriteString(fileId, "SubarrayName_" + i.ToString(), cid821Element.subarrayName);

                    //WriteDoubleArray(fileId, "Voltages_" + i.ToString(), cid821Element.voltages);                    
                    Write2DDoubleArray(fileId, "videoDataList_" + i.ToString(), cid821Element.videoDataList);
                }
            }
            else
            {
                log.Error("SaveDataToHDF5 : No data in list to save");
                return;
            }

            // close everything
            HDF5DotNet.H5D.close(dataSetId);
            HDF5DotNet.H5T.close(dataTypeId);
            HDF5DotNet.H5S.close(dataSpaceId);
            HDF5DotNet.H5F.close(fileId);
            HDF5DotNet.H5.Close();
        }

        static bool WriteString(HDF5DotNet.H5FileId file_id, string section, string value)
        {
            HDF5DotNet.H5Array<byte> hdf5_array = new HDF5DotNet.H5Array<byte>(new ASCIIEncoding().GetBytes(value));

            HDF5DotNet.H5DataTypeId string_id = HDF5DotNet.H5T.copy(HDF5DotNet.H5T.H5Type.C_S1);

            HDF5DotNet.H5T.setSize(string_id, value.Length);

            HDF5DotNet.H5DataSpaceId dataspace_id = HDF5DotNet.H5S.create(H5S.H5SClass.SCALAR);

            HDF5DotNet.H5PropertyListId plist_id = HDF5DotNet.H5P.create(HDF5DotNet.H5P.PropertyListClass.LINK_CREATE);
            HDF5DotNet.H5P.SetIntermediateGroupCreation(plist_id, true);

            HDF5DotNet.H5PropertyListId default_plist_id = new HDF5DotNet.H5PropertyListId(HDF5DotNet.H5P.Template.DEFAULT);
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.create(file_id, section, string_id, dataspace_id, plist_id,
                                                                      default_plist_id, default_plist_id);
            HDF5DotNet.H5D.write<byte>(dataset_id, string_id, hdf5_array);

            HDF5DotNet.H5D.close(dataset_id);
            HDF5DotNet.H5P.close(default_plist_id);  // Not sure this needs to be closed.
            HDF5DotNet.H5P.close(plist_id);
            HDF5DotNet.H5S.close(dataspace_id);
            HDF5DotNet.H5T.close(string_id);

            return false;
        }

        static bool ReadString(HDF5DotNet.H5FileId file_id, string section, out string value)
        {
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.open(file_id, section);
            HDF5DotNet.H5DataTypeId string_id = HDF5DotNet.H5D.getType(dataset_id);

            int string_length = HDF5DotNet.H5T.getSize(string_id);
            byte[] data = new byte[string_length];
            HDF5DotNet.H5Array<byte> hdf5_array = new HDF5DotNet.H5Array<byte>(data);
            H5D.read<byte>(dataset_id, string_id, hdf5_array);
            value = new ASCIIEncoding().GetString(data);

            HDF5DotNet.H5T.close(string_id);
            HDF5DotNet.H5D.close(dataset_id);

            return false;
        }

        static bool WriteInt(HDF5DotNet.H5FileId file_id, string section, int value)
        {
            HDF5DotNet.H5DataTypeId datatype_id = new HDF5DotNet.H5DataTypeId(H5T.H5Type.NATIVE_INT);
            HDF5DotNet.H5DataSpaceId dataspace_id = HDF5DotNet.H5S.create(H5S.H5SClass.SCALAR);

            HDF5DotNet.H5PropertyListId plist_id = HDF5DotNet.H5P.create(HDF5DotNet.H5P.PropertyListClass.LINK_CREATE);
            HDF5DotNet.H5P.SetIntermediateGroupCreation(plist_id, true);

            HDF5DotNet.H5PropertyListId default_plist_id = new HDF5DotNet.H5PropertyListId(HDF5DotNet.H5P.Template.DEFAULT);
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.create(file_id, section, datatype_id, dataspace_id, plist_id,
                                                                      default_plist_id, default_plist_id);

            HDF5DotNet.H5D.writeScalar<int>(dataset_id, datatype_id, ref value);

            HDF5DotNet.H5D.close(dataset_id);
            HDF5DotNet.H5P.close(default_plist_id);  // Not sure this needs to be closed.
            HDF5DotNet.H5P.close(plist_id);
            HDF5DotNet.H5S.close(dataspace_id);
            //HDF5DotNet.H5T.close(datatype_id);    // We're using NATIVE_INT, don't close this.

            return false;
        }

        static bool ReadInt(HDF5DotNet.H5FileId file_id, string section, out int value)
        {
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.open(file_id, section);
            HDF5DotNet.H5DataTypeId datatype_id = HDF5DotNet.H5D.getType(dataset_id);

            int temp = 0;
            H5D.readScalar<int>(dataset_id, datatype_id, ref temp);
            value = temp;

            HDF5DotNet.H5T.close(datatype_id);
            HDF5DotNet.H5D.close(dataset_id);

            return false;
        }

        static bool WriteUInt(HDF5DotNet.H5FileId file_id, string section, uint value)
        {
            HDF5DotNet.H5DataTypeId datatype_id = new HDF5DotNet.H5DataTypeId(H5T.H5Type.NATIVE_UINT);
            HDF5DotNet.H5DataSpaceId dataspace_id = HDF5DotNet.H5S.create(H5S.H5SClass.SCALAR);

            HDF5DotNet.H5PropertyListId plist_id = HDF5DotNet.H5P.create(HDF5DotNet.H5P.PropertyListClass.LINK_CREATE);
            HDF5DotNet.H5P.SetIntermediateGroupCreation(plist_id, true);

            HDF5DotNet.H5PropertyListId default_plist_id = new HDF5DotNet.H5PropertyListId(HDF5DotNet.H5P.Template.DEFAULT);
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.create(file_id, section, datatype_id, dataspace_id, plist_id,
                                                                      default_plist_id, default_plist_id);

            HDF5DotNet.H5D.writeScalar<uint>(dataset_id, datatype_id, ref value);

            HDF5DotNet.H5D.close(dataset_id);
            HDF5DotNet.H5P.close(default_plist_id);  // Not sure this needs to be closed.
            HDF5DotNet.H5P.close(plist_id);
            HDF5DotNet.H5S.close(dataspace_id);

            return false;
        }

        static bool ReadUInt(HDF5DotNet.H5FileId file_id, string section, out uint value)
        {
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.open(file_id, section);
            HDF5DotNet.H5DataTypeId datatype_id = HDF5DotNet.H5D.getType(dataset_id);

            uint temp = 0;
            H5D.readScalar<uint>(dataset_id, datatype_id, ref temp);
            value = temp;

            HDF5DotNet.H5T.close(datatype_id);
            HDF5DotNet.H5D.close(dataset_id);

            return false;
        }

        static bool WriteDoubleArray(HDF5DotNet.H5FileId file_id, string section, double[] array)
        {
            HDF5DotNet.H5Array<double> hdf5_array = new HDF5DotNet.H5Array<double>(array);

            HDF5DotNet.H5DataTypeId datatype_id = HDF5DotNet.H5T.copy(HDF5DotNet.H5T.H5Type.NATIVE_DOUBLE);
            
            HDF5DotNet.H5DataSpaceId dataspace_id = HDF5DotNet.H5S.create_simple(1, new long[] { array.Length });

            HDF5DotNet.H5PropertyListId plist_id = HDF5DotNet.H5P.create(HDF5DotNet.H5P.PropertyListClass.LINK_CREATE);
            HDF5DotNet.H5P.SetIntermediateGroupCreation(plist_id, true);

            HDF5DotNet.H5PropertyListId default_plist_id = new HDF5DotNet.H5PropertyListId(HDF5DotNet.H5P.Template.DEFAULT);
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.create(file_id, section, datatype_id, dataspace_id, plist_id,
                                                                      default_plist_id, default_plist_id);

            HDF5DotNet.H5D.write<double>(dataset_id, datatype_id, hdf5_array);

            HDF5DotNet.H5D.close(dataset_id);
            HDF5DotNet.H5P.close(default_plist_id);  // Not sure this needs to be closed.
            HDF5DotNet.H5P.close(plist_id);
            HDF5DotNet.H5S.close(dataspace_id);
            HDF5DotNet.H5T.close(datatype_id);

            return false;
        }

        private double[] ReadDoubleArray(HDF5DotNet.H5FileId file_id, string section)
        {
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.open(file_id, section);
            HDF5DotNet.H5DataTypeId datatype_id = HDF5DotNet.H5D.getType(dataset_id);
            HDF5DotNet.H5T.H5TClass class_id = H5T.getClass(datatype_id);
            double[] array;
            if (class_id != H5T.H5TClass.FLOAT)
            {
                array = null;
                HDF5DotNet.H5T.close(datatype_id);
                HDF5DotNet.H5D.close(dataset_id);
                return array;
            }

            HDF5DotNet.H5DataSpaceId dataspace_id = H5D.getSpace(dataset_id);

            int length = H5S.getSimpleExtentNPoints(dataspace_id);
            array = new double[length];

            HDF5DotNet.H5Array<double> hdf5_array = new HDF5DotNet.H5Array<double>(array);
            H5D.read<double>(dataset_id, datatype_id, hdf5_array);
        
            HDF5DotNet.H5S.close(dataspace_id);
            HDF5DotNet.H5T.close(datatype_id);
            HDF5DotNet.H5D.close(dataset_id);
            return array;
        }

        private ushort[,] rotate90degrees(ushort[,] array)
        {
            ushort[,] data90 = new ushort[array.GetLength(0), array.GetLength(1)];

            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    data90[x, y] = array[y, (array.GetLength(0) - 1) - x];
                }
            }
            return data90;
        }

        static bool Write2DDoubleArray(HDF5DotNet.H5FileId file_id, string section, ushort[,] array)
        {
           // HDF5DotNet.H5Array<ushort>hdf5_array = new HDF5DotNet.H5Array<ushort>(array);
    
            HDF5DotNet.H5DataTypeId datatype_id = HDF5DotNet.H5T.copy(HDF5DotNet.H5T.H5Type.NATIVE_USHORT);

            HDF5DotNet.H5DataSpaceId dataspace_id = HDF5DotNet.H5S.create_simple(2, new long[] { array.GetLength(0), array.GetLength(1) });

            HDF5DotNet.H5PropertyListId plist_id = HDF5DotNet.H5P.create(HDF5DotNet.H5P.PropertyListClass.LINK_CREATE);
            HDF5DotNet.H5P.SetIntermediateGroupCreation(plist_id, true);

            HDF5DotNet.H5PropertyListId default_plist_id = new HDF5DotNet.H5PropertyListId(HDF5DotNet.H5P.Template.DEFAULT);
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.create(file_id, section, datatype_id, dataspace_id, plist_id,
                                                                      default_plist_id, default_plist_id);

            // rotate 90 degrees
            // ushort[,] array90 = new ushort[array.GetLength(0), array.GetLength(1)];
            // array90 = rotate90degrees(array);

           // HDF5DotNet.H5D.write<double>(dataset_id, datatype_id, new H5Array<double>(array)); 
            HDF5DotNet.H5D.write<ushort>(dataset_id, datatype_id, new H5Array<ushort>(array)); 

            HDF5DotNet.H5D.close(dataset_id);
            HDF5DotNet.H5P.close(default_plist_id);  // Not sure this needs to be closed.
            HDF5DotNet.H5P.close(plist_id);
            HDF5DotNet.H5S.close(dataspace_id);
            HDF5DotNet.H5T.close(datatype_id);

            return false;
        }

        static bool WriteUintArray(HDF5DotNet.H5FileId file_id, string section, uint[] array)
        {
            HDF5DotNet.H5Array<uint> hdf5_array = new HDF5DotNet.H5Array<uint>(array);

            HDF5DotNet.H5DataTypeId datatype_id = HDF5DotNet.H5T.copy(HDF5DotNet.H5T.H5Type.NATIVE_UINT);
            HDF5DotNet.H5DataSpaceId dataspace_id = HDF5DotNet.H5S.create_simple(1, new long[] { array.Length });

            HDF5DotNet.H5PropertyListId plist_id = HDF5DotNet.H5P.create(HDF5DotNet.H5P.PropertyListClass.LINK_CREATE);
            HDF5DotNet.H5P.SetIntermediateGroupCreation(plist_id, true);

            HDF5DotNet.H5PropertyListId default_plist_id = new HDF5DotNet.H5PropertyListId(HDF5DotNet.H5P.Template.DEFAULT);
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.create(file_id, section, datatype_id, dataspace_id, plist_id,
                                                                      default_plist_id, default_plist_id);

            HDF5DotNet.H5D.write<uint>(dataset_id, datatype_id, hdf5_array);

            HDF5DotNet.H5D.close(dataset_id);
            HDF5DotNet.H5P.close(default_plist_id);  // Not sure this needs to be closed.
            HDF5DotNet.H5P.close(plist_id);
            HDF5DotNet.H5S.close(dataspace_id);
            HDF5DotNet.H5T.close(datatype_id);

            return false;
        }

        static bool ReadUintArray(HDF5DotNet.H5FileId file_id, string section, out uint[] array)
        {
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.open(file_id, section);
            HDF5DotNet.H5DataTypeId datatype_id = HDF5DotNet.H5D.getType(dataset_id);
            HDF5DotNet.H5T.H5TClass class_id = H5T.getClass(datatype_id);
            HDF5DotNet.H5DataSpaceId dataspace_id = H5D.getSpace(dataset_id);
            long length = H5S.getSimpleExtentNPoints(dataspace_id);


            if (class_id != H5T.H5TClass.INTEGER)
            {
                array = null;
                HDF5DotNet.H5T.close(datatype_id);
                HDF5DotNet.H5D.close(dataset_id);
                return true;
            }

            uint[] data = new uint[length];
            HDF5DotNet.H5Array<uint> hdf5_array = new HDF5DotNet.H5Array<uint>(data);
            H5D.read<uint>(dataset_id, datatype_id, hdf5_array);
            array = data;

            HDF5DotNet.H5S.close(dataspace_id);
            HDF5DotNet.H5T.close(datatype_id);
            HDF5DotNet.H5D.close(dataset_id);

            return false;
        }

        static bool WriteImage(HDF5DotNet.H5FileId file_id, string section, double[] array)
        {
            HDF5DotNet.H5Array<double> hdf5_array = new HDF5DotNet.H5Array<double>(array);
            HDF5DotNet.H5DataTypeId datatype_id = HDF5DotNet.H5T.copy(HDF5DotNet.H5T.H5Type.NATIVE_DOUBLE);
            HDF5DotNet.H5DataSpaceId dataspace_id = HDF5DotNet.H5S.create_simple(1, new long[] { array.Length });
            HDF5DotNet.H5PropertyListId plist_id = HDF5DotNet.H5P.create(HDF5DotNet.H5P.PropertyListClass.LINK_CREATE);
            HDF5DotNet.H5P.SetIntermediateGroupCreation(plist_id, true);

            HDF5DotNet.H5PropertyListId default_plist_id = new HDF5DotNet.H5PropertyListId(HDF5DotNet.H5P.Template.DEFAULT);
            HDF5DotNet.H5DataSetId dataset_id = HDF5DotNet.H5D.create(file_id, section, datatype_id, dataspace_id, plist_id,
                                                                      default_plist_id, default_plist_id);

            HDF5DotNet.H5D.write<double>(dataset_id, datatype_id, hdf5_array);
            HDF5DotNet.H5D.close(dataset_id);
            HDF5DotNet.H5P.close(default_plist_id);  // Not sure this needs to be closed.
            HDF5DotNet.H5P.close(plist_id);
            HDF5DotNet.H5S.close(dataspace_id);
            HDF5DotNet.H5T.close(datatype_id);

            return false;
        }
    }
}
