using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemPlus.Vectors;

namespace Klad_io
{
    public class WeaponInfo
    {
        public int Id;
        public int Order;
        public BulletProperties BulletProperties;
        public int ClipSize;
        public int ReloadTime; // miliseconds?
        public int ShotDelay; // miliseconds?
        public string SpritePath;
        public string SFX;
        public string ReloadSFX;
        public float Scale;
        public Vector2 Position;
        public int Recoil;
    }

    public class BulletProperties
    {
        public float[] Size;
        public float Mass;
        public int Damage;
        public string SpritePath;
        public int Speed;
        public float[] SpriteSize;
        public int Tint;
    }
}
