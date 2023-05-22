using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Klad_io.Server.Jsons
{
    public class Json_SendStats : BaseJson<Json_SendStats.DataType>
    {
        public SocketMessageType MessageType => SocketMessageType.SendStats;

        public DataType Data { get; set; }

        public Json_SendStats(List<Player> _players)
        {
            Data = new DataType(_players);
        }

        public class DataType
        {
            public List<PlayerType> Players;

            public DataType(List<Player> _players)
            {
                Players = new List<PlayerType>();
                for (int i = 0; i < _players.Count; i++)
                    Players.Add(new PlayerType()
                    {
                        Id = _players[i].ID,
                        Kill = _players[i].Killed,
                        Name = _players[i].Name,
                    });

                Players.Sort((a, b) =>
                {
                    int _a = a.Kill * 2 - a.Dead;
                    int _b = b.Kill * 2 - b.Dead;
                    if (_a != _b)
                        return _b - _a;
                    else
                        return string.Compare(a.Name, b.Name);
                });
            }
        }

        public class PlayerType
        {
            public string Name;
            public byte Id;
            public int Kill;
            public int Dead;
        }
    }
}
