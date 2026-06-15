using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data.Entity.Migrations;
using Thermo.Kronos.Instrument.Camera.Interface;
using System.Configuration;
using CIDSoftwareApplication;
using KronosCameraTestApp.View.UserControls;
namespace KronosCameraTestApp.Presenter
{
    public class ExposureDataPresenter
    {
        KronosTestAppMainForm kronostestappMainForm { get; set; }       
        //new comment
        CommandProcessor commandProcessor = null;      
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CIDInterface cidInterface = null;     
        String path = string.Empty;       
        string enggUserName = string.Empty;             
        string characterizationTest = string.Empty;
        string characterizationTestXML = string.Empty;       
        int errorCode = 0;
        public int ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
        List<ExposureDataModel> exposureDataList = new List<ExposureDataModel>();
        List<SubarrayDataModel> subarrayDataModelList = new List<SubarrayDataModel>();
        public List<ExposureDataModel> ExposureDataList
        {
            get { return exposureDataList; }
            set { exposureDataList = value; }
        }
        public List<SubarrayDataModel> SubarrayDataModelList
        {
            get { return subarrayDataModelList; }
            set { subarrayDataModelList = value; }
        }
        ExposureDataModel exposureDataModel { get; set; }
        SubarrayDataModel subarrayDataModel { get; set; }
        public ExposureDataModel ExposureDataModel
        {
            get { return exposureDataModel; }
            set { exposureDataModel = value; }
        }
        public SubarrayDataModel SubarrayDataModel
        {
            get { return subarrayDataModel; }
            set { subarrayDataModel = value; }
        }
        public ExposureDataPresenter(string characterizationTest)
        {
            this.characterizationTest = characterizationTest;
            exposureDataModel = new ExposureDataModel();
            subarrayDataModel = new SubarrayDataModel();
            subarrayDataModelList = new List<SubarrayDataModel>();
            exposureDataList = new List<ExposureDataModel>();
            commandProcessor = new CommandProcessor(cidInterface);
        }           
        public void GetExposureData(CIDInterface cidInterface)
        {
            try
            {
                this.cidInterface = cidInterface;
                if (TestAppHelper.CheckDBExists())
                {
                    PopulateExposureEntity();
                }
                else
                {
                     ProcessExposureXMLFile();
                    log.Info("Noise Vs NDROs test data populated from XML Repository.");                   
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void PopulateExposureEntity()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                   
                        using (var kcc = new KronosCamContext())
                        {
                                exposureDataList = kcc.exposureDataModel.Include("SubarrayDatas").Where(mv => mv.TestDataID.Equals(characterizationTest)).ToList();
                                List<int> exposureIDList = (from exp in exposureDataList.Where(e => e.TestDataID.Equals(characterizationTest)) select exp.ExposureID).ToList();
                                subarrayDataModelList = (from mvsu in kcc.subarrayDataModel orderby mvsu.ExposureID where exposureIDList.Contains(mvsu.ExposureID) select mvsu).ToList();
                                log.Info("Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                        }
                }
                else
                {                  
                        //get the data from script files
                         ProcessExposureXMLFile();
                       
                        log.Info("Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ProcessExposureXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading NoiseVsNDROData XML.");
                    commandProcessor.ProcessExposureXMLData("NoiseVsNDROData.xml");
                    exposureDataList = new List<ExposureDataModel>();
                    subarrayDataModelList = new List<SubarrayDataModel>();
                    exposureDataList = commandProcessor.ExposureDataList;
                    subarrayDataModelList = commandProcessor.SubarrayDataModelList;
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        public async Task<bool> SaveExposureDataToDB(string operationType, string userName, string xmlFile, string rootElementName)
        {
            var kcc = new KronosCamContext();
            try
            {
                var res = await Task.Run(() =>
                {
                    return commandProcessor.SaveExposureDataToDB(characterizationTest, operationType, xmlFile,
                        rootElementName, exposureDataModel, subarrayDataModel, userName);
                });
                return res;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var ctx = ((IObjectContextAdapter)kcc).ObjectContext;
                ctx.Refresh(RefreshMode.ClientWins, kcc.exposureDataModel);
                ctx.Refresh(RefreshMode.ClientWins, kcc.subarrayDataModel);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        public async Task<bool> SaveSubarrayDataToDB(string operationType, string userName, string xmlFile)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    return commandProcessor.SaveSubarrayDataToDB(characterizationTest, operationType, xmlFile,
                        exposureDataModel, subarrayDataModel, userName);
                });
                return res;
            }
            catch (DbUpdateException ex)
            {               
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                SqlException s = ex.InnerException.InnerException as SqlException;
                errorCode = s.Number;
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }       
    }
}
