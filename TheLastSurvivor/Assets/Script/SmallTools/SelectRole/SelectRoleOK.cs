using UnityEngine;
using System.Collections;

public class SelectRoleOK : MonoBehaviour {
    private SelectRolePanel panel;
    void Awake()
    {
        panel = GameObject.Find("UI Root/SelectRole").GetComponent<SelectRolePanel>();
    }

    void OnClick()
    {
        panel.SelectRoleOK();
    }
}
