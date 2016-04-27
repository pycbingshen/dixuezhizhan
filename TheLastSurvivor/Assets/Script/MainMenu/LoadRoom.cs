using UnityEngine;
using System.Collections;
using client;
using proto_islandsurvival;

public class LoadRoom : MonoBehaviour 
{
    bool flag = false;

    void OnClick()
    {
        if (flag) return;
        flag = true;
        CMessage mess = new CMessage();
        mess.m_head.m_framenum = Controller.CurrentFrameNum;
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSGameEnd));
        CSGameEnd proto = new CSGameEnd();
        mess.m_proto = proto;
        program.SendQueue.push(mess);

        //program.RecvQueue.init();
        //Application.LoadLevel("Room");
    }

    
}
