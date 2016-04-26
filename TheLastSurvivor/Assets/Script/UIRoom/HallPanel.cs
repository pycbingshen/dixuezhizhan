using UnityEngine;
using System.Collections;
using client;
using proto_islandsurvival;

public class HallPanel : MonoBehaviour {
    //private UILabel lNickName=null;
    private RoomPanel roomPanel=null;
    private UIPanel block;
    private Transform roomList, selectRoom;
    //[HideInInspector][System.NonSerialized]public int roomId=0;
    private UILabel gameMode,teamMode;
    private float refreshTime=-1;
    private float refreshDelay=3f;
    private Tip tip;
    void Awake(){
        //lNickName=gameObject.transform.Find("InputNickName/Label").GetComponent<UILabel>();
        //lNickName=GameObject.Find("UI Root/Panel/HallPanel/InputNickName/Label").GetComponent<UILabel>();
        roomPanel=gameObject.transform.parent.Find("RoomPanel").GetComponent<RoomPanel>();
        //roomPanel=GameObject.Find("UI Root/Panel/RoomPanel").GetComponent<RoomPanel>();
        block = GameObject.Find("UI Root").transform.Find("Block").GetComponent<UIPanel>();
        roomList = gameObject.transform.Find("RoomList/ButtonList");
        gameMode = gameObject.transform.Find("Button/GameMode/Content").GetComponent<UILabel>();
        teamMode = gameObject.transform.Find("Button/TeamMode/Content").GetComponent<UILabel>();
        tip = GameObject.Find("UI Root").transform.Find("Panel/Tip").GetComponent<Tip>();

        selectRoom = roomList.transform.Find("SelectRoom");
    }

    void Start () {
        //gameObject.SetActive(false);
    }

    public void CreateRoom() {
//        string nickname=lNickName.text;
//        if (nickname == null || nickname == "请输入昵称")
//        {
//            print("未输入昵称");
//            return;
//        }
        //roomId = 0;

        CMessage mess = new CMessage();
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSCreateRoom));
        CSCreateRoom proto = new CSCreateRoom();
        GeneralData.gameModeNum=MyTool.GameModeToNum(gameMode.text);
        GeneralData.teamModeNum=MyTool.TeamModeToNum(teamMode.text);
        proto.game_mode = GeneralData.gameModeNum;
        proto.team_mode = GeneralData.teamModeNum;
        print(proto.game_mode);
        print(proto.team_mode);
        mess.m_proto = proto;
        program.SendQueue.push(mess);
        block.gameObject.SetActive(true);
    }

    public void JoinRoom() {
//        string nickname=lNickName.text;
        /*
        roomPanel.id = 1;
        roomPanel.test = roomPanel.test + "x";
        roomPanel.OpenRoom();
        //roomPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);*/

//        if (nickname == null || nickname == "请输入昵称")
//        {
//            print("未输入昵称");
//            return;
//        }
        if (GeneralData.roomId == 0)
        {
            print("未选择房间");
            tip.Show("未选择房间");
            return;
        }

        CMessage mess = new CMessage();
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSJoinRoom));
        CSJoinRoom proto = new CSJoinRoom();
        proto.room_id = GeneralData.roomId;
        mess.m_proto = proto;
        program.SendQueue.push(mess);
        block.gameObject.SetActive(true);
    }

    public void SelectRoom(int id)
    {
        selectRoom.gameObject.SetActive(true);
        selectRoom.localPosition = new Vector3(0, 230 - 60 * id, 0);
    }

    public void RefreshHallRoom() {
        //print("刷新房间");

        CMessage mess = new CMessage();
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSPullAllRoomInfo));
        CSPullAllRoomInfo proto = new CSPullAllRoomInfo();
        mess.m_proto = proto;
        program.SendQueue.push(mess);
        //block.gameObject.SetActive(true);
    }

    public void OpenHall(){
        gameObject.SetActive(true);
        GeneralData.roomId = 0;
        //if(selectRoom==null) selectRoom = gameObject.transform.Find("RoomList/ButtonList/SelectRoom");
        //print(selectRoom);
        //selectRoom = gameObject.transform.Find("RoomList/ButtonList/SelectRoom");
        //selectRoom.gameObject.SetActive(false);
        RefreshHallRoom();
    }

    private void OpenRoom(SCPullOneRoomDetail roomDetail){
        roomPanel.OpenRoom(roomDetail);
        //roomPanel.RefreshRoom(joinroom);
        gameObject.SetActive(false);
    }

    void Update(){
        refreshTime -= Time.deltaTime;
        if (refreshTime < 0)
        {
            RefreshHallRoom();
            refreshTime=refreshDelay;
        }

        if (program.RecvQueue.empty())
            return;
        bool handle = false;
        CMessage mess = program.RecvQueue.front();
        if (mess.m_proto is SCPullAllRoomInfo)
        {   
            SCPullAllRoomInfo gameMess = (SCPullAllRoomInfo)mess.m_proto;
            selectRoom.gameObject.SetActive(false);
            for (int i=0;i<3;i++){
                Transform room=roomList.Find("Room"+(i+1));
                if(i<gameMess.rooms.Count){
                    room.Find("RoomName").GetComponent<UILabel>().text="房间"+gameMess.rooms[i].id;
                    room.Find("PlayerNum").GetComponent<UILabel>().text=gameMess.rooms[i].have+"/"+gameMess.rooms[i].limit;
                    room.Find("GameMode").GetComponent<UILabel>().text=MyTool.NumToGameMode(gameMess.rooms[i].game_mode);
                    room.Find("TeamMode").GetComponent<UILabel>().text=MyTool.NumToTeamMode(gameMess.rooms[i].team_mode);
                    room.gameObject.SetActive(true);
                    if(gameMess.rooms[i].id==GeneralData.roomId)SelectRoom(i+1);
                }
                else{
                    room.gameObject.SetActive(false);
                }
            }
            handle = true;
            //print("刷新大厅成功");
        }

        if (mess.m_proto is SCCreateRoom)
        {   
            SCCreateRoom gameMess = (SCCreateRoom)mess.m_proto;
            if(gameMess.result){
                print("创建成功");
                //OpenRoom();
                GeneralData.roomId=gameMess.room_id;
            }else{
                print("创建失败");
                tip.Show("创建失败");
                RefreshHallRoom();
            }
            handle = true;
        }

        if (mess.m_proto is SCJoinRoom)
        {   
            SCJoinRoom gameMess = (SCJoinRoom)mess.m_proto;
            if(gameMess.result==3){
                print("加入成功");
            }else{
                RefreshHallRoom();
                if(gameMess.result==0){
                    print("房间不存在");
                    tip.Show("房间不存在");
                }
                if(gameMess.result==1){
                    print("游戏已经开始");
                    tip.Show("游戏已经开始");
                }
                if(gameMess.result==2){
                    print("房间人数已满");
                    tip.Show("房间人数已满");
                }
            }
            handle = true;
        }

        if (mess.m_proto is SCPullOneRoomDetail)
        {   
            SCPullOneRoomDetail gameMess = (SCPullOneRoomDetail)mess.m_proto;
            print("进入房间1");
            OpenRoom(gameMess);
            print("进入房间2");
            handle = true;
        }
        if (handle == false) print("无法处理协议(丢弃)：" + mess.m_proto);

        program.RecvQueue.pop();
        block.gameObject.SetActive(false);
    }
}
