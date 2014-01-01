using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Fishing
{
    internal static class LoggingClass
    {
        private static readonly object lockobject = new object();
        public static void WriteFishingStatsToFile(string data)
        {
            var filepath = string.Format("{0}\\FishLog_{1}.txt", Application.StartupPath,
                DateTime.Now.ToShortDateString().Replace('/', '-'));
            try
            {
                lock (lockobject)
                {
                    using (StreamWriter sb = new StreamWriter(filepath, true, Encoding.UTF8))
                    {
                        sb.WriteLine(data);
                        sb.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while attempting to write to log file!\n\n" + ex.Message, "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                throw;
            }
        }
    }
}