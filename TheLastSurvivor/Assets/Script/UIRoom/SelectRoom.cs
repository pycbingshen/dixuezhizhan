using UnityEngine;
using System.Collections;

public class SelectRoom : MonoBehaviour {
    //private Transform room;
    private HallPanel hallPanel;

    void Awake () {
        hallPanel = GameObject.Find("UI Root/Panel/HallPanel").GetComponent<HallPanel>();
        //room = gameObject.transform.parent.Find("SelectRoom");
    }
    
    void OnClick () {
        string roomName = gameObject.transform.Find("RoomName").GetComponent<UILabel>().text;
        string x=roomName.Substring(2);
        GeneralData.roomId =int.Parse(x);
        string gameMode = gameObject.transform.Find("GameMode").GetComponent<UILabel>().text;
        GeneralData.gameModeNum = MyTool.GameModeToNum(gameMode);
        string teamMode = gameObject.transform.Find("TeamMode").GetComponent<UILabel>().text;
        GeneralData.teamModeNum = MyTool.TeamModeToNum(teamMode);
        print("选中房间"+x);
        hallPanel.SelectRoom(int.Parse(gameObject.name.Substring(4)));
    }
}
