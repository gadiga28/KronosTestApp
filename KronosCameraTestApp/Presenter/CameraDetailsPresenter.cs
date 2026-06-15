using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace KronosCameraTestApp.Presenter
{
    public class CameraDetailsPresenter
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);      
        KronosTestAppMainForm kronostestappMainForm{ get; set; }
        CameraDetailsModel cameraDetailsModel { get; set; }
        List<CameraDetailsModel> cameraDetailsList { get; set; }
        String path = string.Empty;
        string enggUserName = string.Empty;
        bool cameraDetailsDBChanged = false;
        public XDocument xmlDoc;
        public bool DBExists = false;       
        public CameraDetailsModel CameraDetailsModel
        {
            get { return cameraDetailsModel; }
            set { cameraDetailsModel = value; }
        }      
        public CameraDetailsPresenter(OperatorInputForm operatorInputForm, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                cameraDetailsList = new List<CameraDetailsModel>();
                operatorInputForm.cameraDetailsPresenter = this;
                kronostestappMainForm.cameraDetailsPresenter = this;
                this.kronostestappMainForm = kronostestappMainForm;
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);                             
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }

        public async Task<List<CameraDetailsModel>> GetCameraDetailsModel(string cameraSN)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    List<CameraDetailsModel> cameraDetailsModels = new List<CameraDetailsModel>();
                    using (var kcc = new KronosCamContext())
                    {
                        cameraDetailsModels = (from camera in kcc.cameraDetailsModel
                                               where camera.CameraSerialNumber.Equals(cameraSN)
                                               select camera).ToList();
                        return cameraDetailsModels;
                    }
                });
                return res;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public async Task UpdateCRANumber(List<CameraDetailsModel> cameraDetailsModels, string CRANumber)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        foreach (CameraDetailsModel cdm in cameraDetailsModels)
                        {
                            cdm.CRA = CRANumber;
                            cdm.UserExecuted = TestAppHelper.UserLoginName;
                            cdm.DateDefined = DateTime.Now;
                            kcc.cameraDetailsModel.Attach(cdm);
                            kcc.Entry(cdm).State = System.Data.Entity.EntityState.Modified;
                        }
                        kcc.SaveChangesAsync();
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
