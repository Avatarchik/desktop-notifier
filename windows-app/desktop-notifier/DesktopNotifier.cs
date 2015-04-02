using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace desktop_notifier
{
    public partial class DesktopNotifier : Form
    {
        public delegate void MessageReceivedEvent(Message message);
        private BluetoothComm comm = new BluetoothComm();

        public DesktopNotifier()
        {
            InitializeComponent();
            //checkBoxEnableNotifications.Checked = true;
        }

        private void DesktopNotifier_Load(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                StartInternal();
            }, null);
        }

        private void StartInternal()
        {
            comm.Callback = new MessageReceivedEvent(MessageReceived);
            comm.Start();
        }

        public void MessageReceived(Message message)
        {
            Console.WriteLine("Message shown: {0} : {1}", message.Title, message.Text);
            // TODO: Display image also
            notifyIcon.ShowBalloonTip(5000, message.Title, message.Text, ToolTipIcon.Info);
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
            comm.Stop();
        }
    }
}
