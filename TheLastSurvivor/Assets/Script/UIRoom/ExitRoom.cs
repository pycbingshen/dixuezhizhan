using UnityEngine;
using System.Collections;

public class ExitRoom : MonoBehaviour {
    private RoomPanel roomPanel;
    void Awake () {
        roomPanel=GameObject.Find("UI Root/Panel/RoomPanel").GetComponent<RoomPanel>();
    }
    
    void OnClick () {
        roomPanel.ExitRoom();
    }
}
