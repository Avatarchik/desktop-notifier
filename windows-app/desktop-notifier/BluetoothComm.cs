using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.IO;

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
                    BluetoothClient client = listener.AcceptBluetoothClient();
                    string message = ReadMessage(client.GetStream());
                    //Console.WriteLine(message);

                    SendMessage(message);
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

        private string ReadMessage(Stream stream)
        {
            string message = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                message = reader.ReadLine();
            }
            return message;
        }
    }
}
