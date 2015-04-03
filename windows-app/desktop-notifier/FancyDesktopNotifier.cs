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

        public void ShowNotification(Message message)
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
    }
}
