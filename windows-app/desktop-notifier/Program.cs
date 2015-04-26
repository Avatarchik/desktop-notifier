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
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;


[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile="DesktopNotifier.log4net.config")]

namespace desktop_notifier
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                StartApplication();
            }
            catch (Exception e)
            {
                log.Error("Application error", e);
            }
        }

        private static void StartApplication()
        {
            log.Info("Application started");

            // We want a single instance of this app
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "Desktop-Notifier", out createdNew))
            {
                log.InfoFormat("Another instance running already? {0}", !createdNew);
                if (createdNew)
                {
                    // If no other instance exists, create a new one
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new DesktopNotifier());
                }
                else
                {
                    // If another instance exists, disaply that instance
                    Process currentProcess = Process.GetCurrentProcess();
                    Process otherProcess = Process.GetProcessesByName(currentProcess.ProcessName)
                                                    .Where(process => process.Id != currentProcess.Id)
                                                    .First();
                    log.InfoFormat("Another instance pid: {0}", otherProcess.Id);
                    SetForegroundWindow(otherProcess.MainWindowHandle);
                }
            }
        }
    }
}
