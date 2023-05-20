using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Jsons
{
    public class Json_Event : BaseJson<List<Json_Event.DataTypeA>>
    {
        public SocketMessageType MessageType => SocketMessageType.SendEvents;

        public List<DataTypeA> Data { get; set; }

        public Json_Event(object value, EventType eventType)
        {
            Data = new List<DataTypeA>() {
                new DataTypeA()
                {
                    Data = value,
                    Type = eventType
                },
            };
        }

        public class DataTypeA
        {
            public EventType Type;

            public object Data;
        }

        public class Type_JoinLeave
        {
            public string PlayerName;

            public Type_JoinLeave(string _playerName)
            {
                PlayerName = _playerName;
            }
        }

        public class Type_Chat
        {
            public string PlayerName;
            public string Text;

            public Type_Chat(string _playerName, string _message)
            {
                PlayerName = _playerName;
                Text = _message;
            }
        }

        //t.KillerId,t.KillerName,t.KilledId,t.KilledName,t.WeaponId
        public class Type_Kill
        {
            public byte KillerId;
            public string KillerName;
            public byte KilledId;
            public string KilledName;
            public byte WeaponId;

            public Type_Kill(Player killer, Player killed)
            {
                KillerId = killer.ID;
                KillerName = killer.Name;
                KilledId = killed.ID;
                KilledName = killed.Name;
                WeaponId = killer.Weapon;
            }
        }
    }
}
