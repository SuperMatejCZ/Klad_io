using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Jsons
{
    public class Json_MapData : BaseJson<Json_MapData.DataType>
    {
        public SocketMessageType MessageType => SocketMessageType.MapData;

        public DataType Data { get; set; }

        public Json_MapData(string mapName)
        {
            Data = new DataType()
            {
                Map = mapName, 
            };
        }

        public class DataType
        {
            public string Map; // Name
        }
    }
}
