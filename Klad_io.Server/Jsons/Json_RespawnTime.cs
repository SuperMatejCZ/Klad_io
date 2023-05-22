using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Server.Jsons
{
    public class Json_RespawnTime : BaseJson<int>
    {
        public SocketMessageType MessageType => SocketMessageType.RespawnTime;

        public int Data { get; set; }
    }
}
