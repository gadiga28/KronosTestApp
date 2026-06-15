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
    public partial class TemperatureGraphForm : Form
    {
        double imageTemperature;
        public double ImageTemperature
        {
            get { return imageTemperature; }
            set 
            { 
                imageTemperature = value;
                if (!this.waveformPlot1.IsDisposed)
                {
                    if (imageTemperature !=null)
                        this.waveformPlot1.PlotYAppend(imageTemperature);
                }
            }            
        }
        double imageHumidity;
        public double ImageHumidity
        {
            get { return imageHumidity; }
            set
            {
                imageHumidity = value;
                if (!this.waveformPlot2.IsDisposed)
                {
                    if (imageHumidity !=null)
                        this.waveformPlot2.PlotYAppend(imageHumidity);
                }
            }
        }
        public TemperatureGraphForm()
        {
            InitializeComponent();
        }
        private void TemperatureGraphForm_Load(object sender, EventArgs e)
        {
        }
        private void TemperatureGraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
