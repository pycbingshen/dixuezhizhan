using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using client;
using UnityEngine;

namespace client
{
    public class TcpSocket
    {
		private TcpSocket()
        {
			m_recv_head = m_recv_tail = 0;
        }

		private static TcpSocket Singleton = null;
		public static TcpSocket Instance()
        {
			if(Singleton == null)
			{
				Singleton = new TcpSocket();
            }
            return Singleton;
        }

        const int BufferSize = 200000;
        private int m_recv_head;
        private int m_recv_tail;

        private byte[] m_recv_buffer = new byte[BufferSize];

        private Socket clientSocket;
        
        public int ConnectToServer(string IPaddress, int port)
        {
            IPAddress ip = IPAddress.Parse (IPaddress);
            clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect (new IPEndPoint (ip, port)); //配置服务器IP与端口
                Console.WriteLine("连接服务器成功");
                return 1;
            }
            catch
            {
                Console.WriteLine("连接服务器失败！");
                return 0;
            }
        }

		public int RecvData()
        {
            if (clientSocket == null) return 0;
            try
            {
                m_recv_tail += clientSocket.Receive(m_recv_buffer, m_recv_tail, BufferSize - m_recv_tail, SocketFlags.None);
            }
            catch
            {
                clientSocket.Close();
                return -1;
            }
            return 1;
        }

        public void SplitMessage()
        {
			while(true)
            {
				CMessage mess = new CMessage();
				int Len;
                int ret = mess.decode(m_recv_buffer, m_recv_head, m_recv_tail - m_recv_head, out Len);
                if (ret == (int)enum_decode.NotEnoughLonger) break;
                else
                {
                    m_recv_head += Len;
                    if (ret == (int)enum_decode.TypeError)
                    {
                        //TODO
                        //Type error
                    }
                    else if (ret == (int)enum_decode.successful)
                    {
                        program.RecvQueue.push(mess);
                    }
                }
            }
			if(m_recv_head == m_recv_tail)
            {
				m_recv_head = m_recv_tail = 0;
                return;
            }
            if (m_recv_head == 0) return;

            for (int i = m_recv_head; i < m_recv_tail; i++)
                m_recv_buffer[i - m_recv_head] = m_recv_buffer[i];
            m_recv_tail = m_recv_tail - m_recv_head;
            m_recv_head = 0;
        }

        public int SendMessage(CMessage mess)
        {
            byte[] by = new byte[0];
            mess.encode(ref by);
            try
            {
                clientSocket.Send(by);
            }
            catch
            {
                clientSocket.Close();
                return 0;
            }
            return 1;
        }

        public void Close()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
        }
    }
}
