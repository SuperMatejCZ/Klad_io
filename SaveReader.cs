using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io
{
    public class SaveReader : IDisposable
    {
        private Stream stream;

        public long Position { get => stream.Position; set => stream.Position = value; }

        public long Length => stream.Length;

        public long BytesLeft => stream.Length - Position;

        public SaveReader(byte[] _bytes)
        {
            stream = new MemoryStream(_bytes);
            if (!stream.CanRead)
                throw new Exception("Can't read from stream");
            Position = 0;
        }

        public SaveReader(Stream _stream)
        {
            stream = _stream;
            if (!stream.CanRead)
                throw new Exception("Can't read from stream");
            Position = 0;
        }

        public SaveReader(string _path)
        {
            if (!File.Exists(_path))
                throw new FileNotFoundException($"File \"{_path}\" doesn't exist", _path);

            stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            if (!stream.CanRead)
                throw new Exception("Can't read from stream");
            Position = 0;
        }

        public void Reset() => stream.Position = 0;

        public byte[] ReadBytes(int count)
        {
            if (BytesLeft < count)
                throw new Exception("Reached end of stream");

            byte[] bytes = new byte[count];
            stream.Read(bytes, 0, count);
            return bytes;
        }

        public byte[] ReadByteArray()
        {
            return ReadBytes(ReadInt32());
        }

        public bool ReadBool()
        {
            return ReadUInt8() != 0;
        }

        public sbyte ReadInt8()
        {
            return (sbyte)ReadBytes(1)[0];
        }

        public byte ReadUInt8()
        {
            return ReadBytes(1)[0];
        }

        public Int16 ReadInt16()
        {
            return BitConverter.ToInt16(ReadBytes(2), 0);
        }

        public UInt16 ReadUInt16()
        {
            return BitConverter.ToUInt16(ReadBytes(2), 0);
        }

        public Int32 ReadInt32()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }

        public UInt32 ReadUInt32()
        {
            return BitConverter.ToUInt32(ReadBytes(4), 0);
        }

        public Int64 ReadInt64()
        {
            return BitConverter.ToInt64(ReadBytes(8), 0);
        }

        public UInt64 ReadUInt64()
        {
            return BitConverter.ToUInt64(ReadBytes(8), 0);
        }

        public string ReadShortString()
        {
            return Encoding.UTF8.GetString(ReadBytes(ReadUInt16()));
        }

        public string ReadString()
        {
            return Encoding.UTF8.GetString(ReadBytes(ReadInt32()));
        }

        public void Dispose()
        {
            stream.Close();
            stream.Dispose();
        }
    }
}
