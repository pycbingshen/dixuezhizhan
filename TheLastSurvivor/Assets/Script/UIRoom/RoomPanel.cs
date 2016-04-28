using UnityEngine;
using System.Collections;
using client;
using proto_islandsurvival;

public class RoomPanel : MonoBehaviour {
    [HideInInspector][System.NonSerialized] public int id=0;
    private HallPanel hallPanel=null;
    private UILabel gameMode,teamMode,winCondition, winContent, RoomId;
    private UIPanel block;
    private Transform playerList,button,win,winSymbol,team,startgame;
    private int gameOption=10,teamId=1;
    private Tip tip;

    void Awake(){
        hallPanel=gameObject.transform.parent.Find("HallPanel").GetComponent<HallPanel>();
        button=gameObject.transform.Find("Button");
        startgame = button.transform.Find("StartGame"); 
        gameMode=button.transform.Find("GameMode/Content").GetComponent<UILabel>();
        teamMode=button.transform.Find("TeamMode/Content").GetComponent<UILabel>();
        win=button.transform.Find("Win");
        winCondition=win.transform.Find("Condition").GetComponent<UILabel>();
        winContent = win.transform.Find("Content").GetComponent<UILabel>();
        winSymbol = win.Find("Symbol");
        team = button.transform.Find("TeamID");
        playerList=gameObject.transform.Find("PlayerList/ButtonList");
        block = GameObject.Find("UI Root").transform.Find("Block").GetComponent<UIPanel>();
        RoomId=button.transform.Find("RoomId/Content").GetComponent<UILabel>();
        tip = GameObject.Find("UI Root").transform.Find("Panel/Tip").GetComponent<Tip>();
    }

    void Start(){
        //gameObject.SetActive(false);
    }

