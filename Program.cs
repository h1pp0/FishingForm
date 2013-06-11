using System;
using System.Windows.Forms;

namespace Fishing {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) 
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
            {
                args = new string[] { "No_args" };
            }
            try
            {
            	Application.Run(new FishingForm(args));
            }
            catch(Exception e)
            {
                MessageBox.Show(e.StackTrace, "FishingForm Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
