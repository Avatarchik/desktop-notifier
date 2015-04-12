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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace desktop_notifier
{
    public partial class DesktopNotifier : Form
    {
        #region Win32 API call
        
        // This code is sourced from http://stackoverflow.com/questions/26153810/get-the-applications-notifyicon-rectangle

        public const Int32 WM_MYMESSAGE = 0x8000; //WM_APP
        public const Int32 NOTIFYICON_VERSION_4 = 0x4;

        public const Int32 NIIF_USER = 0x4;
        public const Int32 NIIF_NONE = 0x0;
        public const Int32 NIIF_INFO = 0x1;
        public const Int32 NIIF_WARNING = 0x2;
        public const Int32 NIIF_ERROR = 0x3;
        public const Int32 NIIF_LARGE_ICON = 0x20;

        public const Int32 NIN_BALLOONHIDE = 0x403;
        public const Int32 NIN_BALLOONSHOW = 0x402;
        public const Int32 NIN_BALLOONTIMEOUT = 0x404;
        public const Int32 NIN_BALLOONUSERCLICK = 0x405;
        public const Int32 NIN_KEYSELECT = 0x403;
        public const Int32 NIN_SELECT = 0x400;
        public const Int32 NIN_POPUPOPEN = 0x406;
        public const Int32 NIN_POPUPCLOSE = 0x408;

        public enum NotifyFlags
        {
            NIF_MESSAGE = 0x01, NIF_ICON = 0x02, NIF_TIP = 0x04, NIF_INFO = 0x10, NIF_STATE = 0x08,
            NIF_GUID = 0x20, NIF_SHOWTIP = 0x80, NIF_REALTIME = 0x40,
        }

        public enum NotifyCommand { NIM_ADD = 0x0, NIM_DELETE = 0x2, NIM_MODIFY = 0x1, NIM_SETVERSION = 0x4 }

        [StructLayout(LayoutKind.Sequential)]
        public struct NOTIFYICONDATA
        {
            public Int32 cbSize;
            public IntPtr hWnd;
            public Int32 uID;
            public NotifyFlags uFlags;
            public Int32 uCallbackMessage;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public String szTip;
            public Int32 dwState;
            public Int32 dwStateMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String szInfo;
            public Int32 uVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String szInfoTitle;
            public Int32 dwInfoFlags;
            public Guid guidItem; //> IE 6
            public IntPtr hBalloonIcon;
        }

        [DllImport("shell32.dll")]
        public static extern System.Int32 Shell_NotifyIcon(NotifyCommand cmd, ref NOTIFYICONDATA data);

        private void AddIcon()
        {
            NOTIFYICONDATA data = new NOTIFYICONDATA();

            data.cbSize = Marshal.SizeOf(data);
            data.hWnd = Handle;
            data.uID = 0x01;
            data.hIcon = Icon.Handle;
            data.szTip = "Desktop Notifier";

            data.uCallbackMessage = WM_MYMESSAGE; //This is the message sent to our app

            data.uFlags = NotifyFlags.NIF_ICON | NotifyFlags.NIF_MESSAGE | NotifyFlags.NIF_TIP;


            data.uVersion = NOTIFYICON_VERSION_4;

            Debug.WriteLine( Shell_NotifyIcon(NotifyCommand.NIM_ADD, ref data) );
            
            Debug.WriteLine(Shell_NotifyIcon(NotifyCommand.NIM_SETVERSION, ref data));
        }

        private void DeleteIcon()
        {
            NOTIFYICONDATA data = new NOTIFYICONDATA();
            data.cbSize = Marshal.SizeOf(data);
            data.uID = 0x01;
            data.hWnd = Handle;
            data.hIcon = Icon.Handle;

            Shell_NotifyIcon(NotifyCommand.NIM_DELETE, ref data);
        }

        private void AddBalloon(Message message, int timeout)
        {
            NOTIFYICONDATA data = new NOTIFYICONDATA();

            data.cbSize = Marshal.SizeOf(data);
            data.uID = 0x01;
            data.hWnd = Handle;
            data.dwInfoFlags = NIIF_USER;
            data.hIcon = Icon.Handle;
            data.hBalloonIcon = IntPtr.Zero;
            if (message.Image != null)
            {
                data.hBalloonIcon = ((Bitmap)message.Image).GetHicon();
                data.dwInfoFlags |= NIIF_LARGE_ICON;
            }
            data.szInfo = message.Text + " (" + message.Length + "KB)";
            data.szInfoTitle = message.Title;

            data.uFlags = NotifyFlags.NIF_INFO | NotifyFlags.NIF_SHOWTIP | NotifyFlags.NIF_REALTIME;;

            Console.WriteLine(Shell_NotifyIcon(NotifyCommand.NIM_MODIFY, ref data));
        }
        #endregion

        public delegate void MessageReceivedEvent(Message message);
        private BluetoothComm comm;
        private Thread listeningThread;

        public DesktopNotifier()
        {
            InitializeComponent();
            LoadSettings();
            AddIcon();

            InitializeBluetooth();
        }

        private void DesktopNotifier_Load(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            if (listeningThread == null)
            {
                listeningThread = new Thread(StartInternal);
                listeningThread.SetApartmentState(ApartmentState.STA);
            }
            
            listeningThread.Start();
        }

        private void InitializeBluetooth()
        {
            Console.WriteLine("Initializing bluetooth");
            while(comm == null)
            {
                Console.WriteLine("Waiting for bluetooth...");
                try
                {
                    comm = new BluetoothComm();
                }
                catch (Exception)
                {
                    Console.Write(".");
                    Thread.Sleep(1000);
                }
            }
            Console.WriteLine("Done");
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
                    ShowNotification(message, Properties.Settings.Default.NotificationInterval);
                }
            }
        }

        private void ShowNotification(Message message, int timeout)
        {
            AddBalloon(message, timeout);
        }

        private bool IsNotificationBlacklisted(Message message)
        {
            string appname = message.AppName;
            if (appname == null)
                return false;
            return Properties.Settings.Default.NotificationBlacklistApps.Contains(appname);
        }

        private void DesktopNotifier_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeleteIcon();
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

        private void DesktopNotifier_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
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
            DeleteIcon();
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
            ShowNotification(message, Properties.Settings.Default.NotificationInterval);
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

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_MYMESSAGE)
            {
                //(Int32)m.LParam & 0x0000FFFF get the low 2 bytes of LParam, we dont need the high ones. 
                //(Int32)m.WParam & 0x0000FFFF is the X coordinate and 
                //((Int32)m.WParam & 0xFFFF0000) >> 16 the Y
                switch ((Int32)m.LParam & 0x0000FFFF)
                {
                    case NIN_SELECT:
                        ToggleWindowVisibility();
                        break;

                    default:
                        break;
                }
            }

            base.WndProc(ref m);
        }

        private void ToggleWindowVisibility()
        {
            if (Visible)
            {
                Hide();
            }
            else
            {
                Show();
                Focus();
                BringToFront();
            }
        }
    }
}
