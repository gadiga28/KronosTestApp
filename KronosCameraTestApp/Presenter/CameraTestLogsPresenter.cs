using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace KronosCameraTestApp.Presenter
{
   public class CameraTestLogsPresenter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        KronosTestAppMainForm kronosTestAppMainForm { get; set; }
        CameraTestLogDetails cameraTestLogDetails { get; set; }
        CameraTestLogsModel cameraTestLogs { get; set; }
        List<CameraDetailsModel> cameraDetailsModelList = new List<CameraDetailsModel>();
        List<AffectedItemsModel> affectedItemsModelList = new List<AffectedItemsModel>();
        List<CameraTestLogsModel> cameraTestLogsList = new List<CameraTestLogsModel>();
        List<CameraTestFailureModesMetricsModel> cameraTestFailureModesMetricsList = new List<CameraTestFailureModesMetricsModel>();      
        public CameraTestLogsPresenter(CameraTestLogDetails cameraTestLogDetails, CameraTestLogsModel cameraTestLogs, KronosTestAppMainForm kronosTestAppMainForm)
        {
            this.cameraTestLogDetails = cameraTestLogDetails;
            this.cameraTestLogs = cameraTestLogs;
            cameraTestLogDetails.cameraTestLogsPresenter = this;
            this.kronosTestAppMainForm = kronosTestAppMainForm;
            kronosTestAppMainForm.cameraTestLogsPresenter = this;
        }
        public async Task<List<CameraDetailsModel>> GetCameraDetails()
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        cameraDetailsModelList = (from cameras in kcc.cameraDetailsModel
                                                  orderby cameras.DateDefined descending
                                                  select cameras).Distinct().ToList();
                        return cameraDetailsModelList;
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
        public async Task<List<CameraTestLogsModel>> GetCameraTestLogs(string cameraSN, string imagerSN)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    cameraTestLogsList = new List<CameraTestLogsModel>();
                    using (var kcc = new KronosCamContext())
                    {
                        if(!string.IsNullOrWhiteSpace(cameraSN))
                        {
                            cameraDetailsModelList = (from cameras in kcc.cameraDetailsModel
                                                      where cameras.CameraSerialNumber.Equals(cameraSN)
                                                      orderby cameras.DateDefined descending
                                                      select cameras).ToList();
                        }
                        else if(!string.IsNullOrWhiteSpace(imagerSN))
                        {
                            cameraDetailsModelList = (from cameras in kcc.cameraDetailsModel
                                                      where cameras.ImagerSerialNumber.Equals(imagerSN)
                                                      orderby cameras.DateDefined descending
                                                      select cameras).ToList();
                        }                            
                        //else
                        //{
                        //    cameraDetailsModelList = (from cameras in kcc.cameraDetailsModel
                        //                              orderby cameras.DateDefined descending
                        //                              select cameras).ToList();
                        //}
                        if(cameraDetailsModelList.Count() > 0)
                        {
                            cameraTestLogsList.Clear();
                            foreach (CameraDetailsModel camera in cameraDetailsModelList)
                            {
                                CameraTestLogsModel cameraTestLog = new CameraTestLogsModel();
                                cameraTestLog = (from cameraLog in kcc.cameraTestLogs
                                                 where cameraLog.CameraTestID.Equals(camera.CameraTestID)
                                                 select cameraLog).FirstOrDefault();
                                if (cameraTestLog != null)
                                {
                                    cameraTestLogsList.Add(cameraTestLog);
                                }
                            }
                        }
                        
                        return cameraTestLogsList;
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
        public async Task<CameraTestFailureModesMetricsModel> GetCameraTestFailureModesMetrics(int resultsID)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    CameraTestFailureModesMetricsModel cameraTestFailureModesMetricsModel = new CameraTestFailureModesMetricsModel();
                    cameraTestFailureModesMetricsList = new List<CameraTestFailureModesMetricsModel>();
                    using (var kcc = new KronosCamContext())
                    {
                        if (resultsID > 0)
                            cameraTestFailureModesMetricsModel = (from camera in kcc.cameraTestFailureModesMetricsModel
                                                                  where camera.ResultsID.Equals(resultsID) select camera).FirstOrDefault();                                                     
                        return cameraTestFailureModesMetricsModel;
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
        public async Task<bool> updateCameraTestLogsInDB(CameraTestLogsModel cameraTestLogs)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        log.Info("Begin update camera test log metrics");                      
                        kcc.cameraTestLogs.Attach(cameraTestLogs);
                        kcc.Entry(cameraTestLogs).State = System.Data.Entity.EntityState.Modified;
                        int savedTestResults = kcc.SaveChanges();
                        if (savedTestResults > 0)
                        {                
                            log.Info("Created a new user account in DB");
                            log.Info("End update camera test log metrics");
                            return true;
                        }
                        else
                        {
                            log.Error("Error occured while creating a new user account in DB");
                            log.Info("End update camera test log metrics");
                            return false;
                        }
                    }
                });
                return res;
            }
            catch (DbUpdateException ex)
            {               
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
            catch (Exception ex)
            {              
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        public async Task<bool> updateCameraFailureModesMetrics(CameraTestFailureModesMetricsModel cameraTestFailureModesMetricsModel,string operationType)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        log.Info("Begin update camera failure modes metrics");
                        if(operationType.Equals("Update"))
                        {
                            kcc.cameraTestFailureModesMetricsModel.Attach(cameraTestFailureModesMetricsModel);
                            kcc.Entry(cameraTestLogs).State = System.Data.Entity.EntityState.Modified;
                        }
                        else if(operationType.Equals("Add"))
                        {
                            kcc.cameraTestFailureModesMetricsModel.Add(cameraTestFailureModesMetricsModel);
                        }
                        int savedTestResults = kcc.SaveChanges();                      
                        if (savedTestResults > 0)
                        {
                            log.Info("Updated/Added camera failure modes metrics");
                            return true;
                        }
                        else
                        {
                            log.Error("Error occured while camera failure modes metrics");
                            return false;
                        }
                    }
                });
                return res;
            }
            catch (DbUpdateException ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        public async Task<List<AffectedItemsModel>> GetAffectedItems()
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        affectedItemsModelList = (from items in kcc.affectedItemsModel
                                                  orderby items.ItemName descending
                                                  select items).Distinct().ToList();
                        affectedItemsModelList.Sort((x,y)=> string.Compare(x.ItemName,y.ItemName));                        
                        return affectedItemsModelList;
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
        public async Task<bool> addNewAffectedItemInDB(List<AffectedItemsModel> newAffectedItemsList)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        log.Info("Begin insert new affected Items");         
                        foreach(AffectedItemsModel affectedItemsModel in newAffectedItemsList)
                        {
                            kcc.affectedItemsModel.Add(affectedItemsModel);
                        }                       
                       
                        int savedTestResults = kcc.SaveChanges();
                        if (savedTestResults > 0)
                        {
                            log.Info("inserted new affected Items");
                            return true;
                        }
                        else
                        {
                            log.Error("Error occured while inserting new affected Items");
                            return false;
                        }
                    }
                });
                return res;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
    }
}
