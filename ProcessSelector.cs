using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Fishing
{
    internal partial class ProcessSelector : Form
    {
        #region Classes

        /// <summary>
        /// Class container holding the PID and name of a POL process 
        /// </summary>
        public class POLProcess
        {
            private int thisPOLID;
            private string thisPOLName;

            public POLProcess(int intPOLID, string strPOLName)
            {
                this.thisPOLID = intPOLID;
                this.thisPOLName = strPOLName + " - " + intPOLID.ToString();
            }

            public int POLID
            {
                get
                {
                    return thisPOLID;
                }
            }

            public string POLName
            {
                get
                {
                    return thisPOLName;
                }
            }

        } // @ internal class POLProcess

        #endregion

        #region Members

        /// <summary>
        /// 'Public' property, holding either the required or chosen POL process
        /// </summary>
        internal POLProcess ThisProcess { get; set; }

        /// <summary>
        /// Array holding all of the currently running "pol.exe" processes
        /// </summary>
        private static Process[] p = Process.GetProcessesByName("pol");
        //private static Process[] p = Process.GetProcessesByName("ffxi-boot");

        /// <summary>
        /// POL names that are not to be included in process selection
        /// </summary>
        /// <todo>
        /// Add any 'names' that might be needed, e.g., from FR/GR/JP POL
        /// </todo>
        private static Regex ignoreThese = new Regex("(PlayOnline)|(Final Fantasy)");

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal ProcessSelector()
        {
            InitializeComponent();

            if(0 == p.Length)  //no 'pol' process found, error out
            {
                MessageBox.Show("Please fully log into your FFXI character before starting!", "Error");
                System.Environment.Exit(0);
            }
            else if(1 == p.Length)  //automatically choose 'pol' process if only one running
            {
                if(ignoreThese.IsMatch(p[0].MainWindowTitle))  //if not fully logged in, error out
                {
                    MessageBox.Show("Please fully log into your FFXI character before starting!", "Error");
                    System.Environment.Exit(0);
                }
                else  //attach to the only running 'pol' process, if fully logged in
                {
                    ThisProcess = new POLProcess(p[0].Id, p[0].MainWindowTitle);
                }
            }
            else  //more than one 'pol' process found, fill a ListBox with them to choose from
            {
                int index = GetProcesses();

                if(-1 == index)  //none of the 'pol' processes were actually logged in
                {
                    MessageBox.Show("Please fully log into your FFXI character before starting!", "Error");
                    System.Environment.Exit(0);
                }
                
                if(-1 < index)  //(index >= 0) means only one of the characters of all the 'pol' processes is logged in
                {
                    ThisProcess = new POLProcess(p[index].Id, p[index].MainWindowTitle);
                }
            }

        } // @ internal ProcessSelector()

        #endregion

        #region Methods

        /// <summary>
        /// Populate ListBox with currently logged in characters 
        /// </summary>
        private int GetProcesses()
        {
            int processPlace = 0;
            List<POLProcess> POLProcesses = new List<POLProcess>();

            for(int i = 0; i < p.Length; i++)
            {
                //if "PlayOnline" or "Final Fantasy" are not in the WindowTitle
                // add the process to the ListBox and store the p[index]
                if(!ignoreThese.IsMatch(p[i].MainWindowTitle))
                {
                    processPlace = i;
                    POLProcesses.Add(new POLProcess(p[i].Id, p[i].MainWindowTitle));
                }
            }

            POLProcesses.TrimExcess();  //get rid of any null entries from List<> default capacity
            lbProcessList.DataSource = POLProcesses;
            lbProcessList.DisplayMember = "POLName";
            lbProcessList.ValueMember = "POLID";
            lbProcessList.ClearSelected();

            if(0 == lbProcessList.Items.Count)  //no characters fully logged in, return an indicator to that fact
            {
                return -1;
            }
            
            if(1 == lbProcessList.Items.Count)  //only one fully logged in character found
            {
                return processPlace;  //return the array index of that one logged in character
            }

            return -42;  //process ListBox has two or more good entries
                         // negative return, so it doesn't look like a possible array index 

        } // @ private int GetProcesses()

        #endregion

        #region Events

        private void btnProcessAttach_Click(object sender, System.EventArgs e)
        {
            if(-1 != lbProcessList.SelectedIndex)
            {
                ThisProcess = (POLProcess)lbProcessList.SelectedItem;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a process first!");
            }
        }

        #endregion

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            System.Environment.Exit(1);
        }
    }
}
