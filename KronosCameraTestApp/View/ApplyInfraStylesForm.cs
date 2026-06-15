using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.AppStyling;
using System.Reflection;
namespace KronosCameraTestApp.View
{
    public partial class ApplyInfraStylesForm : Form
    {
        private static string _StylePath = string.Empty;
        string assemblyPath = string.Empty;
        bool stylePath = false;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //This is static so that we can set this property before we even load the user control instance. e.g., we set this property from the Main( ) method.
        public static string StylePath
        {
            get { return _StylePath; }
            set { _StylePath = value; }
        }
        public ApplyInfraStylesForm()
        {
            InitializeComponent();
        }
        private void ultraButton1_Click(object sender, EventArgs e)
        {
            try
            {
                assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (this.cboStyles.SelectedIndex != -1)
                {
                    string theStyleFile = this.cboStyles.SelectedItem.DataValue.ToString();
                    stylePath = File.Exists("InfraStyles/" + cboStyles.SelectedItem.ToString() + ".isl");
                    Infragistics.Win.AppStyling.StyleManager.Load(stylePath ? theStyleFile : assemblyPath + "//" + cboStyles.SelectedItem.ToString() + ".isl");
                }
            }
          catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ApplyInfraStylesForm_Load(object sender, EventArgs e)
        {
            try
            {
            this.cboStyles.Items.Clear();   
            string[] theFiles = Directory.GetFiles(@"../../Infrastyles", "*.isl");
            if (theFiles.Length > 0)
            {
                foreach (string theFile in theFiles)
                {
                    this.cboStyles.Items.Add(theFile, Path.GetFileNameWithoutExtension(theFile));
                }                
            }  
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
