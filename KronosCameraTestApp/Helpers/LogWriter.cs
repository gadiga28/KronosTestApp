using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Helpers
{
    public class LogWriter
    {
        private string m_exePath = string.Empty;
        string fileName = string.Empty;
        public LogWriter(string filePath)
        {
            m_exePath = filePath;
            fileName = "Temp_Hum_log_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm") + ".txt";
            //LogWrite(logMessage);
        }
        public void LogWrite(string camTemp, string camHum)
        {
            //m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);            
            try
            {
                using (StreamWriter txtWriter = File.AppendText(m_exePath + "\\" + fileName))
                {
                    txtWriter.WriteLine("{0} {1} {2} {3} {4}","Camera Temperature: ",camTemp, "Camera Humidity: ", camHum, DateTime.Now.ToLongTimeString());
                    txtWriter.WriteLine("-------------------------------");
                    //Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {                
                txtWriter.WriteLine("{0} {1} {2}", logMessage,"at", DateTime.Now.ToLongTimeString()); 
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
