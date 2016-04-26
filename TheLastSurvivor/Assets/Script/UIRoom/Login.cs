using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {
    private LoginPanel loginPanel;
    void Awake () {
        loginPanel=GameObject.Find("UI Root/Panel/LoginPanel").GetComponent<LoginPanel>();
    }
    
    void OnClick () {
        loginPanel.Login();
    }
}
