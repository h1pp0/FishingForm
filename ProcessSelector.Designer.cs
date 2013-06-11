namespace Fishing
{
    partial class ProcessSelector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbProcessList = new System.Windows.Forms.ListBox();
            this.btnProcessAttach = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbProcessList
            // 
            this.lbProcessList.Location = new System.Drawing.Point(0, 0);
            this.lbProcessList.Name = "lbProcessList";
            this.lbProcessList.Size = new System.Drawing.Size(172, 108);
            this.lbProcessList.TabIndex = 0;
            this.lbProcessList.UseTabStops = false;
            // 
            // btnProcessAttach
            // 
            this.btnProcessAttach.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcessAttach.Location = new System.Drawing.Point(0, 109);
            this.btnProcessAttach.Name = "btnProcessAttach";
            this.btnProcessAttach.Size = new System.Drawing.Size(121, 33);
            this.btnProcessAttach.TabIndex = 1;
            this.btnProcessAttach.Text = "ATTACH";
            this.btnProcessAttach.UseVisualStyleBackColor = true;
            this.btnProcessAttach.Click += new System.EventHandler(this.btnProcessAttach_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(120, 109);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(52, 33);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "EXIT";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // ProcessSelector
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(172, 143);
            this.ControlBox = false;
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnProcessAttach);
            this.Controls.Add(this.lbProcessList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ProcessSelector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FishingForm - Select Process";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbProcessList;
        private System.Windows.Forms.Button btnProcessAttach;
        private System.Windows.Forms.Button btnExit;
    }
}