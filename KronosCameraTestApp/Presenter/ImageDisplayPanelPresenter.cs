using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.Presenter
{
    public class ImageDisplayPanelPresenter
    {
        KronosTestAppMainForm kronostestappMainForm;
        ImageDisplayPanel imageDisplayPanel;
        CommandProcessor commandProcessor = null;   
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CIDInterface cidInterface = null;
        String path = string.Empty;        
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
        List<ExposureDataModel> exposureDataList = new List<ExposureDataModel>();       
        public List<ExposureDataModel> ExposureDataList
        {
            get { return exposureDataList; }
            set { exposureDataList = value; }
        }
        int exposureCount = 0;       
        public ImageDisplayPanelPresenter(ImageDisplayPanel imageDisplayPanel, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                this.imageDisplayPanel = imageDisplayPanel;
                this.kronostestappMainForm = kronostestappMainForm;
                imageDisplayPanel.imageDisplayPanelPresenter = this;
                kronostestappMainForm.imageDisplayPanelPresenter = this;              
                exposureDataList = new List<ExposureDataModel>();
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                commandProcessor = new CommandProcessor(cidInterface);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
          public async Task runImageTest(CIDInterface cidInterface, CancellationToken ct, int exposureCount = 1)
          {
              try
              {
                  if (!ct.IsCancellationRequested)
                  {
                      this.exposureCount = exposureCount;
                      log.Info("Begin Image test.");
                      this.cidInterface = cidInterface;
                      if (cidInterface != null)
                          cidInterface.ResetExposureId();
                      TestAppHelper.currentRunningTest = "ImageTest";
                      kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                      kronostestappMainForm.cidDataList.Clear();
                      kronostestappMainForm.cidIntegratedDataList.Clear();
                      Array.Clear(kronostestappMainForm.ZPatternCorrectedData, 0, kronostestappMainForm.ZPatternCorrectedData.Length);
                      if (!ct.IsCancellationRequested)
                      {                    
                          await commandProcessor.ProcessExposureData(exposureDataList, ct);
                          await commandProcessor.SendExposure(ct, exposureCount, cidInterface);
                      }
                      else
                      {
                          return;
                      }
                      log.Info("End Image test.");
                  }
                  else
                      return;
              }
              catch (Exception ex)
              {
                  log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
              }
          }
          public async Task CalculateImageTestData(CancellationToken ct)
          {
              try
              {
                  await Task.Run(() =>
                  {                     
                      log.Info("Begin Calcluate Mean Variance for Exposure Count " + exposureCount.ToString());
                      while (kronostestappMainForm.cidDataList.Count < exposureCount)
                      {
                          if (ct.IsCancellationRequested)
                          {                             
                              kronostestappMainForm.cidDataList.Clear();
                              kronostestappMainForm.cidIntegratedDataList.Clear();
                              return;
                          }
                      }
                  });
                  log.Info("End Calcluate Mean Variance for Exposure Count " + exposureCount.ToString());
                  kronostestappMainForm.cidDataList.Clear();
                  kronostestappMainForm.cidIntegratedDataList.Clear();
              }
              catch (Exception ex)
              {
                  kronostestappMainForm.cidDataList.Clear();
                  kronostestappMainForm.cidIntegratedDataList.Clear();
                  log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
              }
          }
    }
}
