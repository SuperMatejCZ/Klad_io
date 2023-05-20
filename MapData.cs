using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io
{
    public class MapData
    {
        public string[] Textures;
        public Polygon[] Polygons;

        public static MapData map0;

        static MapData()
        {
            map0 = new MapData()
            {
                Textures = new string[]
                {
                    "bg.png",
                    "ground.png",
                },
                Polygons = new Polygon[]
                {
                    new Polygon(new float[] {
                        -50f, -200f,
                        -50f, -10f,
                        800f, -10f,
                        800f, -200f,
                    }, "ground.png")
                }
            };
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
