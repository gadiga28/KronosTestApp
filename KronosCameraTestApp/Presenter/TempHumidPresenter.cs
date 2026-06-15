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
    public class TempHumidPresenter
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<TemperatureHumidityLimitsData> TemperatureHumidityLimitsDataList { get; set; }
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        List<TempHumReadingModel> tempHumReadingModelList = new List<TempHumReadingModel>();
        TempHumReadingModel tempHumReadingModel { get; set; }
        public List<TempHumReadingModel> TempHumReadingModelList
        {
            get { return tempHumReadingModelList; }
            set { tempHumReadingModelList = value; }
        }
        public TempHumidPresenter()
        {

        }
        public TempHumidPresenter(KronosTestAppMainForm kronostestappMainForm)
        {
            this.kronostestappMainForm = kronostestappMainForm;
            kronostestappMainForm.tempHumidPresenter = this;
        }
        public async Task GetTempHumidLimits()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                    
                    await Task.Run(() =>
                    {                      
                        using (var kcc = new KronosCamContext())
                        {
                            TemperatureHumidityLimitsDataList = (from limits in kcc.temperatureHumidityLimitsData orderby limits.DefinedDate descending select limits).ToList();
                        }
                    });
                }
                else
                {
                   
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task<List<TempHumReadingModel>> GetTempHumReading(string Workstation = "")
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
                                tempHumReadingModelList = (from results in kcc.tempHumReadingModel
                                                                 orderby results.ReadingDateTime descending
                                                                 select results).ToList();
                            }
                            else
                            {
                                tempHumReadingModelList = (from results in kcc.tempHumReadingModel
                                                           where results.TestStation.Equals(Workstation)
                                                                 orderby results.ReadingDateTime descending
                                                                 select results).ToList();
                            }

                            return tempHumReadingModelList;
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
        public async Task<bool> SaveTempHumReading(TempHumReadingModel tempHumReadingModel)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        log.Info("Begin save temphum in DB");

                        kcc.tempHumReadingModel.Add(tempHumReadingModel);
                        int savedTestResults = kcc.SaveChanges();
                        if (savedTestResults > 0)
                        {
                            log.Info("Inserted temphum in DB");
                            return true;
                        }
                        else
                        {
                            log.Error("Error occured while inserting temphum in DB");
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
