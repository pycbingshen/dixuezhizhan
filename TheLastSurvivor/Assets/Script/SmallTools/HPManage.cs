using UnityEngine;
using System.Collections;

public class HPManage : MonoBehaviour 
{
    GameObject PlayerHP;
    GameObject MonsterHP;
    GameObject HurtLabel;
	// Use this for initialization
	public void XStart () 
    {
        PlayerHP = transform.FindChild("PlayerHP").gameObject;
        MonsterHP = transform.FindChild("MonsterHP").gameObject;
        HurtLabel = transform.FindChild("HurtLabel").gameObject;
	}
	
	// Update is called once per frame
	public void XUpdate () 
    {
        for (int i = 0; i < PlayerHP.transform.childCount; i++)
            PlayerHP.transform.GetChild(i).GetComponent<HPFollow>().XUpdate();
        for (int i = 0; i < MonsterHP.transform.childCount; i++)
            MonsterHP.transform.GetChild(i).GetComponent<HPFollow>().XUpdate();
	}

    public void XFixedUpdate()
    {
        for (int i = 0; i < HurtLabel.transform.childCount; i++)
            HurtLabel.transform.GetChild(i).GetComponent<HurtNum>().XFixedUpdate();
    }
}
