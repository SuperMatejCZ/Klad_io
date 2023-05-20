﻿using Klad_io.Jsons;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemPlus;
using SystemPlus.Vectors;
using WebSocketSharp;
using WebSocketSharp.Server;

using File = System.IO.File;
using Log = Klad_io.Log;

namespace Klad_io
{
    public static class Server
    {
        public static WeaponInfo[] Weapons;


        private static List<Player> Players = new List<Player>();
        private static List<Player> PlayersToJoin = new List<Player>();

        private static Random rng = new Random();

        private static uint Tick;

        public static void Start()
        {
            Weapons = JsonConvert.DeserializeObject<WeaponInfo[]>(File.ReadAllText(Program.BasePath + "/assets/weapondata.json"));

            WebSocketServer wssv = new WebSocketServer("ws://127.0.0.1:5002");

            wssv.AddWebSocketService<ServerSocket>("/");
            wssv.Start();
        }

        public static WeaponInfo GetWeaponById(int id)
        {
            for (int i = 0; i < Weapons.Length; i++)
                if (Weapons[i].Id == id)
                    return Weapons[i];

            return null;
        }

        class ServerSocket : WebSocketBehavior
        {
            public void PlayerLoop()
            {
                Stopwatch watch = new Stopwatch();
                const double C = 1000d / 60d;

                while (Running) {
                    watch.Restart();

                    SendPlayerData(SendTo.All);

                    Tick += 8; // every 60 pings + 500 ticks, 60 pings every second (63)

                    while (watch.Elapsed.TotalMilliseconds < C) { Thread.Sleep(0); }
                }
            }

            private static Thread playerThread;
            private static bool Running;

            protected override void OnOpen()
            {
                if (!Running) {
                    Running = true;
                    playerThread = new Thread(PlayerLoop);
                    playerThread.Start();
                }

                Console.WriteLine($"[Socket] Player Connected, IP: {Sessions[ID].Context.UserEndPoint}");

                Player player = new Player(new Vector2(2, 12), (byte)rng.Next(0, 256), ID);
                PlayersToJoin.Add(player);

                AcceptJoin(player, SendTo.One(ID));

                SendMapData(SendTo.One(ID));
            }

            protected override void OnError(ErrorEventArgs e)
            {
                Console.WriteLine($"[Socket] Error: {e.Message}");
                Klad_io.Log.Exception(e.Exception);
            }

            /*e.prototype.Fire
             (this.PrimaryWeapon.Recoil)*/

            protected override void OnMessage(MessageEventArgs e) 
            {
                if (!e.IsText) {
                    // we only need last 25, others are outdated
                    byte[] Data = new byte[25];
                    Array.Copy(e.RawData, e.RawData.Length - 25, Data, 0, 25);
                    SaveReader reader = new SaveReader(Data);
                    byte keys = reader.ReadUInt8(); // left, right, up, down, mouse left, mouse right, reload, none
                    short aimX = reader.ReadInt16();
                    short aimY = reader.ReadInt16();
                    float posX = reader.ReadFloat();
                    float posY = reader.ReadFloat();
                    float velX = reader.ReadFloat();
                    float velY = reader.ReadFloat();
                    uint tick = reader.ReadUInt32();

                    reader.Dispose();

                    Player player = GetPlayerBySocket(ID);

                    player.AimX = aimX;
                    player.AimY = aimY;
                    player.Pos = new Vector2(posX, posY);
                    player.Velocity = new Vector2(velX, velY);

                    if ((keys & 0b_0001_0000) != 0)
                        TryFire(player);
                    if ((keys & 0b_1100_0000) != 0)
                        Reload(player);

                    SendPlayerData(SendTo.All);
                    return;
                }
                Console.WriteLine("[Socket] Received message: " + e.Data);

                BaseSocketJson json = JsonConvert.DeserializeObject<BaseSocketJson>(e.Data);
                JObject data = json.Data as JObject;

                //SocketMessageType

                switch (json.MessageType) {
                    case 1: { // startGame
                            Json_PlayerInit playerInit = data.ToObject<Json_PlayerInit>();
                            Player player = GetPlayerToJoinBySocket(ID);
                            player.Name = playerInit.Name;
                            player.Weapon = playerInit.Weapon;
                            player.CharacterData0 = (byte)playerInit.CharacterData[0];
                            player.CharacterData2 = (byte)playerInit.CharacterData[2];
                            player.CharacterData4 = (byte)playerInit.CharacterData[4];
                            player.CharacterData6 = (byte)playerInit.CharacterData[6];
                            player.CharacterData1 = playerInit.CharacterData[1];
                            player.CharacterData3 = playerInit.CharacterData[3];
                            player.CharacterData5 = playerInit.CharacterData[5];
                            player.CharacterData7 = playerInit.CharacterData[7];

                            player.LastTimeShot = DateTime.MinValue;
                            player.WeaponInfo = GetWeaponById(player.Weapon);
                            player.Bullets = player.WeaponInfo.ClipSize;

                            PlayersToJoin.Remove(player);
                            Players.Add(player);

                            SendEvent(new Json_Event.Type_Join(player.Name), EventType.JoinGame, SendTo.XOne(ID));
                            SendPlayerData(SendTo.All);
                        }
                        break;
                    case 12: { // chat
                            string message = json.Data as string;
                            Player player = GetPlayerBySocket(ID);
                            SendEvent(new Json_Event.Type_Chat(player.Name, message), EventType.SendChat, SendTo.All);
                        }
                        break;
                    default:
                        Log.Error($"Unknown message type: {json.MessageType}");
                        break;
                }

                //data.ToObject<>();
            }

