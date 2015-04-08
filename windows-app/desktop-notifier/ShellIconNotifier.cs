using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows;
using System.Diagnostics;
using System.Windows.Forms;

namespace desktop_notifier
{
    class ShellIconNotifier : DesktopNotifierInterface
    {

        public IntPtr HANDLE { get; set; }
        public Guid GUID { get; set; }
        public IntPtr HICON { get; set; }

        public const Int32 WM_MYMESSAGE = 0x8000; //WM_APP
        public const Int32 NOTIFYICON_VERSION_4 = 0x4;

        public const Int32 NIIF_USER = 0x4;
        public const Int32 NIIF_NONE = 0x0;
        public const Int32 NIIF_INFO = 0x1;
        public const Int32 NIIF_WARNING = 0x2;
        public const Int32 NIIF_ERROR = 0x3;
        public const Int32 NIIF_LARGE_ICON = 0x20;

        public enum NotifyFlags
        {
            NIF_MESSAGE = 0x01, NIF_ICON = 0x02, NIF_TIP = 0x04, NIF_INFO = 0x10, NIF_STATE = 0x08,
            NIF_GUID = 0x20, NIF_SHOWTIP = 0x80
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
            data.hWnd = HANDLE;
            data.guidItem = GUID;
            data.hIcon = HICON;
            data.szTip = "Desktop Notifier";

            data.uCallbackMessage = WM_MYMESSAGE; //This is the message sent to our app

            data.uFlags = NotifyFlags.NIF_ICON | NotifyFlags.NIF_GUID | NotifyFlags.NIF_MESSAGE | NotifyFlags.NIF_TIP |
                          NotifyFlags.NIF_SHOWTIP;

            Shell_NotifyIcon(NotifyCommand.NIM_ADD, ref data);

            data.uVersion = NOTIFYICON_VERSION_4;
            Shell_NotifyIcon(NotifyCommand.NIM_SETVERSION, ref data);
        }

        private void DeleteIcon()
        {
            NOTIFYICONDATA data = new NOTIFYICONDATA();
            data.cbSize = Marshal.SizeOf(data);
            data.uFlags = NotifyFlags.NIF_GUID;
            data.guidItem = GUID;

            Shell_NotifyIcon(NotifyCommand.NIM_DELETE, ref data);
        }

        private void AddBalloon(Message message)
        {
            NOTIFYICONDATA data;
            data = new NOTIFYICONDATA();

            data.cbSize = Marshal.SizeOf(data);
            data.guidItem = GUID;

            data.dwInfoFlags = NIIF_USER | NIIF_LARGE_ICON;
            data.hBalloonIcon = IntPtr.Zero;
            data.hIcon = HICON;
            if (message.Image != null)
            {
                data.hBalloonIcon = ((Bitmap)message.Image).GetHicon();
            }
            data.szInfo = message.Text;
            data.szInfoTitle = message.Title;

            data.uFlags = NotifyFlags.NIF_INFO | NotifyFlags.NIF_SHOWTIP | NotifyFlags.NIF_GUID;

            Console.WriteLine(Shell_NotifyIcon(NotifyCommand.NIM_MODIFY, ref data));
        }

        public void ShowNotification(Message message, int timeout)
        {
            Console.WriteLine(message);
            Console.WriteLine("Message shown: {0} : {1}", message.Title, message.Text);

            
            AddBalloon(message);
        }

        public void CleanUp()
        {
            DeleteIcon();
        }

        public void Initialize(IntPtr hwnd, IntPtr hicon)
        {
            HANDLE = hwnd;
            GUID = Marshal.GetTypeLibGuidForAssembly(Assembly.GetExecutingAssembly());
            HICON = hicon;

            AddIcon();
        }
        
    }
}
