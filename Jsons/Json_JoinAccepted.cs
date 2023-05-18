using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Jsons
{
    public class Json_JoinAccepted : BaseJson<Json_JoinAccepted.DataType>
    {
        public SocketMessageType MessageType => SocketMessageType.JoinAccepted;

        public DataType Data { get; set; }

        public Json_JoinAccepted(Player player)
        {
            Data = new DataType
            {
                PlayerId = player.ID,
                Weapon = player.Weapon,
                Name = player.Name,
                Ping = 0,
                Tick = 0,
                MaxPoints = 0
            };
        }

        public class DataType
        {
            public byte PlayerId;
            public byte Weapon;
            public string Name;
            public int Ping;
            public int Tick;
            public int MaxPoints;
        }
    }
}
