using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klad_io.Server
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
            byte[] _ = ReadBytes(2);
            byte[] b = new byte[2]
            {
                _[1],
                _[0],
            };
            return BitConverter.ToInt16(b, 0);
        }

        public UInt16 ReadUInt16()
        {
            byte[] _ = ReadBytes(2);
            byte[] b = new byte[2]
            {
                _[1],
                _[0],
            };
            return BitConverter.ToUInt16(b, 0);
        }

        public Int32 ReadInt32()
        {
            byte[] _ = ReadBytes(4);
            byte[] b = new byte[4]
            {
                _[3],
                _[2],
                _[1],
                _[0],
            };
            return BitConverter.ToInt32(b, 0);
        }

        public UInt32 ReadUInt32()
        {
            byte[] _ = ReadBytes(4);
            byte[] b = new byte[4]
            {
                _[3],
                _[2],
                _[1],
                _[0],
            };
            return BitConverter.ToUInt32(b, 0);
        }

        public Int64 ReadInt64()
        {
            byte[] _ = ReadBytes(8);
            byte[] b = new byte[8]
            {
                _[7],
                _[6],
                _[5],
                _[4],
                _[3],
                _[2],
                _[1],
                _[0],
            };
            return BitConverter.ToInt64(b, 0);
        }

        public UInt64 ReadUInt64()
        {
            byte[] _ = ReadBytes(8);
            byte[] b = new byte[8]
            {
                _[7],
                _[6],
                _[5],
                _[4],
                _[3],
                _[2],
                _[1],
                _[0],
            };
            return BitConverter.ToUInt64(b, 0);
        }

        public float ReadFloat()
        {
            byte[] _ = ReadBytes(4);
            byte[] b = new byte[4]
            {
                _[3],
                _[2],
                _[1],
                _[0],
            };
            return BitConverter.ToSingle(b, 0);
        }

        public void Dispose()
        {
            stream.Close();
            stream.Dispose();
        }
    }
}
