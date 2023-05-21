using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Klad_io
{
    static class PingServer
    {
        static HttpListener listener;

        public static void Start()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:5001/");
            listener.Start();

            Thread t = new Thread(() =>
            {
                while (true) {
                    HttpListenerContext context = listener.GetContext();
                    try {
                        HandleRequest(context);
                    }
                    catch (Exception ex) {
                        Log.Exception(ex);
                    }
                }
            });
            t.Start();
        }

        static void HandleRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string url = request.Url.AbsolutePath;

            StreamReader reader = new StreamReader(request.InputStream);
            string s = reader.ReadToEnd();
            reader.Dispose();

            Log.Info($" PING \"{url}\" \"{request.Url}\" " + s);

            response.StatusCode = 200;
            response.OutputStream.Flush();
            response.Close();
        }
    }
}
