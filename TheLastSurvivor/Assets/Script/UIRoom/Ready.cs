using UnityEngine;
using System.Collections;

public class Ready : MonoBehaviour {
    private RoomPanel roomPanel;
    void Start () {
        roomPanel=GameObject.Find("UI Root/Panel/RoomPanel").GetComponent<RoomPanel>();
    }
    
    void OnClick () {
        roomPanel.StartGame();
    }
}
