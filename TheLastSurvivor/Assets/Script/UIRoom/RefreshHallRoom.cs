using UnityEngine;
using System.Collections;

public class RefreshHallRoom : MonoBehaviour {
    private HallPanel hallPanel;
    void Awake () {
        hallPanel=GameObject.Find("UI Root/Panel/HallPanel").GetComponent<HallPanel>();
    }
    
    void OnClick () {
        hallPanel.RefreshHallRoom();
    }
}