            protected override void OnClose(CloseEventArgs e)
            {
                Players.Remove(GetPlayerBySocket(ID));
                Console.WriteLine($"[Socket] Player Disconnected");
            }


            private void TryFire(Player player)
            {
                if ((DateTime.Now - player.LastTimeShot).Milliseconds < player.WeaponInfo.ShotDelay || player.Bullets < 1)
                    return;

                player.Bullets--;
                player.BulletsToFire++;
                player.LastTimeShot = DateTime.Now;

                Console.WriteLine("[FIRE] " + player.Bullets);
            }


            private void StartGame(SendTo to)
            {
                Json_GameStart gameStart = new Json_GameStart();
                SendJson(gameStart, to);
            }

            private void AcceptJoin(Player player, SendTo to)
            {
                Json_JoinAccepted acceptJoin = new Json_JoinAccepted(player);
                SendJson(acceptJoin, to);
            }
            private void SendMapData(SendTo to)
            {
                Json_MapData mapData = new Json_MapData("Default0");
                SendJson(mapData, to);
            }
            private void SendEvent(object value, EventType eventType, SendTo to)
            {
                Json_Event json_Event = new Json_Event(value, eventType);
                SendJson(json_Event, to);
            }
            private void ReloadReady(SendTo to)
            {
                Json_ReloadReady reloadReady = new Json_ReloadReady();
                SendJson(reloadReady, to);
            }

            private void SendJson(object value, SendTo to)
            {
                string s = JsonConvert.SerializeObject(value);
                Send(s, to);
            }

            private void SendPlayerData(SendTo to)
            {
                if (Players.Count < 1)
                    return;

                byte[] buffer = new byte[45 * Players.Count + 2 + 5];
                SaveWriter writer = new SaveWriter(buffer);

                writer.WriteUInt8(0);
                writer.WriteUInt8((byte)Players.Count);

                for (int i = 0; i < Players.Count; i++) {
                    Player player = Players[i];
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
                    player.BulletsToFire = 0;
                    writer.WriteUInt8(player.Dead);

                    writer.WriteUInt8(player.CharacterData0);
                    writer.WriteUInt32(player.CharacterData1);
                    writer.WriteUInt8(player.CharacterData2);
                    writer.WriteUInt32(player.CharacterData3);
                    writer.WriteUInt8(player.CharacterData4);
                    writer.WriteUInt32(player.CharacterData5);
                    writer.WriteUInt8(player.CharacterData6);
                    writer.WriteUInt32(player.CharacterData7);
                }

                writer.WriteUInt8(0); // l variable (number of bytes to... discart???)
                writer.WriteUInt32(Tick); // tick

                writer.Flush();
                writer.Dispose();

                Send(buffer, to);
            }

            private void Reload(Player player)
            {
                //ReloadReady(SendTo.One(player.SocketID));
            }

            private void Send(string value, SendTo to)
            {
                if (to.Type == SendToType.One)
                    Sessions.SendTo(value, to.SocketID);
                else if (to.Type == SendToType.All)
                    Sessions.Broadcast(value);
                else
                    foreach(string id in Sessions.IDs)
                        if (id != to.SocketID)
                            Sessions.SendTo(value, id);
            }
            private void Send(byte[] value, SendTo to)
            {
                if (to.Type == SendToType.One)
                    Sessions.SendTo(value, to.SocketID);
                else if (to.Type == SendToType.All)
                    Sessions.Broadcast(value);
                else
                    foreach (string id in Sessions.IDs)
                        if (id != to.SocketID)
                            Sessions.SendTo(value, id);
            }

            private Player GetPlayerToJoinBySocket(string socketID)
            {
                for (int i = 0; i < PlayersToJoin.Count; i++)
                    if (PlayersToJoin[i].SocketID == socketID)
                        return PlayersToJoin[i];

                return null;
            }
            private Player GetPlayerBySocket(string socketID)
            {
                for (int i = 0; i < Players.Count; i++)
                    if (Players[i].SocketID == socketID)
                        return Players[i];

                return null;
            }

        }
    }

    public struct SendTo
    {
        public static SendTo All = new SendTo(SendToType.All);

        public static SendTo One(string _socketID = "") => new SendTo(SendToType.One, _socketID);
        public static SendTo XOne(string _socketID = "") => new SendTo(SendToType.XOne, _socketID);

        public SendToType Type;
        public string SocketID;

        public SendTo(SendToType _type, string _socketID = "")
        {
            Type = _type;
            SocketID = _socketID;
        }
    }

    public enum SendToType : byte
    {
        One, // to SocketID
        XOne, // to all except SocketID
        All // to all
    }
}
