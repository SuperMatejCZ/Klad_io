using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io
{
    public class SaveWriter : IDisposable
    {
        private Stream stream;

        public long Position { get => stream.Position; set => stream.Position = value; }

        public long Length => stream.Length;

        public SaveWriter(byte[] _bytes)
        {
            stream = new MemoryStream(_bytes);
            if (!stream.CanWrite)
                throw new Exception("Can't write to stream");
            Position = 0;
        }

        public SaveWriter(Stream _stream)
        {
            stream = _stream;
            if (!stream.CanWrite)
                throw new Exception("Can't write to stream");
            Position = 0;
        }

        public SaveWriter(string _path, bool clear)
        {
            if (!File.Exists(_path) || clear)
                File.WriteAllBytes(_path, new byte[] { });

            stream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write);
            if (!stream.CanWrite)
                throw new Exception("Can't write to stream");
            Position = 0;
        }

        public void Reset() => stream.Position = 0;

        public void WriteBytes(byte[] bytes) => WriteBytes(bytes, 0, bytes.Length);
        public void WriteBytes(byte[] bytes, int offset, int count)
        {
            stream.Write(bytes, offset, count);
        }

        public void WriteByteArray(byte[] value)
        {
            WriteInt32(value.Length);
            WriteBytes(value);
        }

        public void WriteBool(bool value)
        {
            WriteUInt8((byte)(value ? 1 : 0));
        }

        public void WriteInt8(sbyte value)
        {
            WriteBytes(new byte[] { (byte)value });
        }

        public void WriteUInt8(byte value)
        {
            WriteBytes(new byte[] { value });
        }

        public void WriteInt16(Int16 value)
        {
            byte[] _ = BitConverter.GetBytes(value);
            byte[] b = new byte[2]
            {
                _[1],
                _[0]
            };
            WriteBytes(b);
        }

        public void WriteUInt16(UInt16 value)
        {
            byte[] _ = BitConverter.GetBytes(value);
            byte[] b = new byte[2]
            {
                _[1],
                _[0]
            };
            WriteBytes(b);
        }

        public void WriteInt32(Int32 value)
        {
            byte[] _ = BitConverter.GetBytes(value);
            byte[] b = new byte[4]
            {
                _[3],
                _[2],
                _[1],
                _[0]
            };
            WriteBytes(b);
        }

        public void WriteUInt32(UInt32 value)
        {
            byte[] _ = BitConverter.GetBytes(value);
            byte[] b = new byte[4]
            {
                _[3],
                _[2],
                _[1],
                _[0]
            };
            WriteBytes(b);
        }

        public void WriteInt64(Int64 value)
        {
            byte[] _ = BitConverter.GetBytes(value);
            byte[] b = new byte[8]
            {
                _[7],
                _[6],
                _[5],
                _[4],
                _[3],
                _[2],
                _[1],
                _[0]
            };
            WriteBytes(b);
        }

        public void WriteUInt64(UInt64 value)
        {
            byte[] _ = BitConverter.GetBytes(value);
            byte[] b = new byte[8]
            {
                _[7],
                _[6],
                _[5],
                _[4],
                _[3],
                _[2],
                _[1],
                _[0]
            };
            WriteBytes(b);
        }

        public void WriteFloat(float value)
        {
            byte[] _ = BitConverter.GetBytes(value);
            byte[] b = new byte[4]
            {
                _[3],
                _[2],
                _[1],
                _[0]
            };
            WriteBytes(b);
        }

        public void Flush() => stream.Flush();

        public void Dispose()
        {
            stream.Close();
            stream.Dispose();
        }
    }
}
