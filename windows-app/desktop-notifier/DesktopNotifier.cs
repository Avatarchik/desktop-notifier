using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

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
            SetCheckBoxSilent(checkBoxEnableNotifications,true);
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
            GetDesktopNotifierInterface().ShowNotification(message);
        }

        private DesktopNotifierInterface GetDesktopNotifierInterface()
        {
            //return new FancyDesktopNotifier(Icon);
            return new WindowsDefaultNotifier(notifyIcon);
        }

        private void DesktopNotifier_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon.Visible = false;
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
        }

        private void Stop()
        {
            if (listeningThread == null) { return; }
            comm.Stop();
            listeningThread.Abort();
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
    }
}
