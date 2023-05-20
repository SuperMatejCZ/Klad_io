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

        public class Type_Join
        {
            public string PlayerName;

            public Type_Join(string _playerName)
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
    }
}
