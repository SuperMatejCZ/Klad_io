using Klad_io.Jsons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klad_io
{
    static class Program
    {
        static HttpListener listener;
        internal static TcpClient client = new TcpClient();
        public static readonly string BasePath = @"E:\download\websites\klad.io";

        static List<ServerInfo> servers = new List<ServerInfo>();

        static void Main(string[] args)
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:80/");
            listener.Start();

            Thread t = new Thread(Loop);
            t.Start();

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
        
        static void HandleRequest(HttpListenerContext context)
        {
            DateTime now = DateTime.Now;
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string url = request.Url.AbsolutePath;

            Log.Debug($"\"{url}\" \"{request.Url}\"");

            string ContentText()
            {
                StreamReader reader = new StreamReader(request.InputStream);
                string s = reader.ReadToEnd();
                reader.Dispose();
                return s;
            }

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
                case "/getServers":
                    List<Json_GetServer> serversResponse = new List<Json_GetServer>();
                    for (int i = 0; i < servers.Count; i++)
                        serversResponse.Add(new Json_GetServer()
                        {
                            Id = i + 1,
                            Ip = servers[i].Ip,
                            Name = servers[i].Name,
                            PingPort = 0,
                        });
                    RespJson(serversResponse);
                    break;
                case "/getRoom":
                    int serverId = int.Parse(request.QueryString.Get("serverId"));
                    if (serverId < 1 || serverId > servers.Count)
                        RespNotFound();
                    else {
                        ServerInfo info = servers[serverId - 1];
                        string[] split = info.InfoIp.Split(':');
                        client.Connect(new IPEndPoint(IPAddress.Parse(split[0]), int.Parse(split[1])));
                        while (client.Available < 4) Thread.Sleep(0);
                        byte[] buffer = new byte[4];
                        client.Client.Receive(buffer);
                        client.Close();
                        client = new TcpClient();
                        int numbPlayers = BitConverter.ToInt32(buffer, 0);
                        Json_GetRoom roomResponse = new Json_GetRoom()
                        {
                            Id = serverId,
                            WS = info.WS,
                            RoomCount = 1,
                            PlayerCount = numbPlayers
                        };
                        RespJson(roomResponse);
                    }
                    break;
                case "/registerServer":
                    ServerInfo registerRequest = JsonConvert.DeserializeObject<ServerInfo>(ContentText());
                    servers.Add(registerRequest);
                    Json_RegisterServerResponse registerResponse = new Json_RegisterServerResponse()
                    {
                        BasePath = BasePath,
                        Id = servers.Count,
                    };
                    RespJson(registerResponse);
                    Log.Info($"Server registered itself Name: {registerRequest.Name}, Ip: {registerRequest.Ip}:{registerRequest.Port}");
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
