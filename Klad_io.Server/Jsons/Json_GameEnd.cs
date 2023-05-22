using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Server.Jsons
{
    public class Json_GameEnd : BaseJson<Json_GameEnd.DataType>
    {
        public SocketMessageType MessageType => SocketMessageType.GameEnd;

        public DataType Data { get; set; }

        public Json_GameEnd(byte _winnerId, List<Player> _players)
        {
            Data = new DataType()
            {
                WinnerId = _winnerId,
                Stats = new Json_SendStats.DataType(_players),
            };
        }

        public class DataType
        {
            public byte WinnerId;
            public Json_SendStats.DataType Stats;
        }
    }
}
