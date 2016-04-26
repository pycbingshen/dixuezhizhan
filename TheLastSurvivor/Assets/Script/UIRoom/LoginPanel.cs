using UnityEngine;
using System.Collections;
using client;
using proto_islandsurvival;

public class LoginPanel : MonoBehaviour {
    UILabel userName;
    UIInput password;
    UIPanel block;
    HallPanel hallPanel;
    Tip tip;
    void Awake(){
        userName=gameObject.transform.Find("Label/UserName/Input/Label").GetComponent<UILabel>();
        password=gameObject.transform.Find("Label/Password/Input").GetComponent<UIInput>();
        hallPanel=gameObject.transform.parent.Find("HallPanel").GetComponent<HallPanel>();
        block = GameObject.Find("UI Root").transform.Find("Block").GetComponent<UIPanel>();
        tip = GameObject.Find("UI Root").transform.Find("Panel/Tip").GetComponent<Tip>();
    }

    void Start(){
        block.gameObject.SetActive(false);
        if (GeneralData.myName == null)
        {
            program.StartConnect();
            Debug.Log("connect");
        }
        else {
            hallPanel.OpenHall();
            gameObject.SetActive(false);
        }
    }

    public void Login(){
        string sUserName = userName.text;
        string sPassword = password.value;
        if (sUserName == null || sUserName == "")
        {
            tip.Show("请填写用户名");
            return;
        }
        if (sPassword == null || sPassword == "")
        {
            tip.Show("请填写密码");
            return;
        }
        print(sUserName);
        print(sPassword);
        CMessage mess = new CMessage();
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSLogin));
        CSLogin proto = new CSLogin();
        proto.id = sUserName;
        proto.passwd = sPassword;
        mess.m_proto = proto;
        program.SendQueue.push(mess);
        block.gameObject.SetActive(true);
    }

    public void Registered(){
        string sUserName = userName.text;
        string sPassword = password.value;
        if (sUserName == null || sUserName == "")
        {
            tip.Show("请填写用户名");
            return;
        }
        if (sPassword == null || sPassword == "")
        {
            tip.Show("请填写密码");
            return;
        }
        CMessage mess = new CMessage();
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSRegist));
        CSRegist proto = new CSRegist();
        proto.id = sUserName;
        proto.passwd = sPassword;
        mess.m_proto = proto;
        program.SendQueue.push(mess);
        block.gameObject.SetActive(true);
    }

    void OpenHall(){
        GeneralData.myName=userName.text;
        hallPanel.OpenHall();
        gameObject.SetActive(false);
    }

    void Update(){
        if (program.RecvQueue.empty())
            return;
        bool handle = false;
        CMessage mess = program.RecvQueue.front();
        if (mess.m_proto is SCLogin)
        {
            SCLogin gameMess = (SCLogin)mess.m_proto;
            print(gameMess.result);
            if(gameMess.result==2){
                print("登录成功");
                OpenHall();
            }else{
                if(gameMess.result==0){
                    print("用户名或密码错误");
                    tip.Show("用户名或密码错误");
                }
                if(gameMess.result==1){
                    print("数据库错误");
                    tip.Show("数据库错误");
                }
                if (gameMess.result == 3)
                {
                    print("账户已登录");
                    tip.Show("账户已登录");
                }
            }
            handle = true;
        }

        if (mess.m_proto is SCRegist)
        {
            SCRegist gameMess = (SCRegist)mess.m_proto;
            if(gameMess.result==2){
                print("注册成功");
                tip.Show("注册成功");
                OpenHall();
            }else{
                if(gameMess.result==0){
                    print("用户名被占用");
                    tip.Show("用户名被占用");
                }
                if(gameMess.result==1){
                    print("数据库错误");
                    tip.Show("数据库错误");
                }
            }
            handle = true;
        }

        if(handle==false) print("无法处理协议(丢弃)："+mess.m_proto);

        program.RecvQueue.pop();
        block.gameObject.SetActive(false);
    }
}
