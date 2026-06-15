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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Thermo.Kronos.Instrument.Camera.Interface;
using System.Data.Entity.Migrations;
using System.IO;
using System.Drawing;
using System.Configuration;
using CIDSoftwareApplication;
namespace KronosCameraTestApp.Presenter
{
	public class NoiseVsNDROPresenter
	{
		NoiseVsNDROTestForm noiseVsNDROTestForm;
		KronosTestAppMainForm kronostestappMainForm;
		NoiseVsNDROTestResults noiseVsNDROTestResults { get; set; }
		bool noiseVsNDROsTestPassed = true;
		public bool NoiseVsNDROsTestPassed
		{
			get { return noiseVsNDROsTestPassed; }
			set { noiseVsNDROsTestPassed = value; }
		}
		string testResultImagePath = string.Empty;
		public string TestResultImagePath
		{
			get { return testResultImagePath; }
			set { testResultImagePath = value; }
		}
		string enggUserName = string.Empty;
		public string EnggUserName
		{
			get { return enggUserName; }
			set { enggUserName = value; }
		}
		NoiseVsNDROSLimitsData noiseVsNDROSLimitsData { get; set; }
		public NoiseVsNDROSLimitsData NoiseVsNDROSLimitsData
		{
			get { return noiseVsNDROSLimitsData; }
			set { noiseVsNDROSLimitsData = value; }
		}
		List<NoiseVsNDROSLimitsData> noiseVsNDROsLimitsDataList { get; set; }
		public List<NoiseVsNDROSLimitsData> NoiseVsNDROsLimitsDataList
		{
			get { return noiseVsNDROsLimitsDataList; }
			set { noiseVsNDROsLimitsDataList = value; }
		}
		public NoiseVsNDROTestResults NoiseVsNDROTestResults
		{
			get { return noiseVsNDROTestResults; }
			set { noiseVsNDROTestResults = value; }
		}
		List<NoiseVsNDROTestResults> noiseVsNDROTestResultsDataList { get; set; }
		public List<NoiseVsNDROTestResults> NoiseVsNDROTestResultsDataList
		{
			get { return noiseVsNDROTestResultsDataList; }
			set { noiseVsNDROTestResultsDataList = value; }
		}
		CommandProcessor commandProcessor = null;
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(
		   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		CIDInterface cidInterface = null;
		string correctedMeanData = string.Empty;
		int errorCode = 0;
		public XDocument xmlDoc;
		private const int BLACK_LEVEL = 16384;
		DataSet dsXML = new DataSet();
		string sql = null;
		XDocument xmlResultsDoc;
		double signalReadNoise = 0;
		public double SignalReadNoise
		{
			get { return signalReadNoise; }
			set { signalReadNoise = value; }
		}
		double nDROReadNoise = 0;
		public double NDROReadNoise
		{
			get { return nDROReadNoise; }
			set { nDROReadNoise = value; }
		}		
		bool noiseNDRODBChanged = false;
		public bool NoiseNDRODBChanged
		{
			get { return noiseNDRODBChanged; }
			set { noiseNDRODBChanged = value; }
		}
		int exposureCount = 0;
		public int ExposureCount
		{
			get { return exposureCount; }
			set { exposureCount = value; }
		}
		double[] noiseRatio;
		public double[] NoiseRatio
		{
			get { return noiseRatio; }
			set { noiseRatio = value; }
		}
		double r2Correlation = 0;
		public double R2Correlation
		{
			get { return r2Correlation; }
			set { r2Correlation = value; }
		}
		double[] r2forX;
		public double[] R2forX
		{
			get { return r2forX; }
			set { r2forX = value; }
		}
		double[] bestFit;
		double[] bestFitPower;
		double[] exposureNDRO;
		public double[] ExposureNDRO
		{
			get { return exposureNDRO; }
			set { exposureNDRO = value; }
		}
		double[] theoryValue;
		public double[] TheoryValue
		{
			get { return theoryValue; }
			set { theoryValue = value; }
		}
		double[] noise;
		public double[] Noise
		{
			get { return noise; }
			set { noise = value; }
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
		String path = string.Empty;
		public bool DBExists = false;
		bool testResultsSaved = true;
		string noisVsNDROsTestFailureReason = string.Empty;
		public string NoisVsNDROsTestFailureReason
		{
			get { return noisVsNDROsTestFailureReason; }
			set { noisVsNDROsTestFailureReason = value; }
		}
		public NoiseVsNDROPresenter(NoiseVsNDROTestForm noiseVsNDROTestForm, KronosTestAppMainForm kronostestappMainForm)
		{
			try
			{
				this.noiseVsNDROTestForm = noiseVsNDROTestForm;
				noiseVsNDROTestForm.noiseVsNDROPresenter = this;
				kronostestappMainForm.noiseVsNDROPresenter = this;
				exposureDataModel = new ExposureDataModel();
				subarrayDataModel = new SubarrayDataModel();
				subarrayDataModelList = new List<SubarrayDataModel>();
				exposureDataList = new List<ExposureDataModel>();
				this.kronostestappMainForm = kronostestappMainForm;
				path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
				commandProcessor = new CommandProcessor(cidInterface);
				noiseVsNDROTestResultsDataList = new List<NoiseVsNDROTestResults>();
				noiseVsNDROsLimitsDataList = new List<NoiseVsNDROSLimitsData>();
			}
			catch (Exception ex)
			{
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
		public async Task GetNoiseNDROData(CIDInterface cidInterface)
		{
			try
			{
				kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
				this.cidInterface = cidInterface;
				if (TestAppHelper.CheckDBExists())
				{
					await PopulateNoiseNDROEntity();
					DBExists = true;
				}
				else
				{
					await ProcessNoiseNDROXMLFile();
					log.Info("Noise Vs NDROs test data populated from XML Repository.");
					DBExists = false;
				}
			}
			catch (Exception ex)
			{
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
		public async Task runNoiseNDROTest(CIDInterface cidInterface, CancellationToken ct, string userMode, int exposureCount = 8)
		{
			try
			{
				if (!ct.IsCancellationRequested)
				{
                    this.cidInterface = cidInterface;
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    this.exposureCount = exposureCount;
					log.Info("Begin NoiseVsNDRO test.");
					if (cidInterface != null)
						cidInterface.ResetExposureId();
					noiseVsNDROsTestPassed = false;
					kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
					kronostestappMainForm.cidDataList.Clear();
					kronostestappMainForm.cidIntegratedDataList.Clear();
                    if (!ct.IsCancellationRequested)
					{
						//process the mean variance model data and populate exposure structures                      
						await commandProcessor.ProcessExposureData(exposureDataList, ct);
						//send exposure data to camera
						await commandProcessor.SendExposure(ct, exposureCount, cidInterface);
					}
					else
					{
						return;
					}
					log.Info("End NoiseVsNDRO test.");
				}
				else
					return;
			}
			catch (Exception ex)
			{
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
		public async Task ProcessNoiseVsNDROsResultsXMLFile()
		{
			try
			{
				await Task.Run(() =>
				{
					log.Info("Begin Reading NoiseVsNDROsResultsData XML.");
					if (testResultsSaved)
					{
						DataSet ds = new DataSet();
						ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "NoiseVsNDROsResultsData.xml", XmlReadMode.InferSchema);
						if (ds.Tables.Count > 0)
						{
							DataView dvExposure;
							dvExposure = ds.Tables[0].DefaultView;
							foreach (DataRowView dr in dvExposure)
							{
								NoiseVsNDROTestResults m = new NoiseVsNDROTestResults();
								m.ResultsID = Convert.ToInt32(dr[0]);
								m.TestResultsID = Convert.ToInt32(dr[1]);
								m.DateExecuted = Convert.ToDateTime(dr[2]);
								m.UserExecuted = Convert.ToString(dr[3]);
								m.ExposureNDRO = Convert.ToString(dr[4]);
								m.NoiseRatio = Convert.ToString(dr[5]);
								m.Noise = Convert.ToString(dr[6]);
								m.R2Correlation = Convert.ToDouble(dr[7]);
								m.BestFit = Convert.ToDouble(dr[8]);
								m.BestFitPower = Convert.ToDouble(dr[9]);
                                m.TheoryValue = Convert.ToString(dr[10]);
                                m.SignalReadNoise = Convert.ToDouble(dr[11]);
                                m.NDRONoise = Convert.ToDouble(dr[12]);
                                m.TestPassed = Convert.ToBoolean(dr[13]);
								m.TestResultImagePath = Convert.ToString(dr[14]);
								noiseVsNDROTestResultsDataList.Add(m);
							}
							if (noiseVsNDROTestResultsDataList.Count > 0)
							{
								log.Info("End Reading NoiseVsNDROsResultsData XML.");
								testResultsSaved = false;
							}
							else
							{
								log.Error("Error Loading NoiseVsNDROsResultsData xml");
								testResultsSaved = true;
							}
						}
					}
				});
			}
			catch (Exception ex)
			{
				testResultsSaved = true;
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
		public async Task WriteTestResultsToXML()
		{
			try
			{
				await Task.Run(() =>
				{
					log.Info("Begin saving Noise Vs NDROs Test Results to NoiseVsNDROsResultsData XML.");
					xmlResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "NoiseVsNDROsResultsData.xml");
					if (xmlResultsDoc != null)
					{
						xmlResultsDoc.Element("NoiseVsNDROsTestResults").Add
						(
							new XElement("TestResult",
							new XElement("ResultsID", DBExists ? noiseVsNDROTestResults.ResultsID : noiseVsNDROTestResultsDataList.Count + 1),
							new XElement("TestResultsID", noiseVsNDROTestResults.TestResultsID),
							new XElement("DateExecuted", noiseVsNDROTestResults.DateExecuted),
							new XElement("UserExecuted", noiseVsNDROTestResults.UserExecuted),
							new XElement("ExposureNDRO", noiseVsNDROTestResults.ExposureNDRO),
							new XElement("NoiseRatio", noiseVsNDROTestResults.NoiseRatio),
							new XElement("Noise", noiseVsNDROTestResults.Noise),
							new XElement("R2Correlation", noiseVsNDROTestResults.R2Correlation),
							new XElement("BestFit", noiseVsNDROTestResults.BestFit),
							new XElement("BestFitPower", noiseVsNDROTestResults.BestFitPower),
                            new XElement("TheoryValue", noiseVsNDROTestResults.TheoryValue),
                            new XElement("SignalReadNoise", noiseVsNDROTestResults.SignalReadNoise),
                            new XElement("NDRONoise", noiseVsNDROTestResults.NDRONoise),
                            new XElement("TestPassed", noiseVsNDROTestResults.TestPassed),
							new XElement("TestResultImage", noiseVsNDROTestResults.TestResultImagePath))
						);
						xmlResultsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "NoiseVsNDROsResultsData.xml");
						testResultsSaved = true;
						log.Info("End saving Noise Vs NDROs Test Results to NoiseVsNDROsResultsData XML.");
					}
					else
					{
						log.Error("NoiseVsNDROsResultsData xml is null");
						testResultsSaved = false;
					}
				});
			}
			catch (Exception ex)
			{
				testResultsSaved = false;
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
		public NoiseVsNDROTestResults ReturnNoiseVsNDROsResults()
		{
			try
			{
				noiseVsNDROTestResults = new NoiseVsNDROTestResults();
				noiseVsNDROTestResults = new NoiseVsNDROTestResults();
				noiseVsNDROTestResults.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
				noiseVsNDROTestResults.ExposureNDRO = String.Join(",", exposureNDRO.Select(p => p.ToString()).ToArray());
				noiseVsNDROTestResults.NoiseRatio = String.Join(",", noiseRatio.Select(p => p.ToString()).ToArray());
				noiseVsNDROTestResults.Noise = String.Join(",", noise.Select(p => p.ToString()).ToArray());
				noiseVsNDROTestResults.R2Correlation = r2Correlation;
				noiseVsNDROTestResults.BestFit = 0;
				noiseVsNDROTestResults.BestFitPower = 0;
                noiseVsNDROTestResults.TheoryValue= String.Join(",", theoryValue.Select(p => p.ToString()).ToArray());
                noiseVsNDROTestResults.SignalReadNoise = signalReadNoise;
                noiseVsNDROTestResults.NDRONoise = nDROReadNoise;
                noiseVsNDROTestResults.TestPassed = noiseVsNDROsTestPassed;
				noiseVsNDROTestResults.TestResultImagePath = testResultImagePath;
				noiseVsNDROTestResults.DateExecuted = DateTime.Now;
				noiseVsNDROTestResultsDataList.Add(noiseVsNDROTestResults);
				return noiseVsNDROTestResults;
			}
			catch (Exception ex)
			{
				testResultsSaved = false;
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
				return null;
			}
		}		
		public async Task PopulateNoiseNDROEntity()
		{
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                    DBExists = true;
                    await Task.Run(() =>
                    {
                        using (var kcc = new KronosCamContext())
                        {
                            exposureDataList = kcc.exposureDataModel.Include("SubarrayDatas").Where(mv => mv.TestDataID.Equals("RNT")).ToList();
                            List<int> exposureIDList = (from exp in exposureDataList.Where(e => e.TestDataID.Equals("RNT")) select exp.ExposureID).ToList();
                            subarrayDataModelList = (from mvsu in kcc.subarrayDataModel orderby mvsu.ExposureID where exposureIDList.Contains(mvsu.ExposureID) select mvsu).ToList();
                            log.Info("Mean Variance Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                        }
                    });
                }
                else
                {
                    await ProcessNoiseNDROXMLFile();
                    DBExists = false;
                    log.Info("Mean Variance Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
		public async Task CalculateNoiseNDRO(CancellationToken ct)
		{
			try
			{
				await Task.Run(() =>
				{
                    log.Info("Begin Calcluate NoiseNDRO for Exposure Count " + exposureCount.ToString());
                    bool firstPoint = true;
                    int k = 0;
                    while (kronostestappMainForm.cidDataList.Count < exposureCount * 2)
					{
						if (ct.IsCancellationRequested)
						{                           
                            kronostestappMainForm.cidDataList.Clear();
							kronostestappMainForm.cidIntegratedDataList.Clear();
							return;
						}
                        if (!cidInterface.IsConnected())
                        {
                            //MessageBox.Show("Camera got disconnected, Trying to reconnect.....");
                            log.Error("Camera disconnected during NoiseVSNDRO's test " + kronostestappMainForm.cidDataList.Count.ToString());
                            //cidInterface.Disconnect();
                            //kronostestappMainForm.AutoConnectCamera();
                            //if (cidInterface.IsConnected())
                            //    MessageBox.Show("Camera reconnected successfully.");
                        }
                    }
					// Process the exposures CID Data list collected from the Acquire 
					CID821_Data cid821Data = new CID821_Data();
					cid821Data = kronostestappMainForm.cidDataList[0];
					// Calculate the size needed to hold the signal data from the Subarray
					int length = (cid821Data.dr) * (cid821Data.dc);
					double[] ROI1 = new double[length];
					double[] ROI2 = new double[length];
					double[] DIFF = new double[length];
					double[] DIFF_SQRD = new double[length];
					r2forX = new double[exposureCount];
					double[] R2forY = new double[exposureCount];
					double[] bestFitY = new double[exposureCount];
					bestFit= new double[exposureCount];
					exposureNDRO = new double[exposureCount];
					noise = new double[exposureCount];
					noiseRatio = new double[exposureCount];
					theoryValue = new double[exposureCount];
					int exposureNDRONumber = 1;
					for (int exposureIndex = 0; exposureIndex < exposureCount; exposureIndex++)
					{
						for (int frameIndex = 0; frameIndex < kronostestappMainForm.cidDataList.Count; frameIndex++)
						{
							if ((kronostestappMainForm.cidDataList[frameIndex].exposureNumber == exposureIndex) &&
							   (kronostestappMainForm.cidDataList[frameIndex].subarrayName == "0"))
							{
								ROI1 = TestAppHelper.convertVideoDataToDouble(kronostestappMainForm.cidDataList[frameIndex].videoDataList);
							}
							if ((kronostestappMainForm.cidDataList[frameIndex].exposureNumber == exposureIndex) &&
								(kronostestappMainForm.cidDataList[frameIndex].subarrayName == "1"))
							{
								ROI2 = TestAppHelper.convertVideoDataToDouble(kronostestappMainForm.cidDataList[frameIndex].videoDataList);
							}
							if ((ROI1[0] > 0) && (ROI2[0] > 0))
							{
								double sumOfSquares = 0.0;
								try
								{
									for (int i = 0; i < length - 1; i++)
									{
										sumOfSquares += Math.Pow((ROI1[i] - ROI2[i]), 2.0);
									}
								}
								catch (IndexOutOfRangeException ex)
								{
									log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
								}
								double MSE = (sumOfSquares / length);
								noise[exposureIndex] = Math.Sqrt((MSE / 2));
								if (firstPoint)
								{
									signalReadNoise = noise[exposureIndex];
									firstPoint = false;
								}
								else
								{
									nDROReadNoise = noise[exposureIndex];
								}
								noiseRatio[exposureIndex] = Math.Log((signalReadNoise / nDROReadNoise));
								theoryValue[exposureIndex] = SignalReadNoise / Math.Sqrt(exposureNDRONumber);
								exposureNDRO[exposureIndex] = exposureNDRONumber;
								r2forX[exposureIndex] = exposureNDRONumber;
								R2forY[exposureIndex] = Math.Log(noise[exposureIndex], 10);
								bestFitY[exposureIndex] = noise[exposureIndex];
								bestFit[exposureIndex] = theoryValue[exposureIndex];
								exposureNDRONumber = exposureNDRONumber * 2;
								Array.Clear(ROI1, 0, ROI1.Length);
								Array.Clear(ROI2, 0, ROI2.Length);
								break;
							}
						}
					}
					// Calculate the Correlation Coefficient or R2
					double xy = 0;
					double x2 = 0;
					double y2 = 0;
					double sumX = 0;
					double sumY = 0;
					double sumXY = 0;
					double sumX2 = 0;
					double sumY2 = 0;
                    r2Correlation = 0;
                    for (int i = 0; i < exposureCount; i++)
					{
						xy = bestFit[i] * R2forY[i];
						x2 = bestFit[i] * bestFit[i];
						y2 = R2forY[i] * R2forY[i];
						sumX = sumX + bestFit[i];
						sumY = sumY + R2forY[i];
						sumXY = sumXY + xy;
						sumX2 = sumX2 + x2;
						sumY2 = sumY2 + y2;
					}
					double dem = Math.Sqrt(((exposureCount * sumX2) - (sumX * sumX)) *
											 (exposureCount * sumY2) - (sumY * sumY));
					double r = ((exposureCount * sumXY) - (sumX * sumY)) / dem;
                    if (double.IsNaN(sumX))
                        sumX = 0;
                    if (double.IsNaN(sumXY))
                        sumXY = 0;
                    if (double.IsNaN(r))
                        r = 0;
                    r2Correlation = r * r;
                    if (double.IsNaN(r2Correlation))
                        r2Correlation = 0;
                    if ((signalReadNoise >= noiseVsNDROSLimitsData.SnglNoiseLowerLimit && signalReadNoise <= noiseVsNDROSLimitsData.SnglNoiseUpperLimit)
                    && (r2Correlation >= noiseVsNDROSLimitsData.RPower2Correlation))

                    {
						noiseVsNDROsTestPassed = true;
					}
					else
					{
						noiseVsNDROsTestPassed = false;
						if (signalReadNoise < noiseVsNDROSLimitsData.SnglNoiseLowerLimit && signalReadNoise >= noiseVsNDROSLimitsData.SnglNoiseUpperLimit)
                        {
							noisVsNDROsTestFailureReason = "Read Noise @ 1 NDRO";
						}
						if(r2Correlation < noiseVsNDROSLimitsData.RPower2Correlation)
                        {
							noisVsNDROsTestFailureReason = "Read Noise Best Fit R^2 Correlation";
						}
						if ((signalReadNoise < noiseVsNDROSLimitsData.SnglNoiseLowerLimit && signalReadNoise >= noiseVsNDROSLimitsData.SnglNoiseUpperLimit)
								&& (r2Correlation <= noiseVsNDROSLimitsData.RPower2Correlation))
                        {
							noisVsNDROsTestFailureReason = "Read Noise @ 1 NDRO & Read Noise Best Fit R^2 Correlation";
						}
					}
				});
				log.Info("End Calcluate NoiseNDRO for Exposure Count " + exposureCount.ToString());
				kronostestappMainForm.cidDataList.Clear();
				kronostestappMainForm.cidIntegratedDataList.Clear();
			}
			catch (Exception ex)
			{
				noiseVsNDROsTestPassed = false;
				kronostestappMainForm.cidDataList.Clear();
				kronostestappMainForm.cidIntegratedDataList.Clear();
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
		public async Task WriteNoiseNDROTestResultToDB(CancellationToken ct, string TestResultImageFilePath)
		{
			try
			{
				kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
				//write to DB
				if (ct.IsCancellationRequested)
					return;
				noiseVsNDROTestResults = new NoiseVsNDROTestResults();
				noiseVsNDROTestResults.ExposureNDRO = String.Join(",", exposureNDRO.Select(p => p.ToString()).ToArray());
				noiseVsNDROTestResults.NoiseRatio = String.Join(",", noiseRatio.Select(p => p.ToString()).ToArray());
				noiseVsNDROTestResults.Noise = String.Join(",", noise.Select(p => p.ToString()).ToArray());
                noiseVsNDROTestResults.TheoryValue = String.Join(",", theoryValue.Select(p => p.ToString()).ToArray());
                noiseVsNDROTestResults.SignalReadNoise = signalReadNoise;
                noiseVsNDROTestResults.NDRONoise = nDROReadNoise;
                noiseVsNDROTestResults.TestPassed = true;
				noiseVsNDROTestResults.TestResultImagePath = TestResultImageFilePath;
				noiseVsNDROTestResultsDataList.Add(noiseVsNDROTestResults);
				if (TestAppHelper.CheckDBExists())
				{
					await SaveNoiseVsNDROsTestResultsToDB();
				}
				log.Info("End NoiseVsNDRO Test Results Saving to DB");
				await ProcessNoiseVsNDROsResultsXMLFile();
				//write test results to XML
				await WriteTestResultsToXML();
				//ExcelUtilities.WriteTestResultsToExcel("NoiseVsNDRO Test", noiseVsNDROTestResults);
			}
			catch (Exception ex)
			{
				testResultsSaved = false;
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
		public async Task SaveNoiseVsNDROsTestResultsToDB()
		{
			try
			{
				await Task.Run(() =>
				{
					using (var kcc = new KronosCamContext())
					{
						log.Info("Begin NoiseVsNDRO Test Results Saving to DB");
						kcc.noiseVsNDROTestResults.Add(noiseVsNDROTestResults);
						int savedTestResults = kcc.SaveChanges();
						if (savedTestResults > 0)
						{
							testResultsSaved = true;
							log.Info("NoiseVsNDRO Test Results Saved to DB");
						}
						else
						{
							testResultsSaved = false;
							log.Error("Error occured while saving NoiseVsNDRO Test Results to DB");
						}
					}
				});
			}
			catch (Exception ex)
			{
				testResultsSaved = false;
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}		
		public async Task ProcessNoiseNDROXMLFile()
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
		public async Task GetLimits()
		{
			try
			{
				if (TestAppHelper.CheckDBExists())
				{
					DBExists = true;
					await Task.Run(() =>
					{
						//comment below line while unit testing
						kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
						using (var kcc = new KronosCamContext())
						{
								noiseVsNDROsLimitsDataList = (from limits in kcc.noiseVsNDROSLimitsData orderby limits.DefinedDate descending select limits).ToList();
						}
					});
				}
				else
				{
						DBExists = false;					
				}
			}
			catch (Exception ex)
			{
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
	}
}
