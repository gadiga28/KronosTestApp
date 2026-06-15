using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace KronosCameraTestApp.Presenter
{
    public class EnvironmentalStatusPresenter
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<COMPortConfigurationModel> comPortConfigurationDataList { get; set; }
        List<EnvironmentalStatusLimitsData> environmentalStatusLimitsDataList { get; set; }
        public List<EnvironmentalStatusLimitsData> EnvironmentalStatusLimitsDataList
        {
            get { return environmentalStatusLimitsDataList; }
            set { environmentalStatusLimitsDataList = value; }
        }
        public List<COMPortConfigurationModel> COMPortConfigurationDataList
        {
            get { return comPortConfigurationDataList; }
            set { comPortConfigurationDataList = value; }
        }
        public COMPortConfigurationModel COMPortConfigurationModel { get; set; }
        EnvironmentalStatus environmentalStatus { get; set; }
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        EnvironmentalStatusModel environmentalStatusModel { get; set; }
        private List<string> COMPortList = new List<string>();
        public System.IO.Ports.SerialPort ssCoolingSerialPort = new SerialPort();
        public System.IO.Ports.SerialPort powerSupplySerialPort = new SerialPort();
        public System.IO.Ports.SerialPort flowMeterSerialPort = new SerialPort();
        bool runStatus = true;       
        public DispatcherTimer _flowMeterDispatcherTimer = new DispatcherTimer();
        public DispatcherTimer _powerSupplyDispatcherTimer = new DispatcherTimer();
        private string bkPowerCOMPort = string.Empty;
        private string flowMeterCOMPort = string.Empty;
        private string tcubeCOMPort = string.Empty;
        private string purgeFlowRate = string.Empty;
        private string[] comPorts;
        private string currentAmps = string.Empty;
        private string powerVolts = string.Empty;
        private string faultStatus = string.Empty;
        private string tankLevel = string.Empty;
        private string pwmCooling = string.Empty;
        private string fanSpeed = string.Empty;
        private string pumpTemp = string.Empty;
        private string setPointTemp = string.Empty;
        private string coolantTemp = string.Empty;
        private bool coolantTempPassed = false;
        private bool tankLevelPassed = false;
        private bool powerVoltsPassed = false;
        private bool currentAmpsPassed = false;
        private bool purgeFlowRatePassed = false;
        private bool envrionmentalStatusTestFailed = false;
        private List<double> ampsReadings = new List<double>();   
        public bool RunStatus
        {
            get { return runStatus; }
            set { runStatus = value; }
        }
        public EnvironmentalStatusPresenter()
        {

        }
        public EnvironmentalStatusPresenter(EnvironmentalStatus environmentalStatus, KronosTestAppMainForm kronostestappMainForm)
        {
            this.environmentalStatus = environmentalStatus;
            this.kronostestappMainForm = kronostestappMainForm;
            environmentalStatus.environmentalStatusPresenter = this;
            kronostestappMainForm.environmentalStatusPresenter = this;
            GetEnironmentalStatusLimits();
            //StartEnvironmentalStatusMonitoring();          
        }
        public async void StartEnvironmentalStatusMonitoring()
        {
            try
            {
                await getSerialPowerCOMPorts();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }

        }
        public async Task getSerialPowerCOMPorts()
        {
            try
            {
                await Task.Run(() =>
                {
                    comPorts = SerialPort.GetPortNames();
                    using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
                    {
                        var portdetails = searcher.Get().Cast<ManagementBaseObject>().ToList();
                        COMPortList = (from n in comPorts
                                       join p in portdetails on n equals p["DeviceID"].ToString()
                                       select n + " - " + p["Caption"]).ToList();
                    }
                    foreach (string ComPort in COMPortList)
                    {
                        if (ComPort.Contains("Silicon Labs"))
                        {
                            bkPowerCOMPort = ComPort.Substring(0, 4);
                        }
                    }                  

                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        public void DisconnectCOMPorts()
        {
            try
            {
                runStatus = false;
                if (ssCoolingSerialPort.IsOpen)
                {
                    ssCoolingSerialPort.Close();
                }
                if (powerSupplySerialPort.IsOpen)
                {
                    powerSupplySerialPort.Close();
                }
                if (flowMeterSerialPort.IsOpen)
                {
                    flowMeterSerialPort.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }

        }              
      
        public void GetEnironmentalStatusLimits()
        {
            try
            {
                using (var kcc = new KronosCamContext())
                {
                    environmentalStatusLimitsDataList = (from limits in kcc.environmentalStatusLimitsData select limits).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        public void GetCOMPortConfiguration()
        {
            try
            {
                using (var kcc = new KronosCamContext())
                {
                    COMPortConfigurationModel= kcc.comPortConfigurationModel.Where(machine => machine.StationName.Equals(Environment.MachineName)).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);               
            }
        }
        public async Task SaveCOMPortConfigurationToDB(COMPortConfigurationModel comPortConfigurationModel, bool add)
        {
            try
            {
                using (var kcc = new KronosCamContext())
                {
                    log.Info("Begin save COM Ports configuration in DB");

                    if(add)
                    {
                        kcc.comPortConfigurationModel.Add(comPortConfigurationModel);
                    }
                    else
                    {
                        kcc.comPortConfigurationModel.Attach(this.COMPortConfigurationModel);
                        kcc.Entry(comPortConfigurationModel).State = System.Data.Entity.EntityState.Modified;
                    }
                   
                    int savedTestResults = await kcc.SaveChangesAsync();
                    if (savedTestResults > 0)
                    {
                        log.Info("Saved COM Ports configuration in DB");
                    }
                    else
                    {
                        log.Error("Error occured while saving COM Ports configuration in DB");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }

    }
}
