using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Server.Jsons
{
    public class Json_GameStart : BaseJson<Json_GameStart.DataType>
    {
        public SocketMessageType MessageType => SocketMessageType.GameStart;

        public DataType Data { get; set; }

        public Json_GameStart(string mapName)
        {
            Data = new DataType()
            {
                Map = new DataType.MapType()
                {
                    File = mapName
                },
            };
        }

        public class DataType
        {
            public MapType Map;

            public class MapType
            {
                public string File;
            }
        }
    }
}
