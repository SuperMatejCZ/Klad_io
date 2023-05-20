using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Jsons
{
    // {"MessageType":1,"Data":{"Name":"","Weapon":7,"CharacterData":[0,16631808,0,10509083,0,4960991,0,2385187]}}
    public class Json_PlayerInit
    {
        public string Name;
        public byte Weapon;
        public uint[] CharacterData;
    }
}
