using System;
using MemoryStream=System.IO.MemoryStream;
using BinaryReader=System.IO.BinaryReader;
using BinaryWriter=System.IO.BinaryWriter;
using Stack=System.Collections.Stack;
using Stream=System.IO.Stream;

namespace TESFaith {
    /// <summary>
    /// A class to emulate c++'s type casting of string data
    /// </summary>
    class ByteStream {
        private MemoryStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private int Offset;
        private Stack OffsetStack=new Stack();
        public ByteStream(byte[] data) {
            stream=new MemoryStream(data);
            reader=new BinaryReader(stream);
            writer=new BinaryWriter(stream);
        }
        public ByteStream(Stream s,int count) {
            if(s.Length-s.Position<count) throw new ArgumentException("End of stream reached");
            byte[] bytes=new byte[count];
            s.Read(bytes,0,count);
            stream=new MemoryStream(bytes);
            reader=new BinaryReader(stream);
            writer=new BinaryWriter(stream);
        }

        public void Close() {
            writer.Close();
            reader.Close();
            stream.Close();
        }

        public int Size { get { return (int)(stream.Length-Offset); } }
        public void AddOffset(int i) {
            OffsetStack.Push(Offset);
            Offset+=i;
        }
        public void RemoveOffset() {
            Offset=(int)OffsetStack.Pop();
        }

        public byte GetByte(int index) {
            stream.Position=index+Offset;
            return reader.ReadByte();
        }
        /*public short GetShort(int index) {
            stream.Position=index+Offset;
            return reader.ReadInt16();
        }*/
        public int GetInt(int index) {
            stream.Position=index+Offset;
            return reader.ReadInt32();
        }
        /*public long GetLong(int index) {
            stream.Position=index+Offset;
            return reader.ReadInt64();
        }*/
        /*public ushort GetUShort(int index) {
            stream.Position=index+Offset;
            return reader.ReadUInt16();
        }*/
        /*public uint GetUInt(int index) {
            stream.Position=index+Offset;
            return reader.ReadUInt32();
        }*/
        /*public ulong GetULong(int index) {
            stream.Position=index+Offset;
            return reader.ReadUInt64();
        }*/
        public float GetFloat(int index) {
            stream.Position=index+Offset;
            return reader.ReadSingle();
        }
        public string GetString(int index) {
            //Do this one manually, because .NET has some funny ideas about how strings should be encoded
            stream.Position=index+Offset;
            string s="";
            byte b;
            while((b=reader.ReadByte())!=0) s+=(char)b;
            return s;
        }
        public string GetString(int index,int len) {
            if(index+Offset+len>stream.Length) return "";
            stream.Position=index+Offset;
            string s="";
            for(int i=0;i<len;i++) s+=(char)reader.ReadByte();
            return s;
        }
        public byte[] GetBytes(int index,int length) {
            byte[] b=new byte[length];
            stream.Position=index+Offset;
            stream.Read(b,0,length);
            return b;
        }

        public void WriteByte(int index,byte b) {
            stream.Position=index+Offset;
            writer.Write(b);
        }
        /*public void WriteShort(int index,short b) {
            stream.Position=index+Offset;
            writer.Write(b);
        }*/
        public void WriteInt(int index,int b) {
            stream.Position=index+Offset;
            writer.Write(b);
        }
        /*public void WriteLong(int index,long b) {
            stream.Position=index+Offset;
            writer.Write(b);
        }*/
        /*public void WriteUShort(int index,ushort b) {
            stream.Position=index+Offset;
            writer.Write(b);
        }*/
        /*public void WriteUInt(int index,uint b) {
            stream.Position=index+Offset;
            writer.Write(b);
        }*/
        /*public void WriteULong(int index,ulong b) {
            stream.Position=index+Offset;
            writer.Write(b);
        }*/
        public void WriteFloat(int index,float b) {
            stream.Position=index+Offset;
            writer.Write(b);
        }
        public void WriteString(int index,string s) {
            stream.Position=index+Offset;
            for(int i=0;i<s.Length;i++) writer.Write((byte)s[i]);
        }
        public void WriteBytes(int index,byte[] bytes) {
            stream.Position=index;
            stream.Write(bytes,0,bytes.Length);
        }

        public override string ToString() {
            return GetString(0,Size);
        }
    }
}
