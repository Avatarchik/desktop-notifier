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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesktopNotifier));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.checkBoxEnableNotifications = new System.Windows.Forms.CheckBox();
            this.trackBarDisplayInterval = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDisplayInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Desktop Notifier";
            this.notifyIcon.Visible = true;
            // 
            // checkBoxEnableNotifications
            // 
            this.checkBoxEnableNotifications.AutoSize = true;
            this.checkBoxEnableNotifications.Location = new System.Drawing.Point(104, 12);
            this.checkBoxEnableNotifications.Name = "checkBoxEnableNotifications";
            this.checkBoxEnableNotifications.Size = new System.Drawing.Size(155, 21);
            this.checkBoxEnableNotifications.TabIndex = 0;
            this.checkBoxEnableNotifications.Text = "Enable Notifications";
            this.checkBoxEnableNotifications.UseVisualStyleBackColor = true;
            this.checkBoxEnableNotifications.CheckedChanged += new System.EventHandler(this.checkBoxEnableNotifications_CheckedChanged);
            // 
            // trackBarDisplayInterval
            // 
            this.trackBarDisplayInterval.Location = new System.Drawing.Point(15, 83);
            this.trackBarDisplayInterval.Maximum = 60;
            this.trackBarDisplayInterval.Minimum = 3;
            this.trackBarDisplayInterval.Name = "trackBarDisplayInterval";
            this.trackBarDisplayInterval.Size = new System.Drawing.Size(341, 56);
            this.trackBarDisplayInterval.TabIndex = 1;
            this.trackBarDisplayInterval.Value = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Notification Display Interval";
            // 
            // DesktopNotifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 253);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarDisplayInterval);
            this.Controls.Add(this.checkBoxEnableNotifications);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DesktopNotifier";
            this.Text = "Desktop Notifier";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DesktopNotifier_FormClosed);
            this.Load += new System.EventHandler(this.DesktopNotifier_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDisplayInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.CheckBox checkBoxEnableNotifications;
        private System.Windows.Forms.TrackBar trackBarDisplayInterval;
        private System.Windows.Forms.Label label1;
    }
}

