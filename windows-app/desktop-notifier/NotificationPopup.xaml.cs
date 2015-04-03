using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace desktop_notifier
{
    /// <summary>
    /// Interaction logic for NotificationPopup.xaml
    /// </summary>
    public partial class NotificationPopup : UserControl
    {
        public NotificationPopup()
        {
            InitializeComponent();
        }

        public string NotificationTitle
        {
            get { return (string)GetValue(NotificationTitleProperty); }
            set { SetValue(NotificationTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotificationTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationTitleProperty =
            DependencyProperty.Register("NotificationTitle", typeof(string), typeof(NotificationPopup), new UIPropertyMetadata(""));


        public string Label
        {
            get { return label1.Content.ToString(); }
            set { label1.Content = value; }
        }
    }
}
