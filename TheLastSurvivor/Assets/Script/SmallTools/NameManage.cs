using UnityEngine;
using System.Collections;

public class NameManage : MonoBehaviour 
{
    public void XStart () 
    {

    }
    
    // Update is called once per frame
    public void XUpdate () 
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<NameFollow>().XUpdate();
    }
}
