using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Jsons
{
    public class Json_ServerInfo
    {
        public int Id;
        public string Name;
        public string Ip;
        public int PingPort;

        public Json_ServerInfo(int _id, string _name, string _ip)
        {
            Id = _id;
            Name = _name;
            Ip = _ip;
            PingPort = 5001;
        }
    }
}
