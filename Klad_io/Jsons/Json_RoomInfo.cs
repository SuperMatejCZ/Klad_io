using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Jsons
{
    public class Json_RoomInfo
    {
        public string WS;
        public int Id;
        public int RoomCount;
        public int PlayerCount;

        public Json_RoomInfo(string _ws, int _id, int _roomCount, int _playerCount)
        {
            WS = _ws;
            Id = _id;
            RoomCount = _roomCount;
            PlayerCount = _playerCount;
        }
    }
}
