using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io
{
    public enum EventType : byte
    {
        JoinGame = 1,
        LeaveGame = 2,
        SendChat = 3,
        KillPlayer = 4
    }
}
