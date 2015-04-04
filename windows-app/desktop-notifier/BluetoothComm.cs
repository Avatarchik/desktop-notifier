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
            Console.WriteLine("Stopping");
            run = false;
        }

        private void StartListening()
        {
            Console.WriteLine("Listening");
            listener.Start();
            run = true;
            while (run)
            {
                if (listener.Pending())
                {
                    Console.WriteLine("Got a new client");
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
                            //Console.WriteLine(line);
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
                Console.WriteLine("Timedout: " + watch.Elapsed + " " + message);
            }
            else
            {
                Console.WriteLine("Message received: " + message);
                SendMessage(message.ToString().Trim());
            }
        }
    }
}
