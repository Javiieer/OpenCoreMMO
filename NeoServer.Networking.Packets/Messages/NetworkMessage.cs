﻿namespace NeoServer.Networking.Packets
{
    using NeoServer.Networking.Packets.Messages;
    using NeoServer.Server.Contracts.Network;
    using NeoServer.Server.Security;
    using System;
    using System.Linq;
    using System.Text;

    public class NetworkMessage : ReadOnlyNetworkMessage
    {
        private int Position;

        public NetworkMessage()
        {
            Buffer = new byte[1024];
        }

        public void AddString(string value)
        {
            AddUInt16((ushort)value.Length);
            WriteBytes(Encoding.UTF8.GetBytes(value));
        }

        public void AddUInt32(uint value) => WriteBytes(BitConverter.GetBytes(value));
        public void AddUInt16(ushort value) => WriteBytes(BitConverter.GetBytes(value));

        public void AddUInt8(sbyte value) => WriteBytes(BitConverter.GetBytes(value));
        public void AddByte(byte b) => WriteBytes(new[] { b });

        /// <summary>
        /// Add bytes with payload length
        /// </summary>
        /// <param name="bytes"></param>
        public void AddBytes(byte[] bytes)
        {
            AddUInt16((ushort)bytes.Length);

            foreach (var b in bytes)
            {
                WriteByte(b);
            }
        }
        public void AddPaddingBytes(int count) => WriteBytes(0x33, count);

       
        private byte[] GetLengthBytes() => BitConverter.GetBytes((ushort)Length);

        private void AddLengthToHeader()
        {
            var length = GetLengthBytes();

            for (int i = 0; i < length.Length; i++)
            {
                WriteByte(length[i], i);
            }
        }



        private void WriteBytes(byte[] bytes)
        {
            foreach (var b in bytes)
            {
                WriteByte(b);
            }
        }
        private void WriteBytes(byte b, int times)
        {
            for (int i = 0; i < times; i++)
            {
                WriteByte(b);
            }
        }

        private void WriteByte(byte b, int position)
        {
            Length++;
            Buffer[position] = b;
        }
        private void WriteByte(byte b)
        {
            Length++;
            Buffer[Position++] = b;
        }


    }
}