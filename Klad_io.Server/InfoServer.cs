using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Klad_io.Server
{
    static class InfoServer
    {
        private static TcpListener listener;
        private static Thread thread;

        public static void Start(int port)
        {
            listener = new TcpListener(new IPEndPoint(IPAddress.Loopback, port));
            listener.Start();

            thread = new Thread(Loop);
            thread.Start();

            Log.Info("Succesfully started info server");
        }

        private static void Loop()
        {
            while (true) {
                if (!listener.Pending()) Thread.Sleep(0);
                Log.Debug("Info requested");
                TcpClient client = listener.AcceptTcpClient();
                client.Client.Send(BitConverter.GetBytes(Program.GetPlayerCount()));
                client.Close();
            }
        }
    }
}
