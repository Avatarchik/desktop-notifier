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

using CustomUIControls;
using System.Drawing;
using Hardcodet.Wpf.TaskbarNotification;
using desktop_notifier.Properties;
using System.Windows;

namespace desktop_notifier
{
    class FancyDesktopNotifier : DesktopNotifierInterface
    {
        TaskbarIcon taskbarIcon;

        public FancyDesktopNotifier(Icon defaultIcon)
        {
            taskbarIcon = new TaskbarIcon();
            taskbarIcon.Icon = defaultIcon;
        }

        public void ShowNotification(Message message, int timeout)
        {
            
            taskbarIcon.ToolTipText = "Desktop Notifier";
            taskbarIcon.Visibility = Visibility.Visible;

            //NotificationPopup popup = new NotificationPopup();
            //popup.label1.Content = message.Text;

            Samples.FancyPopup popup = new Samples.FancyPopup();
            
            taskbarIcon.ShowCustomBalloon(popup, System.Windows.Controls.Primitives.PopupAnimation.Fade, 1000);
           // taskbarIcon.TrayPopup = popup;
            Console.WriteLine("Displayed");
            
            //taskbarNotifier.Text = message.Text;
            //taskbarNotifier.TitleText = message.Title;
            //if(message.Image != null)
            //    taskbarNotifier.SetBackgroundBitmap(message.Image, Color.AliceBlue); ;

            //taskbarNotifier.Show(message.Title, message.Text, 1000, 10000, 1000);
        }

        public void Initialize(IntPtr hwnd, IntPtr hicon)
        {

        }

        public void CleanUp()
        {

        }
    }
}
