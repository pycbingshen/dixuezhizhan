using UnityEngine;
using System.Collections;

public class CreateRoom : MonoBehaviour {
    private HallPanel hallPanel;
    void Awake () {
        hallPanel=GameObject.Find("UI Root/Panel/HallPanel").GetComponent<HallPanel>();
    }

    void OnClick () {
        hallPanel.CreateRoom();
	}
}
