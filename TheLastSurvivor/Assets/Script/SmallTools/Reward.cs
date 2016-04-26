using UnityEngine;
using System.Collections;

public class Reward : MonoBehaviour 
{
    public GameObject jiangliPrefab;
    [HideInInspector][System.NonSerialized]public int LastDelFrame;
    [HideInInspector][System.NonSerialized]public bool Need2Spawn = true;

	public void XStart () 
    {
        LastDelFrame = Controller.CurrentFrameNum;
	}
	
	// Update is called once per frame
	public void XFixedUpdate () 
    {
        if (Need2Spawn && Controller.CurrentFrameNum - LastDelFrame > 1500)
        {
            Need2Spawn = false;
            GameObject newJiangli = Instantiate(jiangliPrefab) as GameObject;
            newJiangli.transform.parent = transform;
            newJiangli.transform.localPosition = Vector3.zero;

            Info m_Info = GameObject.Find("UI Root/Info").GetComponent<Info>();
            m_Info.AddInfo("在地图中心刷新了一枚手里剑，拾取之后可补充手里剑使用次数，去抢夺吧~");
        }
	}
}
