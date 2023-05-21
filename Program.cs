using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klad_io
{
    static class Program
    {
        static HttpListener listener;
        public static readonly string BasePath = @"E:\download\websites\klad.io";

        static void Main(string[] args)
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:80/");
            listener.Start();

            Thread t = new Thread(Loop);
            t.Start();

            Server.Start();
            PingServer.Start();

            Console.WriteLine("Server started");

            while (true) { Thread.Sleep(0); }
        }

        static void Loop()
        {
            while (true) {
                HttpListenerContext context = listener.GetContext();
                try {
                    HandleRequest(context);
                } catch (Exception ex) {
                    Log.Exception(ex);
                }
            }
        }

        /*
         if killed >= points to win
            GameEnd
         */

        static void HandleRequest(HttpListenerContext context)
        {
            DateTime now = DateTime.Now;
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string url = request.Url.AbsolutePath;

            Log.Info($"\"{url}\" \"{request.Url}\"");

            void Resp()
            {
                response.OutputStream.Flush();
                response.Close();
            }

            void RespFile(string path)
            {
                byte[] bytes = File.ReadAllBytes(path);
                response.StatusCode = 200;
                response.OutputStream.Write(bytes, 0, bytes.Length);
                Resp();
            }
            void RespJson(object value)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
                response.StatusCode = 200;
                response.OutputStream.Write(bytes, 0, bytes.Length);
                Resp();
            }
            void RespNotFound()
            {
                response.StatusCode = 404;
                Resp();
            }

            switch (url) {
                case "/":
                    RespFile(BasePath + "/index.html");
                    break;
                case "/getRoom":
                    string serverId = request.QueryString.Get("serverId");
                    if (serverId == "1")
                        RespFile(BasePath + "/getRoom%3fserverId=1/index.html");
                    else if (serverId == "2")
                        RespFile(BasePath + "/getRoom%3fserverId=2/index.html");
                    else
                        Log.Error($"Unexpected serverId: {serverId}");
                    break;
                case "/data/maps/Default0":
                    RespJson(MapData.map0);
                    break;
                default: {
                        string path = BasePath + url;
                        check:
                        if (File.Exists(path)) {
                            RespFile(path);
                        }
                        else {
                            if (!url.Contains(".")) {
                                path += "/index.html";
                                url += "/index.html";
                                goto check;
                            }
                            Log.Error($"File not found: {path}");
                            RespNotFound();
                        }
                    }
                    break;
            }
        }
    }
}
