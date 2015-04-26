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

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace desktop_notifier
{
    class BluetoothComm
    {
        public desktop_notifier.DesktopNotifier.MessageReceivedEvent Callback {get; set; }

        private BluetoothListener listener;
        private static readonly Guid GUID = new Guid("00001101-0000-1000-8000-00805f9b34fb");
        private volatile bool run = false;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BluetoothComm()
        {
            listener = new BluetoothListener(GUID);
        }

        public void Start()
        {
            run = true;
            StartListening();
        }

        public void Stop()
        {
            log.Info("Stopping");
            run = false;
        }

        private void StartListening()
        {
            log.Info("Listening");
            listener.Start();
            run = true;
            while (run)
            {
                if (listener.Pending())
                {
                    log.Info("Got a new client");
                    using (BluetoothClient client = listener.AcceptBluetoothClient())
                    {
                        ReadMessageAsync(client);
                    }
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
        }

        private void SendMessage(string messageText)
        {
            int oldLength = messageText.Length;
            messageText = messageText.Replace(":END_OF_MESSAGE:", "");
            int newLength = messageText.Length;
            // Invalid message?
            if (oldLength == newLength)
            {
                return;
            }

            Message message = new Message(messageText);
            Callback.Invoke(message);
        }

        private void ReadMessageAsync(BluetoothClient client)
        {
            Thread t = new Thread(() => ReadMessage(client));
            t.Start();
            if (!t.Join(TimeSpan.FromSeconds(30)))
            {
                t.Abort();
            }
        }

        private void ReadMessage(BluetoothClient client)
        {
            String message = "";
            Stopwatch watch = new Stopwatch();
            watch.Start();
            bool timedout = false;

            using (StreamReader reader = new StreamReader(client.GetStream()))
            {
                while(true) 
                {
                    int available = reader.Peek();
                    if (available >= 0)
                    {
                        char[] buffer = new char[available];
                        int read = reader.Read(buffer, 0, available);
                        if (read > 0)
                        {
                            string line = new string(buffer, 0, read);
                            //log.Info(line);
                            message += line;
                        }

                        if (message.Contains(":END_OF_MESSAGE:"))
                            break;
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }

                    // Did we wait too long while reading this stream?
                    if (watch.Elapsed.Seconds > 15)
                    {
                        timedout = true;
                        break;
                    }
                }
            }

            if (timedout)
            {
                log.InfoFormat("Timedout: {0} {1}", watch.Elapsed, message);
            }
            else
            {
                log.InfoFormat("{0} bytes message read in {1}s {2}ms", message.Length, watch.Elapsed.Seconds, watch.Elapsed.Milliseconds);
                SendMessage(message.ToString().Trim());
            }
        }
    }
}
