using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io
{
    public enum SocketMessageType : byte
    {
        JoinGame = 1, // ?
        Leave = 2, // ?
        SendData = 3, // ?
        JoinAccepted = 4,
        ReloadReady = 6,
        RespawnTime = 7,
        SendStats = 8,
        PingRequest = 9, // ?
        PingAnswer = 10, // ?
        SendEvents = 11,
        SendChat = 12, // ?
        JoinServer = 13, // ?
        GameEnd = 14,
        GameStart = 15,
        Disconnect = 16,
        MapData = 17,
    }
}
