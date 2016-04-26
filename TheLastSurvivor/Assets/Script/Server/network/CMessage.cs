using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;
using client;

namespace client
{	
    public class CMessageHead
    {
        public int m_packetlen;
        public int m_message_id;
        public int m_framenum;
        public int m_message_hash;

        public int size()
        {
            return 4*sizeof(int);
        }

		private void encode_Append(ref byte[] Outs, object x)
        {
			int len = Marshal.SizeOf(x);
			if(len == 4)
            {
				int tmp = IPAddress.HostToNetworkOrder((int)x);
				byte[] b = BitConverter.GetBytes(tmp);
				Outs = Outs.Concat(b).ToArray();
            }
        }

        public void encode(ref byte[] Outs)
        {
            encode_Append(ref Outs, m_packetlen);
            encode_Append(ref Outs, m_message_id);
            encode_Append(ref Outs, m_framenum);
            encode_Append(ref Outs, m_message_hash);
        }

        private void decode_Append(byte[] Outs, int offset, out int x)
        {
            int tmp = BitConverter.ToInt32(Outs, offset);
            x =  IPAddress.NetworkToHostOrder(tmp);
        }

        public int decode(byte[] Ins, int offset, int InLen)
        {
            if (InLen != size())
            {
                return 0;
            }
            decode_Append(Ins, offset, out m_packetlen);
            offset += 4;
            decode_Append(Ins, offset, out m_message_id);
            offset += 4;
            decode_Append(Ins, offset, out m_framenum);
            offset += 4;
            decode_Append(Ins, offset, out m_message_hash);
            return 1;
        }

    }

    public enum enum_encode
    {
		ProtoIsNULL = 0,
		successful = 1
    }

	public enum enum_decode
    {
		NotEnoughLonger = 0,
		TypeError = 1,
		successful = 2
    };

    public class CMessage
    {
        public CMessageHead m_head = new CMessageHead();
        public object m_proto;

		public int encode(ref byte[] Outs)
        {
			if(m_proto == null) return (int)enum_encode.ProtoIsNULL;
            MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, m_proto);
            m_head.m_packetlen = (int)ms.Length + m_head.size();
            m_head.encode(ref Outs);
            byte[] tmp = ms.ToArray();
            Outs = Outs.Concat(tmp).ToArray();
			return (int)enum_encode.successful;
        }

		public int decode(byte[] Ins, int offset, int InLen, out int GetLen)
		{
            if (InLen < m_head.size())
            {
                GetLen = 0;
                return (int)enum_decode.NotEnoughLonger;
            }
            m_head.decode(Ins, offset, m_head.size());
			offset += m_head.size();
            if (m_head.m_packetlen > InLen)
            {
				GetLen = 0;
                return (int)enum_decode.NotEnoughLonger;
            }
            GetLen = m_head.m_packetlen;
            Type type = MessageRegister.Instance().GetType(m_head.m_message_id);
            if (type == null)
            { 
                return (int)enum_decode.TypeError;
            }
            MemoryStream ms = new MemoryStream();
            ms.Write(Ins, offset, m_head.m_packetlen - m_head.size());
            ms.Seek(0, SeekOrigin.Begin);
            m_proto = Serializer.NonGeneric.Deserialize(type, ms);
            return (int)enum_decode.successful;
		}
    }
}
