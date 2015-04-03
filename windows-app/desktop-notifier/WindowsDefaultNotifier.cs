using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace desktop_notifier
{
    class WindowsDefaultNotifier : DesktopNotifierInterface
    {
        private NotifyIcon notifyIcon;
        public WindowsDefaultNotifier(NotifyIcon notifyIcon)
        {
            this.notifyIcon = notifyIcon;
        }

        public void ShowNotification(Message message)
        {
            Console.WriteLine("Message shown: {0} : {1}", message.Title, message.Text);
            ShowIcon(message.Image);
            notifyIcon.Visible = false;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(5000, message.Title, message.Text, ToolTipIcon.Info);
        }

        private void ShowIcon(Image image)
        {
            if (image == null)
                return;
            Bitmap bitmap = (Bitmap)image;
            notifyIcon.Icon = Icon.FromHandle(bitmap.GetHicon());
            Console.WriteLine("Icon shown");
        }
    }
}