    public void ExitRoom(){
        print("退出房间");
        hallPanel.OpenHall();
        gameObject.SetActive(false);

        CMessage mess = new CMessage();
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSPlayerExitRoom));
        CSPlayerExitRoom proto = new CSPlayerExitRoom();
        mess.m_proto = proto;
        program.SendQueue.push(mess);
    }

    public void OpenRoom(SCPullOneRoomDetail joinroom){
        gameObject.SetActive(true);
        gameMode.text = MyTool.NumToGameMode(GeneralData.gameModeNum);
        teamMode.text = MyTool.NumToTeamMode(GeneralData.teamModeNum);
        RoomId.text = GeneralData.roomId.ToString();
        winCondition.text=MyTool.GameModeNumToWinCondition(GeneralData.gameModeNum);
        team.GetComponent<UIPopupList>().value = "1";
        print(gameMode.text);
        print(teamMode.text);
        teamId = 1;
        if (MyTool.NumToTeamMode(GeneralData.teamModeNum) == "组队")
        {
            team.gameObject.SetActive(true);
        } else
        {
            team.gameObject.SetActive(false);
        }
        RefreshRoom(joinroom);
    }

    public void StartGame(){
        if (GeneralData.teamModeNum == 2)
        {
            int sum = 0;
            for(int i = 1; i <= GeneralData.PlayerNum; i++)
            {
                if (GeneralData.TeamId[i] == 1) sum++;
            }
            if (GeneralData.PlayerNum != sum * 2)
            {
                print("队伍人数不匹配");
                tip.Show("队伍人数不匹配");
                return;
            }
        }
        print("开始游戏");
        CMessage mess = new CMessage();
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSReadyToStart));
        CSReadyToStart proto = new CSReadyToStart();
        mess.m_proto = proto;
        program.SendQueue.push(mess);
        block.gameObject.SetActive(true);
    }

    private void RefreshRoom(SCPullOneRoomDetail gameMess){
        for(int i=0;i<8;i++){
            Transform player=playerList.Find("Player"+(i+1));
            GeneralData.PlayerNum = gameMess.room_info.players.Count;
            GeneralData.gameOption=gameMess.room_info.game_option;
            if (i<gameMess.room_info.players.Count){
                player.Find("UserName").GetComponent<UILabel>().text=gameMess.room_info.players[i];
                int teamId=gameMess.room_info.team_id[i];
                player.Find("TeamId").GetComponent<UILabel>().text=teamId.ToString();
                player.gameObject.SetActive(true);
                GeneralData.PlayerName[i+1]=gameMess.room_info.players[i];
                //GeneralData.playerId[i]=gameMess.room_info.player_id[i];
                GeneralData.TeamId[i+1]=gameMess.room_info.team_id[i];
                if(GeneralData.myName==GeneralData.PlayerName[i+1]){
                    GeneralData.myID=gameMess.room_info.player_id[i];
                    GeneralData.myTeamId = GeneralData.TeamId[i + 1];
                }
            }
            else{
                player.gameObject.SetActive(false);
            }
        }

        if (GeneralData.myName == gameMess.room_info.players [0])//是否是房主
        {
            winSymbol.gameObject.SetActive(true);
            win.GetComponent<UIButton>().enabled=true;
            win.GetComponent<BoxCollider>().enabled=true;
            win.GetComponent<UISprite>().enabled=true;
            startgame.gameObject.SetActive(true);
            Color color = new Color(0, 0, 0);
            winContent.color = color;
        } else
        {
            winSymbol.gameObject.SetActive(false);
            win.GetComponent<UIButton>().enabled=false;
            win.GetComponent<BoxCollider>().enabled=false;
            win.GetComponent<UISprite>().enabled=false;
            startgame.gameObject.SetActive(false);
            Color color = new Color(192/256.0f, 99/256.0f, 0);
            winContent.color = color;
        }
        win.GetComponent<UIPopupList>().value = gameMess.room_info.game_option.ToString();
        gameOption= gameMess.room_info.game_option;
    }

    private void Change(){
        int value = int.Parse(win.GetComponent<UIPopupList>().value);
        if (gameOption != value)
        {
            gameOption = value;
            CMessage mess = new CMessage();
            mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSChangeTeam));
            CSChangeTeam proto = new CSChangeTeam();
            proto.game_option=gameOption;
            mess.m_proto = proto;
            program.SendQueue.push(mess);
        }
        value = int.Parse(team.GetComponent<UIPopupList>().value);
        if (teamId != value)
        {
            teamId = value;
            CMessage mess = new CMessage();
            mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSChangeTeam));
            CSChangeTeam proto = new CSChangeTeam();
            proto.team_id=teamId;
            mess.m_proto = proto;
            program.SendQueue.push(mess);
        }
    }

    void Update(){
       // print(teamMode.text);
        Change();
        if (program.RecvQueue.empty())
            return;
        bool handle = false;
        CMessage mess = program.RecvQueue.front();
        if (mess.m_proto is SCPullOneRoomDetail)
        {
            SCPullOneRoomDetail gameMess = (SCPullOneRoomDetail)mess.m_proto;
            RefreshRoom(gameMess);
            print("刷新房间详细状态");
            handle = true;
        }
        if (mess.m_proto is SCGameLoading)
        {
            SCGameLoading gameMess = (SCGameLoading)mess.m_proto;
//            GeneralData.PlayerNum = gameMess.player_id.Count;
//            for(int i=0;i<GeneralData.PlayerNum;i++){
//                GeneralData.PlayerName[i]=gameMess.players[gameMess.player_id[i]];
//                if(GeneralData.myName==GeneralData.PlayerName[i]){
//                    GeneralData.myID=i;
//                }
//            }
            GeneralData.randSeed[0] = gameMess.rand;
            for(int i = 0 ; i < 80 ; i++)
                GeneralData.randSeed[i] = GeneralData.XGetRandom(0);
            Debug.Log("random seed = " + GeneralData.randSeed);
            if(GeneralData.teamModeNum == 1)
                GeneralData.AlivePlayerNum[1] = GeneralData.PlayerNum;
            else
            {
                GeneralData.AlivePlayerNum[1] = GeneralData.PlayerNum / 2;
                GeneralData.AlivePlayerNum[2] = GeneralData.PlayerNum / 2;
            }
            Application.LoadLevel("Load");
            handle = true;
        }

        if(mess.m_proto is SCPullAllRoomInfo) handle = true;
        if (handle == false) print("无法处理协议(丢弃)：" + mess.m_proto);

        program.RecvQueue.pop();
        block.gameObject.SetActive(false);
    }
}
