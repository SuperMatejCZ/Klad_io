using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Jsons
{
    public class Json_ReloadReady : BaseJson<object>
    {
        public SocketMessageType MessageType => SocketMessageType.ReloadReady;

        public object Data => null;
    }
}
