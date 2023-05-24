using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Klad_io
{
    public class MapData
    {
        public string[] Textures;
        public Polygon[] Polygons;

        public static void Load()
        {
            MapData exampleMap = new MapData()
            {
                Textures = new string[]
                {
                    "ground.png",
                },
                Polygons = new Polygon[]
                {
                    new Polygon(new float[] {
                        -500f, -200f,
                        -500f, -10f,
                        2000f, -10f,
                        3000f, 80f,
                        2000f, -200f,
                    }, "ground.png")
                }
            };
            string exampleMapPath = Program.BasePath + "/exampleMap.json";
            if (!File.Exists(exampleMapPath))
                File.WriteAllText(exampleMapPath, JsonConvert.SerializeObject(exampleMap, Formatting.Indented));
        }
    }

    public class Polygon
    {
        public float[] Vertice; // x, y, x, y...
        public string Texture; // name of texture

        public Polygon(float[] _vertice, string _texture)
        {
            Vertice = _vertice;
            Texture = _texture;
        }
    }
}
