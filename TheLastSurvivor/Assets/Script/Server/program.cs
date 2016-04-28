using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using client;
using System.Threading;
using proto_islandsurvival;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace client
{
    public class program : MonoBehaviour
    {
        const int PORT = 20000;
        const string IPADDRESS = "10.0.128.147";
        static public RoundRobinQueue RecvQueue = new RoundRobinQueue();
        static public RoundRobinQueue SendQueue = new RoundRobinQueue();
        static Thread recvthread;
        static Thread sendthread;
        static bool isend;
        void Start()
        {
            print("programStart");
            DontDestroyOnLoad(gameObject);
            EditorApplication.playmodeStateChanged += EditorCallBack;
        }

        public static void StartConnect()
        {
            print("programStartConnect");
            RegisterMessage();
            if (TcpSocket.Instance().ConnectToServer(IPADDRESS, PORT) == 0)
            {
                return;
            }
            isend = false;
            recvthread = new Thread(new ThreadStart(RecvThread));
            recvthread.Start();

            sendthread = new Thread(new ThreadStart(SendThread));
            sendthread.Start();
//            Thread processthread;
//            processthread = new Thread(new ThreadStart(ProcessThread));
//            processthread.Start();
            
        }

        public void EditorCallBack()
        {
            if (!EditorApplication.isPlaying && !EditorApplication.isPaused)
            {
                if (TcpSocket.Instance() != null)
                {
                    TcpSocket.Instance().Close();
                }
                isend = true;
            }
        }

        public void OnDestory()
        {
            Debug.Log("Destory Socket");
            if(TcpSocket.Instance() != null) TcpSocket.Instance().Close();
            isend = true;

        }

        static private void ProcessThread()
        {
            for (; ; )
            {
                while (!RecvQueue.empty())
                {
                    CMessage mess = RecvQueue.front();
                    RecvQueue.pop();
                    if (mess.m_proto is SCGameLoading)
                    {
                        Console.WriteLine("...........Loading..............");
                        CMessage cm = new CMessage();
                        CSLoadSucceed proto = new CSLoadSucceed();
                        cm.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSLoadSucceed));
                        cm.m_proto = proto;
                        Thread.Sleep(2000);
					
                        SendQueue.push(cm);
                    }
                    else if (mess.m_proto is SCGameStart)
                    {
                        Console.WriteLine("hahahhahahahaha");
                    }
                    /*pb p1 = (pb)mess.m_proto;
                    pb2 p2 = new pb2();
                    p2.a = p1.b;
                    p2.b = (uint)p1.a;
                    CMessage mess2 = new CMessage(); 
                    mess2.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(pb2));
                    mess2.m_proto = (object)p2;
                    SendQueue.push(mess2);*/
                }
            }
        }

        static private void RecvThread()
        {
            for (; ; )
            {
                if(isend) return;
				int ret = TcpSocket.Instance().RecvData();
                if (ret == -1) return;
                else if (ret == 1)
                {
                    TcpSocket.Instance().SplitMessage();
                }
                Thread.Sleep(5);
            }
        }

        static private void SendThread()
        {
            for (; ; )
            {
                if(isend) return;
                while (!SendQueue.empty())
                {
                    CMessage mess = SendQueue.front();
                    SendQueue.pop();
                    int ret = TcpSocket.Instance().SendMessage(mess);
                    if (ret == 0) return;
                }
                Thread.Sleep(5);
            }
        }

        float lastSend=0;

        void Update()
        {
            //print("Time.time:"+ Time.time+ "\nlastSend:"+ lastSend);
            if (Time.time - lastSend>3)
            {
                lastSend = Time.time;
                CMessage mess = new CMessage();
                mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSHeartbeat));
                CSHeartbeat proto = new CSHeartbeat();
                mess.m_proto = proto;
                program.SendQueue.push(mess);
            }
        }

        static void RegisterMessage()
        {
            MessageRegister.Instance().Register(0x0000, typeof(CSLogin));
            MessageRegister.Instance().Register(0x0001, typeof(SCLogin));
            MessageRegister.Instance().Register(0x0002, typeof(CSRegist));
            MessageRegister.Instance().Register(0x0003, typeof(SCRegist));
            MessageRegister.Instance().Register(0x0004, typeof(PBRoomInfo));
            MessageRegister.Instance().Register(0x0005, typeof(PBDetailRoomInfo));
            MessageRegister.Instance().Register(0x0006, typeof(CSChangeTeam));
            MessageRegister.Instance().Register(0x0007, typeof(CSPullAllRoomInfo));
            MessageRegister.Instance().Register(0x0008, typeof(SCPullAllRoomInfo));
            MessageRegister.Instance().Register(0x0009, typeof(CSCreateRoom));
            MessageRegister.Instance().Register(0x000a, typeof(SCCreateRoom));
            MessageRegister.Instance().Register(0x000b, typeof(CSJoinRoom));
            MessageRegister.Instance().Register(0x000c, typeof(SCJoinRoom));
            MessageRegister.Instance().Register(0x000d, typeof(CSPlayerExitRoom));
            MessageRegister.Instance().Register(0x000e, typeof(SCPlayerExitRoom));
            MessageRegister.Instance().Register(0x000f, typeof(SCPullOneRoomDetail));
            MessageRegister.Instance().Register(0x0010, typeof(CSReadyToStart));
            MessageRegister.Instance().Register(0x0011, typeof(SCGameLoading));
            MessageRegister.Instance().Register(0x0012, typeof(CSLoadSucceed));
            MessageRegister.Instance().Register(0x0013, typeof(SCGameStart));
            MessageRegister.Instance().Register(0x0014, typeof(SCPlayerExitGame));
            MessageRegister.Instance().Register(0x0015, typeof(CSChoseRole));
            MessageRegister.Instance().Register(0x0016, typeof(SCChoseRole));
            MessageRegister.Instance().Register(0x0017, typeof(SCFrameMess));
            MessageRegister.Instance().Register(0x0018, typeof(CSMove));
            MessageRegister.Instance().Register(0x0019, typeof(SCMove));
            MessageRegister.Instance().Register(0x001a, typeof(CSGamePause));
            MessageRegister.Instance().Register(0x001b, typeof(SCGamePause));
            MessageRegister.Instance().Register(0x001c, typeof(CSSkill));
            MessageRegister.Instance().Register(0x001d, typeof(SCSkill));
            MessageRegister.Instance().Register(0x001e, typeof(CSHeartbeat));
            MessageRegister.Instance().Register(0x001f, typeof(CSGameEnd));
            MessageRegister.Instance().Register(0x0020, typeof(SCGameEnd));





        }
    }
}
