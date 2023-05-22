using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Server.Jsons
{
    public interface BaseJson<T>
    {
        SocketMessageType MessageType { get; }
        T Data { get; }
    }
}
