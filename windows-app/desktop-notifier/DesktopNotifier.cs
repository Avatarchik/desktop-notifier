/*
    Desktop Notifier
    Copyright (C) 2015 Gowtham (gowthamgowtham@gmail.com)
    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Specialized;

namespace desktop_notifier
{
    public partial class DesktopNotifier : Form
    {
        public delegate void MessageReceivedEvent(Message message);
        private BluetoothComm comm = new BluetoothComm();
        private Thread listeningThread;

        public DesktopNotifier()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void DesktopNotifier_Load(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            //System.Threading.ThreadPool.QueueUserWorkItem(delegate
            //{
            //    StartInternal();
            //}, null);

            if (listeningThread == null)
            {
                listeningThread = new Thread(StartInternal);
                listeningThread.SetApartmentState(ApartmentState.STA);
            }
            
            listeningThread.Start();
        }

        private void StartInternal()
        {
            comm.Callback = new MessageReceivedEvent(MessageReceived);
            comm.Start();
        }

        public void MessageReceived(Message message)
        {
            if (Properties.Settings.Default.NotificationsEnabled)
            {
                if (!IsNotificationBlacklisted(message))
                {
                    GetDesktopNotifierInterface().ShowNotification(message,
                                                                   Properties.Settings.Default.NotificationInterval);
                }
            }
        }

        private bool IsNotificationBlacklisted(Message message)
        {
            string appname = message.AppName;
            if (appname == null)
                return false;
            return Properties.Settings.Default.NotificationBlacklistApps.Contains(appname);
        }

        private DesktopNotifierInterface GetDesktopNotifierInterface()
        {
            //return new FancyDesktopNotifier(Icon);
            return new WindowsDefaultNotifier(notifyIcon);
        }

        private void DesktopNotifier_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Icon = null;
            SaveSettings();
            Stop();
        }

        private void checkBoxEnableNotifications_CheckedChanged(object sender, EventArgs e)
        {            
            if (checkBoxEnableNotifications.Checked)
            {
                Start();
            }
            else
            {
                Stop();
            }
            SaveSettings();
        }

        private void Stop()
        {
            SaveSettings();
            if (listeningThread == null) { return; }
            comm.Stop();
            listeningThread.Join();
            listeningThread = null;
        }

        private void notifyIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            notifyIcon.Icon = Icon;
        }

        private void DesktopNotifier_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Visible)
                Hide();
            else
                Show();
        }

        private void SetCheckBoxSilent(CheckBox checkbox, Boolean value)
        {
            checkbox.CheckedChanged -= checkBoxEnableNotifications_CheckedChanged;
            checkbox.Checked = value;
            checkbox.CheckedChanged += checkBoxEnableNotifications_CheckedChanged;
        }

        private void SetNumericUpDownSilent(NumericUpDown updown, int value)
        {
            numericUpDownNotificationInterval.ValueChanged -= numericUpDownNotificationInterval_ValueChanged;
            updown.Value = Convert.ToDecimal(value);
            numericUpDownNotificationInterval.ValueChanged += numericUpDownNotificationInterval_ValueChanged;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Icon = null;
            Stop();
            Environment.Exit(0);
        }

        private void buttonTestNotification_Click(object sender, EventArgs e)
        {
            ShowTestNotification();
        }

        private void ShowTestNotification()
        {
            Message message = new Message(@"{text:""Sample text for notification"", title:""Sample title""}");
            GetDesktopNotifierInterface().ShowNotification(message, Properties.Settings.Default.NotificationInterval);
        }

        private void LoadSettings()
        {
            SetCheckBoxSilent(checkBoxEnableNotifications, Properties.Settings.Default.NotificationsEnabled);
            SetNumericUpDownSilent(numericUpDownNotificationInterval, Properties.Settings.Default.NotificationInterval);

            dataGridViewBlacklist.Rows.Clear();
            StringCollection blacklist = Properties.Settings.Default.NotificationBlacklistApps;
            string[] array = new string[blacklist.Count];
            blacklist.CopyTo(array, 0);
            foreach(string name in array) {
                dataGridViewBlacklist.Rows.Add(name);
            }
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.NotificationsEnabled = checkBoxEnableNotifications.Checked;
            Properties.Settings.Default.NotificationInterval = Convert.ToInt32(numericUpDownNotificationInterval.Value);
            Properties.Settings.Default.NotificationBlacklistApps.Clear();
            List<string> names = new List<string>();
            foreach (DataGridViewRow row in dataGridViewBlacklist.Rows)
            {
                if(row.Cells[0].Value != null)
                    names.Add(row.Cells[0].Value.ToString());
            }
            Properties.Settings.Default.NotificationBlacklistApps.AddRange(names.ToArray());
            Properties.Settings.Default.Save();
        }

        private void listViewBlacklist_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            SaveSettings();
        }

        private void numericUpDownNotificationInterval_ValueChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
