using UnityEngine;
using System.Collections;

public class SelectRole : MonoBehaviour {
    private SelectRolePanel panel;
    void Awake()
    {
        panel = GameObject.Find("UI Root/SelectRole").GetComponent<SelectRolePanel>();
    }

    void OnClick()
    {
        if (gameObject.name == "Common") {
            panel.ChangeRole(-1);
            return;
        }
        int id = int.Parse(gameObject.name.Substring(4));
        panel.ChangeRole(id);
    }
}
