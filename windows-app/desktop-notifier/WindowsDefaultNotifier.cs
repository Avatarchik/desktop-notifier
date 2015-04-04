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

        public void ShowNotification(Message message, int timeout)
        {
            Console.WriteLine("Message shown: {0} : {1}", message.Title, message.Text);
            ShowIcon(message.Image);
            notifyIcon.Visible = false;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(timeout, message.Title, message.Text, ToolTipIcon.None);
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
