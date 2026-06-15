using KronosCameraTestApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace KronosCameraTestApp.View
{
    public partial class Logger : Form
    {
        private log4net.Core.Level logLevel = log4net.Core.Level.Warn;
        private LogLocation logLocation = LogLocation.None;
        private bool firmwareLoggingEnabled = false;
        private bool isManufacturingUserMode=false;
        public bool IsManufacturingUserMode
        {
            get { return isManufacturingUserMode; }
            set { isManufacturingUserMode = value; }
        }
        //The once-per-class call to initialize the log object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum LogLocation
        {
            /// <summary>
            /// The none
            /// </summary>
            None = 0,
            /// <summary>
            /// The console
            /// </summary>
            Console = 1,
            /// <summary>
            /// The file
            /// </summary>
            File = 2
        };
        public Logger()
        {
            InitializeComponent();
            SetRadioButtons();
            log.Info("Logger Initialized");
            log4net.Core.Level logLevel = log4net.Core.Level.Warn;
        }
        private void Logger_Load(object sender, EventArgs e)
        {
            this.noneRadioButton.Checked = Properties.Settings.Default.noneRadioButton;
            this.fileRadioButton.Checked = Properties.Settings.Default.fileRadioButton;
            this.consoleRadioButton.Checked = Properties.Settings.Default.consoleRadioButton;
            this.debugRadioButton.Checked = Properties.Settings.Default.debugRadioButton;
            this.infoRadioButton.Checked = Properties.Settings.Default.infoRadioButton;
            this.warningRadioButton.Checked = Properties.Settings.Default.warningRadioButton;
            this.errorRadioButton.Checked = Properties.Settings.Default.errorRadioButton;
            this.fatalRadioButton.Checked = Properties.Settings.Default.fatalRadioButton;
            this.enableFWLogCheckBox.Checked = Properties.Settings.Default.firmwareLoggingEnabled;
            if(!isManufacturingUserMode)
            {
                filterLevelGroupBox.Enabled = true;
            }
            else
            {
                filterLevelGroupBox.Enabled = false;
            }
        }
        private void SetRadioButtons()
        {
            switch (logLevel.Name)
            {
                case "Debug":
                    debugRadioButton.Checked = true;
                    break;
                case "Info":
                    infoRadioButton.Checked = true;
                    break;
                case "Warn":
                    warningRadioButton.Checked = true;
                    break;
                case "Error":
                    errorRadioButton.Checked = true;
                    break;
                case "Fatal":
                    fatalRadioButton.Checked = true;
                    break;
                default:
                    infoRadioButton.Checked = true;
                    break;
            }
            switch (logLocation)
            {
                case LogLocation.None:
                    noneRadioButton.Checked = true;
                    break;
                case LogLocation.File:
                    fileRadioButton.Checked = true;
                    break;
                case LogLocation.Console:
                    consoleRadioButton.Checked = true;
                    break;
                default:
                    noneRadioButton.Checked = true;
                    break;
            }
        }
        private void Logger_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default["noneRadioButton"] = this.noneRadioButton.Checked;
            Properties.Settings.Default["fileRadioButton"] = this.fileRadioButton.Checked;
            Properties.Settings.Default["consoleRadioButton"] = this.consoleRadioButton.Checked;
            Properties.Settings.Default["debugRadioButton"] = this.debugRadioButton.Checked;
            Properties.Settings.Default["infoRadioButton"] = this.infoRadioButton.Checked;
            Properties.Settings.Default["warningRadioButton"] = this.warningRadioButton.Checked;
            Properties.Settings.Default["errorRadioButton"] = this.errorRadioButton.Checked;
            Properties.Settings.Default["fatalRadioButton"] = this.fatalRadioButton.Checked;
            Properties.Settings.Default["firmwareLoggingEnabled"] = this.enableFWLogCheckBox.Checked;
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            // get the current radio button setting for level
            if (debugRadioButton.Checked)
                logLevel = log4net.Core.Level.Debug;
            else if (infoRadioButton.Checked)
                logLevel = log4net.Core.Level.Info;
            else if (warningRadioButton.Checked)
                logLevel = log4net.Core.Level.Warn;
            else if (errorRadioButton.Checked)
                logLevel = log4net.Core.Level.Error;
            else if (fatalRadioButton.Checked)
                logLevel = log4net.Core.Level.Fatal;
            else // set the default
                logLevel = log4net.Core.Level.Info;
            // get the current radio button setting for location
            if (noneRadioButton.Checked)
                logLocation = LogLocation.None;
            else if (consoleRadioButton.Checked)
                logLocation = LogLocation.Console;
            else if (fileRadioButton.Checked)
                logLocation = LogLocation.File;
            else // set the default
                logLocation = LogLocation.None;
            if (enableFWLogCheckBox.Checked)
                firmwareLoggingEnabled = true;
            else
                firmwareLoggingEnabled = false;

            if(logLevel == log4net.Core.Level.Warn || logLevel == log4net.Core.Level.Error || logLevel == log4net.Core.Level.Info)
            {
                TestAppHelper.IsFirmwareLogLevelWarning = false;
            }
            // close this dialog
            this.Close();
        }
        public log4net.Core.Level GetLoggerLevel()
        {
            return logLevel;
        }
        public bool fwLoggingEnabled()
        {
            return firmwareLoggingEnabled;
        }
        /// <summary>
        /// Gets the logger location.
        /// </summary>
        /// <returns></returns>
        public LogLocation GetLoggerLocation()
        {
            return logLocation;
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
