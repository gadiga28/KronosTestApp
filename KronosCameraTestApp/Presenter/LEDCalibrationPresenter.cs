using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Presenter
{
    public class LEDCalibrationPresenter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LEDCalibrationTest ledCalibrationTest { get; set; }
        LEDCalibrationTestResults ledCalibrationTestResults { get; set; }
        KronosTestAppMainForm kronosTestAppMainForm { get; set; }
        List<LEDCalibrationTestResults> ledCalibrationTestResultsList = new List<LEDCalibrationTestResults>();
        public List<LEDCalibrationTestResults> LEDCalibrationTestResultsList
        {
            get { return ledCalibrationTestResultsList; }
            set { ledCalibrationTestResultsList = value; }
        }
        List<LEDCalibrationLimitsData> ledCalibrationLimitsDataList { get; set; }
        public List<LEDCalibrationLimitsData> LEDCalibrationLimitsDataList
        {
            get { return ledCalibrationLimitsDataList; }
            set { ledCalibrationLimitsDataList = value; }
        }
        public LEDCalibrationPresenter()
        {
        }
        public LEDCalibrationPresenter(LEDCalibrationTest ledCalibrationTest, LEDCalibrationTestResults ledCalibrationTestResults, 
                                        KronosTestAppMainForm kronosTestAppMainForm)
        {
            this.ledCalibrationTest = ledCalibrationTest;
            this.ledCalibrationTestResults = ledCalibrationTestResults;
            ledCalibrationTest.ledCalibrationPresenter = this;
            this.kronosTestAppMainForm = kronosTestAppMainForm;
            kronosTestAppMainForm.ledCalibrationPresenter = this;
        }
        public async Task GetLimits()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                    await Task.Run(() =>
                    {
                        //comment below line while unit testing
                        //kronosTestAppMainForm.SetDatabaseConnectivityStatusBarPanel();
                        using (var kcc = new KronosCamContext())
                        {
                            ledCalibrationLimitsDataList = (from limits in kcc.ledCalibrationLimitsData orderby limits.DefinedDate descending select limits).ToList();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task<List<LEDCalibrationTestResults>> GetLEDCalibrationResults(string Workstation="")
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                    var res = await Task.Run(() =>
                     {
                         using (var kcc = new KronosCamContext())
                         {
                             if (string.IsNullOrWhiteSpace(Workstation))
                             {
                                 ledCalibrationTestResultsList = (from results in kcc.ledCalibrationTestResults
                                                                  orderby results.DateExecuted descending
                                                                  select results).ToList();
                             }
                             else
                             {
                                 ledCalibrationTestResultsList = (from results in kcc.ledCalibrationTestResults
                                                                  where results.Workstation.Equals(Workstation)
                                                                  orderby results.DateExecuted descending
                                                                  select results).ToList();
                             }

                             return ledCalibrationTestResultsList;
                         }
                     });
                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public async Task<bool> SaveLEDCalibrationResults(LEDCalibrationTestResults ledCalibrationTestResults)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        log.Info("Begin save ledCalibrationTestResults in DB");                      
                       
                        kcc.ledCalibrationTestResults.Add(ledCalibrationTestResults);                     
                        int savedTestResults = kcc.SaveChanges();                       
                        if (savedTestResults > 0)
                        {
                            log.Info("Updated ledCalibrationTestResults in DB");                            
                            return true;
                        }
                        else
                        {
                            log.Error("Error occured while updating ledCalibrationTestResults in DB");
                            return false;
                        }
                    }
                });
                return res;
            }           
            catch (Exception ex)
            {
               // MessageBox.Show("Error creating User account", "Create User Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }

    }
}
