using System;
using Newtonsoft.Json;
using SystemPlus.Vectors;

namespace Klad_io
{
    public class Player
    {
        public Vector2 Pos;
        public Vector2 Velocity;
        public byte ID;
        public string SocketID;
        public short AimX;
        public short AimY;
        public byte Health;
        public byte Weapon;
        public byte BulletsToFire;
        public byte Dead;

        [JsonIgnore]
        public string Name;
        [JsonIgnore]
        public DateTime LastTimeShot;
        [JsonIgnore]
        public int Bullets;
        [JsonIgnore]
        public WeaponInfo WeaponInfo;

        public byte CharacterData0;
        public uint CharacterData1;
        public byte CharacterData2;
        public uint CharacterData3;
        public byte CharacterData4;
        public uint CharacterData5;
        public byte CharacterData6;
        public uint CharacterData7;


        /*CharacterData:[
				e.getUint8(o+25),
				e.getUint32(o+26),
				e.getUint8(o+30),
				e.getUint32(o+31),
				e.getUint8(o+35),
				e.getUint32(o+36),
				e.getUint8(o+40),
				e.getUint32(o+41)
				]
			};*/

        public Player(Vector2 _pos, byte _id, string _socketID)
        {
            Pos = _pos;
            Velocity = Vector2.Zero;
            ID = _id;
            SocketID = _socketID;

            Health = 100;

            Dead = 0;
        }
    }
}