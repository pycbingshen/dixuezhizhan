using UnityEngine;
using System.Collections;
using client;
using proto_islandsurvival;

public class LoadRoom : MonoBehaviour 
{
    void OnClick()
    {
        program.RecvQueue.init();


        CMessage mess = new CMessage();
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSGameEnd));
        CSGameEnd proto = new CSGameEnd();
        mess.m_proto = proto;
        program.SendQueue.push(mess);



        Application.LoadLevel("Room");
    }
}
