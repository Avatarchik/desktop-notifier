namespace desktop_notifier
{
    partial class DesktopNotifier
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
            if (disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesktopNotifier));
            this.checkBoxEnableNotifications = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonTestNotification = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.dataGridViewBlacklist = new System.Windows.Forms.DataGridView();
            this.AppName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBlacklist)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxEnableNotifications
            // 
            this.checkBoxEnableNotifications.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxEnableNotifications.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxEnableNotifications.Location = new System.Drawing.Point(153, 3);
            this.checkBoxEnableNotifications.Name = "checkBoxEnableNotifications";
            this.checkBoxEnableNotifications.Size = new System.Drawing.Size(212, 28);
            this.checkBoxEnableNotifications.TabIndex = 0;
            this.checkBoxEnableNotifications.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxEnableNotifications.UseVisualStyleBackColor = true;
            this.checkBoxEnableNotifications.CheckedChanged += new System.EventHandler(this.checkBoxEnableNotifications_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.96516F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.03484F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxEnableNotifications, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonTestNotification, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonExit, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewBlacklist, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(368, 253);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 179);
            this.label2.TabIndex = 3;
            this.label2.Text = "Blacklist";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 34);
            this.label3.TabIndex = 6;
            this.label3.Text = "Notifications Enabled?";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonTestNotification
            // 
            this.buttonTestNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTestNotification.Location = new System.Drawing.Point(150, 213);
            this.buttonTestNotification.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTestNotification.Name = "buttonTestNotification";
            this.buttonTestNotification.Size = new System.Drawing.Size(218, 40);
            this.buttonTestNotification.TabIndex = 7;
            this.buttonTestNotification.Text = "Test Notification";
            this.buttonTestNotification.UseVisualStyleBackColor = true;
            this.buttonTestNotification.Click += new System.EventHandler(this.buttonTestNotification_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonExit.Location = new System.Drawing.Point(0, 213);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(150, 40);
            this.buttonExit.TabIndex = 8;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // dataGridViewBlacklist
            // 
            this.dataGridViewBlacklist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBlacklist.ColumnHeadersVisible = false;
            this.dataGridViewBlacklist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AppName});
            this.dataGridViewBlacklist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBlacklist.Location = new System.Drawing.Point(153, 37);
            this.dataGridViewBlacklist.Name = "dataGridViewBlacklist";
            this.dataGridViewBlacklist.RowHeadersVisible = false;
            this.dataGridViewBlacklist.RowTemplate.Height = 24;
            this.dataGridViewBlacklist.Size = new System.Drawing.Size(212, 173);
            this.dataGridViewBlacklist.TabIndex = 9;
            // 
            // AppName
            // 
            this.AppName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AppName.HeaderText = "AppName";
            this.AppName.Name = "AppName";
            // 
            // DesktopNotifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(368, 253);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DesktopNotifier";
            this.Text = "Desktop Notifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DesktopNotifier_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DesktopNotifier_FormClosed);
            this.Load += new System.EventHandler(this.DesktopNotifier_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBlacklist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxEnableNotifications;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonTestNotification;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.DataGridView dataGridViewBlacklist;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppName;
    }
}

