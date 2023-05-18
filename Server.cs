using Klad_io.Jsons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemPlus.Vectors;
using WebSocketSharp;
using WebSocketSharp.Server;

using File = System.IO.File;

namespace Klad_io
{
    public static class Server
    {
        public static WeaponInfo[] Weapons;


        private static List<Player> Players = new List<Player>();

        private static Random rng = new Random();

        public static void Start()
        {
            Weapons = JsonConvert.DeserializeObject<WeaponInfo[]>(File.ReadAllText(Program.BasePath + "/assets/weapondata.json"));

            WebSocketServer wssv = new WebSocketServer("ws://127.0.0.1:5002");

            wssv.AddWebSocketService<ServerSocket>("/");
            wssv.Start();
        }


        class ServerSocket : WebSocketBehavior
        {
            protected override void OnOpen()
            {
                Console.WriteLine($"[Socket] Player Connected, IP: {Sessions[ID].Context.UserEndPoint}");

                Player player = new Player(new Vector2(2, 12), (byte)rng.Next(0, 256), ID);

                AcceptJoin(ID, player);

                SendMapData(ID);
                StartGame();

                SendPlayerData(player);
            }

            protected override void OnError(ErrorEventArgs e)
            {
                Console.WriteLine($"[Socket] Error: {e.Message}");
                Klad_io.Log.Exception(e.Exception);
            }
            // {"MessageType":1,"Data":{"Name":"","Weapon":7,"CharacterData":[0,16631808,0,10509083,0,4960991,0,2385187]}}
            protected override void OnMessage(MessageEventArgs e) 
            {
                Console.WriteLine("[Socket] Received message: " + e.Data); 
                if (!e.IsText) {
                    throw new Exception("Socket message isn't text");
                }
            }

            protected override void OnClose(CloseEventArgs e)
            {
                Console.WriteLine($"[Socket] Player Disconnected");
            }

            private void StartGame()
            {
                Json_GameStart gameStart = new Json_GameStart();
                BroadcastJson(gameStart);
            }

            private void AcceptJoin(string socketID, Player player)
            {
                Json_JoinAccepted acceptJoin = new Json_JoinAccepted(player);
                SendJson(acceptJoin, socketID);
            }
            private void SendMapData(string socketID)
            {
                Json_MapData mapData = new Json_MapData("Default0");
                SendJson(mapData, socketID);
            }

            private void SendJson(object value, string socketID)
            {
                string s = JsonConvert.SerializeObject(value);
                Console.WriteLine(s);
                Sessions.SendTo(s, socketID);
            }
            private void BroadcastJson(object value)
            {
                string s = JsonConvert.SerializeObject(value);
                Console.WriteLine(s);
                Sessions.Broadcast(s);
            }

            private void SendPlayerData(Player player)
            {
                byte[] buffer = new byte[54];
                SaveWriter writer = new SaveWriter(buffer);
                writer.WriteUInt8(52);
                writer.WriteUInt8(1);
                writer.WriteFloat(player.Pos.x);
                writer.WriteFloat(player.Pos.y);
                writer.WriteFloat(player.Velocity.y);
                writer.WriteFloat(player.Velocity.y);
                writer.WriteUInt8(player.ID);
                writer.WriteInt16(player.AimX);
                writer.WriteInt16(player.AimY);
                writer.WriteUInt8(player.Health);
                writer.WriteUInt8(player.Weapon);
                writer.WriteUInt8(player.BulletsToFire);
                writer.WriteUInt8(player.Dead);
                writer.WriteUInt8(player.FuelEmit);
                writer.WriteUInt8(player.ResetPosition);

                writer.WriteUInt8(player.CharacterData0);
                writer.WriteUInt32(player.CharacterData1);
                writer.WriteUInt8(player.CharacterData2);
                writer.WriteUInt32(player.CharacterData3);
                writer.WriteUInt8(player.CharacterData4);
                writer.WriteUInt32(player.CharacterData5);
                writer.WriteUInt8(player.CharacterData6);
                writer.WriteUInt32(player.CharacterData7);

                writer.WriteUInt8(0); // l variable (number of bytes to... discart???)
                writer.WriteUInt32(0); // tick

                Sessions.Broadcast(buffer);
            }
        }
    }
}
