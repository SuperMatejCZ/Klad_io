using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io
{
    public enum SocketMessageType : byte
    {
        JoinAccepted = 4,
        ReloadReady = 6,
        RespawnTime = 7,
        SendStats = 8,
        SendEvents = 11,
        GameEnd = 14,
        GameStart = 15,
        Disconnect = 16,
        MapData = 17,
    }
}
