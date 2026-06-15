using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Properties;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.View
{
    public partial class BurnInAnalysis : Form
    {
        CIDInterface cidInterface;
        string burnInDataFilesServerPath = string.Empty;      
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DirectorySecurity securityRules { get; set; }
        List<string> diffAppFWFileContents;
        public List<string> DiffAppFWFileContents
        {
            get { return diffAppFWFileContents; }
            set { diffAppFWFileContents = value; }
        }
        List<string> diffDmesgFileContents;
        public List<string> DiffDmesgFileContents
        {
            get { return diffDmesgFileContents; }
            set { diffDmesgFileContents = value; }
        }
        string cameraSerialNumber = string.Empty;
        public string CameraSerialNumber
        {
            get { return cameraSerialNumber; }
            set { cameraSerialNumber = value; }
        }

        string imagerSerialNumber = string.Empty;
        public string ImagerSerialNumber
        {
            get { return imagerSerialNumber; }
            set { imagerSerialNumber = value; }
        }
        bool burnInAnalysisDone = false;
        public bool BurnInAnalysisDone
        {
            get { return burnInAnalysisDone; }
            set { burnInAnalysisDone = value; }
        }
        bool burnInAnalysisNoErrorsWarnings = false;
        public bool BurnInAnalysisNoErrorsWarnings
        {
            get { return burnInAnalysisNoErrorsWarnings; }
            set { burnInAnalysisNoErrorsWarnings = value; }
        }
        string burnInAnalysisResult = "NA";
        public string BurnInAnalysisResult
        {
            get { return burnInAnalysisResult; }
            set { burnInAnalysisResult = value; }
        }
        string burnInAnalysisResultsPath = string.Empty;
        public string BurnInAnalysisResultsPath
        {
            get { return burnInAnalysisResultsPath; }
            set { burnInAnalysisResultsPath = value; }
        }
        List<string> mfgLogFileContents;
        List<string> filteredmfgLogFileContents;
        List<BurnInDMesgIgnoreModel> burnInDMesgIgnoreModelList { get; set; }
        public BurnInAnalysis(CIDInterface cidInterface)
        {
            InitializeComponent();
            this.cidInterface = cidInterface;
        }
        private  void BurnInAnalysis_Load(object sender, EventArgs e)
        {
            burnInResultOptionSet.CheckedIndex = -1;            
            if(startBurnInFileAnalysis())
            {
                highlightErrors();
            }
            else
            {
                this.Close();
            }
             
        }
        public bool lookupBurnInScriptFile()
        {
            try
            {
                //var res = await Task.Run(() =>
                //{

                //});
                //return res;

                using (var sftpClient = new SftpClient("192.168.1.2", 22, "root", "cidtec"))
                {
                    sftpClient.Connect();
                    log.Info("SFTPClient connected for looking burnin files.");
                    sftpClient.ChangeDirectory("/");
                    var files = sftpClient.ListDirectory("root");
                    StringBuilder sb = new StringBuilder();
                    foreach (var file in files)
                    {
                        sb.AppendLine(file.Name);
                    }
                    //log.Error(sb.ToString());
                    if (sftpClient.Exists("/root/burnin.sh"))
                    {
                        //log.Error("lookupBurnInScriptFile1");
                        log.Info("BurnIn.sh file found");
                        sftpClient.Disconnect();
                        // log.Error("lookupBurnInScriptFile2");
                        log.Warn("SFTPClient disconnected");
                        return true;
                    }
                    else
                    {
                        log.Info("BurnIn.sh file NOT found");
                        sftpClient.Disconnect();
                        log.Warn("SFTPClient disconnected");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        public bool startBurnInFileAnalysis()
        {
            try
            {              
                Cursor.Current = Cursors.WaitCursor;
                ParametersModel parametersModel = TestAppHelper.GetParametersModel();
                Settings.Default.AutoTestResultsServerFilePathFromDB = parametersModel.TestResultsServerPath.ToString();
                if (cidInterface.IsConnected() )
                    {
                    if (string.IsNullOrEmpty(cameraSerialNumber))
                    {
                        cameraSerialNumber = "UNKNOWN";
                    }
                    using (var sftpClient = new SftpClient("192.168.1.2", 22, "root", "cidtec"))
                        {
                            sftpClient.Connect();
                            sftpClient.ChangeDirectory("/");
                            //var files = sftpClient.ListDirectory("root").Select(s => s.FullName);
                            var files = sftpClient.ListDirectory("root");
                       
                        if (!Directory.Exists(Settings.Default.AutoTestResultsServerFilePathFromDB+ "BurnInData\\"))
                        {
                            Directory.CreateDirectory(Settings.Default.AutoTestResultsServerFilePathFromDB + "BurnInData\\");
                        }
                            burnInDataFilesServerPath = Path.Combine(Settings.Default.AutoTestResultsServerFilePathFromDB, "BurnInData\\" +
                                                               cameraSerialNumber+"_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm"));
                        if (!Directory.Exists(burnInDataFilesServerPath))
                        {
                                Directory.CreateDirectory(burnInDataFilesServerPath);
                            }
                            if (!string.IsNullOrEmpty(cameraSerialNumber))
                            {
                                foreach (var file in files)
                                {
                                    string remoteFileName = file.Name;
                                    if (file.Name.StartsWith("appfw") || file.Name.StartsWith("dmesg") || file.Name.Equals("Readme.txt")||
                                    file.Name.Equals("mfglog.txt") || file.Name.Equals("burnin.sh") || file.Name.Equals("launcher.txt"))
                                    {
                                        if (!File.Exists(burnInDataFilesServerPath + "\\" + remoteFileName))
                                        {
                                            using (Stream file1 = File.OpenWrite(burnInDataFilesServerPath + "\\" + remoteFileName))
                                            {
                                                sftpClient.DownloadFile(file.FullName, file1);
                                                //sftpClient.DeleteFile(file.FullName);                                                
                                            }
                                        }
                                    }
                                }
                            }
                            sftpClient.Disconnect();
                            formatLogFiles();
                            if (errorWarningBurnInMfgLogFile())
                            {                               
                                burnInMfgLogFileAnalysis(false);
                            }
                            else
                            {
                                burnInMfgLogFileAnalysis(true);
                            }
                        }
                    }
                    else
                    {
                        log.Error("Firmware not running or camera not connected to host");
                        MessageBox.Show(
                            "Firmware not running or camera not connected to host",
                            "Firmware not found",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information);
                        burnInAnalysisDone = false;
                    return false;
                }
                    Cursor.Current = Cursors.Arrow;
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                MessageBox.Show("Unknown error while analyzing Burn-In data");
                Cursor.Current = Cursors.Arrow;
                burnInAnalysisDone = false;
                return false;
            }
        }
        private void formatLogFiles()
        {
            try
            {
                DirectoryInfo dinfo = new DirectoryInfo(burnInDataFilesServerPath + "\\");
                FileInfo[] burnInAppfwDataFiles = dinfo.GetFiles("appfw*.txt");
                foreach (var file in burnInAppfwDataFiles)
                {
                    string[] filecontents = File.ReadAllLines(file.FullName);
                    File.WriteAllText(burnInDataFilesServerPath + "\\" + file.Name, String.Join(Environment.NewLine, filecontents.ToArray()));
                }
                FileInfo[] burnInDmesgDataFiles = dinfo.GetFiles("dmesg*.txt");
                foreach (var file in burnInDmesgDataFiles)
                {
                    string[] filecontents = File.ReadAllLines(file.FullName);
                    File.WriteAllText(burnInDataFilesServerPath + "\\" + file.Name, String.Join(Environment.NewLine, filecontents.ToArray()));
                }
                FileInfo[] burnInAppMfgDataFiles = dinfo.GetFiles("mfglog.txt");
                if(burnInAppMfgDataFiles.Count() > 0)
                {
                    if (TestAppHelper.burnInDMesgIgnoreModelList == null || TestAppHelper.burnInDMesgIgnoreModelList.Count == 0)
                        getDmesgIgnoreFromDB();
                    else
                        burnInDMesgIgnoreModelList = TestAppHelper.burnInDMesgIgnoreModelList;
                    filteredmfgLogFileContents = new List<string>();
                    mfgLogFileContents = File.ReadAllLines(burnInAppMfgDataFiles[0].FullName).ToList();
                    filteredmfgLogFileContents = File.ReadAllLines(burnInAppMfgDataFiles[0].FullName).ToList(); 
                    foreach (string str in mfgLogFileContents)
                    {
                        foreach (BurnInDMesgIgnoreModel burnInDMesgIgnore in burnInDMesgIgnoreModelList)
                        {
                            if (str.Equals(burnInDMesgIgnore.DMesgIgnoreText) || 
                                str.Contains("imx-sdma 20ec000.sdma: Direct firmware load for imx/sdma/sdma-imx6q.bin failed with error -2"))
                            {
                                filteredmfgLogFileContents.Remove(str);
                            }
                               
                        }                        
                    }                   
                    File.WriteAllText(burnInDataFilesServerPath + "\\" + burnInAppMfgDataFiles[0].Name, String.Join(Environment.NewLine, filteredmfgLogFileContents.ToArray()));
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                MessageBox.Show("Unknown error while processing burn-in files");
                this.Close();
            }
        }
        private bool errorWarningBurnInMfgLogFile()
        {
            try
            {
                if (filteredmfgLogFileContents!=null &&(filteredmfgLogFileContents.Any(line => line.ToUpper().Contains("ERROR"))                   
                    || filteredmfgLogFileContents.Any(line => line.ToUpper().Contains("FAIL"))))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
                MessageBox.Show("Unknown error while processing burn-in files");
                this.Close();
            }
        }
        private void burnInMfgLogFileAnalysis(bool burnInPassed)
        {
            try
            {
                burnInAnalysisData.Clear();
                if (!burnInPassed)
                {
                    string[] mfgLogFileErros = filteredmfgLogFileContents.ToArray();
                    foreach(string errString in mfgLogFileErros)
                    {
                        if(errString.ToUpper().Contains("ERROR") || errString.ToUpper().Contains("FAIL"))
                        {
                            burnInAnalysisData.AppendText(errString);
                            burnInAnalysisData.AppendText(Environment.NewLine);
                            burnInAnalysisData.AppendText(Environment.NewLine);
                        }
                    }                  
                }                   
                else
                    burnInAnalysisData.Text = "BurnIn analysis found no issues";
                if (!Directory.Exists(burnInDataFilesServerPath + "\\BurnInTestResults"))
                {
                    Directory.CreateDirectory(burnInDataFilesServerPath + "\\BurnInTestResults");
                }
                if (!File.Exists(burnInDataFilesServerPath + "\\BurnInTestResults\\BurnInAnalysis.txt"))
                {                    
                    File.WriteAllText(burnInDataFilesServerPath + "\\BurnInTestResults\\BurnInAnalysis.txt", burnInAnalysisData.Text);
                }
                string linkPath = burnInDataFilesServerPath;
                reportLocation.Text = "BurnIn Files Location: " + burnInDataFilesServerPath;
                reportLocation.LinkArea = new LinkArea(23, linkPath.Length);
                burnInAnalysisResultsPath = burnInDataFilesServerPath + "\\BurnInTestResults\\";               
                burnInAnalysisDone = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                burnInAnalysisDone = false;
            }
        }       
        private void getDmesgIgnoreFromDB()
        {
            try
            {
                burnInDMesgIgnoreModelList = new List<BurnInDMesgIgnoreModel>();                
                using (var kcc = new KronosCamContext())
                {
                    burnInDMesgIgnoreModelList = (from dMesgIgnore in kcc.burnInDMesgIgnoreModel select dMesgIgnore).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void analyzeBurnInAppFWLogFiles()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DirectoryInfo dinfo = new DirectoryInfo(burnInDataFilesServerPath + "\\");
                FileInfo[] burnInAppfwDataFiles = dinfo.GetFiles("appfw*.txt");
                string[] firstAppFWFileContents = File.ReadAllLines(burnInAppfwDataFiles[0].FullName);
                diffAppFWFileContents = new List<string>();
                if(burnInAppfwDataFiles.Count() == 1)
                {
                    diffAppFWFileContents.Add("Found no errors/warnings/fails in AppFW log files.");
                }
                else
                {
                    foreach (var file in burnInAppfwDataFiles)
                    {
                        string[] filecontents = File.ReadAllLines(file.FullName);
                        diffAppFWFileContents.AddRange(firstAppFWFileContents.Except(filecontents));
                        diffAppFWFileContents.AddRange(filecontents.Except(firstAppFWFileContents));
                    }
                }
                diffAppFWFileContents.RemoveAll(remove=>remove.Equals("Info: getsockopt socket error register=0"));
                diffAppFWFileContents.RemoveAll(remove => remove.Equals("I: CommandCode::SetLogLevel 1 Error"));
                if (!Directory.Exists(burnInDataFilesServerPath + "\\BurnInTestResults"))
                {
                    Directory.CreateDirectory(burnInDataFilesServerPath + "\\BurnInTestResults");
                }
                if (!File.Exists(burnInDataFilesServerPath + "\\BurnInTestResults\\AppFWLogAnalysis.txt"))
                burnInAnalysisResultsPath = burnInDataFilesServerPath + "\\BurnInTestResults\\";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Cursor.Current = Cursors.Arrow;
            }
        }
        private void analyzeBurnInDmesgLogFiles()
        {
            try
            {
                DirectoryInfo dinfo = new DirectoryInfo(burnInDataFilesServerPath + "\\");
                FileInfo[] burnInDmesgDataFiles = dinfo.GetFiles("dmesg*.txt");
                string[] firstDmesgFileContents = File.ReadAllLines(burnInDmesgDataFiles[0].FullName);
                diffDmesgFileContents = new List<string>();
                List<string> dmesg_ignoreFileContents = new List<string>();
                dmesg_ignoreFileContents.Add("Direct firmware load for imx/sdma/sdma-imx6q.bin failed with error -2");
                dmesg_ignoreFileContents.Add("random: nonblocking pool is initialized");
                dmesg_ignoreFileContents.Add(" fec 2188000.ethernet eth0: Link is Up - 1Gbps/Full - flow control rx/tx");
                dmesg_ignoreFileContents.Add(" fec 2188000.ethernet eth0: Link is Down");
                dmesg_ignoreFileContents.Add("max/mean erase counter:");
                if (burnInDmesgDataFiles.Count() == 1)
                {
                    diffDmesgFileContents.Add("Found no errors/warnings/fails in dMesg log files.");
                }
                else
                {
                    foreach (var file in burnInDmesgDataFiles)
                    {
                            string[] filecontents = File.ReadAllLines(file.FullName);
                            diffDmesgFileContents.AddRange(firstDmesgFileContents.Except(filecontents));
                            diffDmesgFileContents.AddRange(filecontents.Except(firstDmesgFileContents));
                    }
                }               
                foreach(string str in dmesg_ignoreFileContents)
                {
                    diffDmesgFileContents.RemoveAll(remove => remove.Equals(str));
                }
                burnInAnalysisResultsPath = burnInDataFilesServerPath + "\\BurnInTestResults\\";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void deleteBurnInLogFiles()
        {
            try
            {
                using (var sftpClient = new SftpClient("192.168.1.2", 22, "root", "cidtec"))
                {
                    sftpClient.Connect();
                    sftpClient.ChangeDirectory("/");
                    var files = sftpClient.ListDirectory("root");
                    foreach (var file in files)
                    {
                        string remoteFileName = file.Name;
                        if (file.Name.StartsWith("appfw") || file.Name.StartsWith("dmesg") || file.Name.Equals("launcher.txt") ||
                            file.Name.Equals("mfglog.txt") || file.Name.Equals("burnin.sh") || file.Name.Equals("cycle.txt") || file.Name.Equals("Readme.txt"))
                        {
                            sftpClient.DeleteFile(file.FullName);
                        }
                    }
                    sftpClient.Disconnect();
                    TestAppHelper.BurnInFilesExistOnCamera = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void highlightErrors()
        {
            try
            {
                int index = 0;
                string temp = burnInAnalysisData.Text;              
                while (index <= burnInAnalysisData.Text.LastIndexOf("Error",StringComparison.CurrentCultureIgnoreCase))
                {
                    burnInAnalysisData.Find("Error", index, burnInAnalysisData.TextLength, RichTextBoxFinds.None);
                    burnInAnalysisData.SelectionBackColor = Color.Red;
                    burnInAnalysisData.SelectionFont = new Font(burnInAnalysisData.SelectionFont.FontFamily, burnInAnalysisData.SelectionFont.Size, FontStyle.Bold);
                    index = burnInAnalysisData.Text.IndexOf("Error", index, StringComparison.CurrentCultureIgnoreCase) + 1;
                }
                index = 0;
                while (index <= burnInAnalysisData.Text.LastIndexOf("Fail", StringComparison.CurrentCultureIgnoreCase))
                {
                    burnInAnalysisData.Find("Fail", index, burnInAnalysisData.TextLength, RichTextBoxFinds.None);
                    burnInAnalysisData.SelectionBackColor = Color.Orange;
                    burnInAnalysisData.SelectionFont = new Font(burnInAnalysisData.SelectionFont.FontFamily, burnInAnalysisData.SelectionFont.Size, FontStyle.Bold);
                    index = burnInAnalysisData.Text.IndexOf("Fail", index, StringComparison.CurrentCultureIgnoreCase) + 1;
                }
                burnInAnalysisData.ReadOnly = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void burninPass_Click(object sender, EventArgs e)
        {
            try
            {               
                TestAppHelper.kronosTestAppMainForm.ledBurnInAnalysisResult.Value = true;
                burnInAnalysisResult = "Pass";
                burnInAnalysisDone = true;
                deleteBurnInLogFiles();
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);               
            }
        }
        private void burninFail_Click(object sender, EventArgs e)
        {
            try
            {
                burnInAnalysisResult = "Fail";
                TestAppHelper.kronosTestAppMainForm.ledBurnInAnalysisResult.Value = false;
                TestAppHelper.kronosTestAppMainForm.ledBurnInAnalysisResult.OffColor = Color.Red;
                burnInAnalysisDone = true;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void saveBurnInResultsToDB()
        {
            try
            {
                BurnInAnalysisModel burnInAnalysisModel = new BurnInAnalysisModel();
                burnInAnalysisModel.CameraSN = cameraSerialNumber;
                burnInAnalysisModel.ImagerSN = imagerSerialNumber;
                burnInAnalysisModel.BurnInResult = TestAppHelper.kronosTestAppMainForm.ledBurnInAnalysisResult.Value;
                burnInAnalysisModel.DateExecuted = DateTime.Now;
                burnInAnalysisModel.UserExecuted = Environment.UserName.ToUpper();
                burnInAnalysisModel.BurnInResultsPath = burnInAnalysisResultsPath;
                if (filteredmfgLogFileContents != null)
                    burnInAnalysisModel.BurnInLogFileData = String.Join(Environment.NewLine, filteredmfgLogFileContents.ToArray());
                else
                    burnInAnalysisModel.BurnInLogFileData = string.Empty;
                using (KronosCamContext kcc = new KronosCamContext())
                {
                    kcc.burnInAnalysisModel.Add(burnInAnalysisModel);
                    int burnInAnalysisSaved = kcc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void BurnInAnalysis_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                burnInAnalysisData.Clear();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private string CreatedFormattedText(string text, IDictionary<string, Color> table)
        {
             StringBuilder sb = new StringBuilder();
            //  First build a regex pattern that will capture each of the words
            //  we want to match on.
            int last = table.Keys.Count - 1;
            int index = 0;
            foreach (string word in table.Keys)
            {
                sb.Append(word);
                if (index < last)
                    sb.Append("|");
                index += 1;
            }
            //  Regex the input string
            string pattern = sb.ToString();
            Match m = Regex.Match(text, pattern);
            //  Clear the StringBuilder and reuse it for the next part...
            sb.Clear();
            //  Iterate the matches, and insert a <span> that colorizes
            //  the text in place of the match.
            int leftOff = 0;
            while (m.Success)
            {
                //  Append the intervening characters, i.e., between the
                //  end of the last match and the beginning of this one.
                int length = m.Index - leftOff;
                if (length > 0)
                    sb.Append(text, leftOff, length);
                //  Update the position of the last match for the next iteration
                leftOff = m.Index + m.Length;
                Color color = Color.Empty;
                if (table.TryGetValue(m.Value, out color))
                {
                    sb.AppendFormat("<span style = \"background-color: {0};\">{1}</span>", FormatColor(color), m.Value);
                }
                m = m.NextMatch();
            }
            //  Append the remaining characters.
            int remaining = text.Length - leftOff;
            if (remaining > 0)
                sb.Append(text, leftOff, remaining);
            return sb.ToString();
        }
        static private string FormatColor(Color color)
        {
            string a = color.A.ToString("x2");
            string r = color.R.ToString("x2");
            string g = color.G.ToString("x2");
            string b = color.B.ToString("x2");
            return string.Format("#{0}{1}{2}{3}", a, r, g, b);
        }
        private void reportLocation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Explorer", "/select," + burnInDataFilesServerPath+"\\BurnInTestResults");
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void burnInResultOptionSet_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (burnInResultOptionSet.CheckedIndex == 0)
                {
                    burnInPassState();
                }
                if (burnInResultOptionSet.CheckedIndex == 1)
                {
                    burnInFailState();
                }
                if (burnInResultOptionSet.CheckedIndex == 0 || burnInResultOptionSet.CheckedIndex == 1 && burnInAnalysisDone)
                {                   
                    saveBurnInResultsToDB();
                }
                if(burnInResultOptionSet.CheckedIndex != -1)
                    this.Close();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void burnInPassState()
        {
            try
            {              
                TestAppHelper.kronosTestAppMainForm.ledBurnInAnalysisResult.Value = true;
                burnInAnalysisResult = "Pass";                
                deleteBurnInLogFiles();                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void burnInFailState()
        {
            try
            {               
                burnInAnalysisResult = "Fail";
                TestAppHelper.kronosTestAppMainForm.ledBurnInAnalysisResult.Value = false;
                TestAppHelper.kronosTestAppMainForm.ledBurnInAnalysisResult.OffColor = Color.Red;              
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
