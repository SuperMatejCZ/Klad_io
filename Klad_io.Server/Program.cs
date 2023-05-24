using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Klad_io.Server.Jsons;
using Newtonsoft.Json;

namespace Klad_io.Server
{
    static class Program
    {
        internal static HttpClient client;
        internal static readonly string CurrentDirectory = Environment.CurrentDirectory;
        internal static string BasePath;
        internal static Json_Config Config;
        internal static int ServerId;
        internal static Func<int> playerCount;

        static async Task Main(string[] args)
        {
            Console.ResetColor();

            string configPath = CurrentDirectory + "/config.json";

            if (!File.Exists(configPath)) {
                File.WriteAllText(configPath, JsonConvert.SerializeObject(new Json_Config()
                {
                    MainServerEndPoint = "127.0.0.1:80",
                    ServerIp = "127.0.0.1",
                    ServerPort = 5002,
                    InfoEndPoint = "127.0.0.1:5003",
                    ServerName = "Europe",
                    KillsToWin = 25
                }, Formatting.Indented));
                Log.Info($"Config file ({configPath}) was created, make sure that all settings are correct");
                Log.PressAnyKey();
            }

            Config = JsonConvert.DeserializeObject<Json_Config>(File.ReadAllText(configPath));

            InfoServer.Start(int.Parse(Config.InfoEndPoint.Split(':')[1]));

            client = new HttpClient();

            Json_RegisterServerRequest registerRequest = new Json_RegisterServerRequest()
            {
                Name = Config.ServerName,
                Ip = Config.ServerIp,
                Port = Config.ServerPort,
                InfoIp = Config.InfoEndPoint,
                WS = $"ws://{Config.ServerIp}:{Config.ServerPort}",
                RoomCount = 1,
            };

            HttpResponseMessage response = null;
            try {
                response = await client.PostAsync("http://" + Config.MainServerEndPoint + "/registerServer",
                    new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8, "application/json"));
            } catch (Exception ex) {
                Log.Error("Failed to register to main server");
                Log.Exception(ex);
                Log.PressAnyKey("", true);
            }

            Json_RegisterServerResponse registerResponse = JsonConvert.DeserializeObject<Json_RegisterServerResponse>(await response.Content.ReadAsStringAsync());

            ServerId = registerResponse.Id;
            BasePath = registerResponse.BasePath;

            Log.Info($"Succesfully registered to main server");

            Server.Start(Config.ServerIp, Config.ServerPort);

            while (true) { Thread.Sleep(0); }
        }

        internal static int GetPlayerCount()
        {
            if (playerCount == null)
                return 0;
            else
                return playerCount();
        }
    }
}
