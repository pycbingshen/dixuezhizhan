using UnityEngine;
using System.Collections;

public class NameFollow : MonoBehaviour 
{

    [HideInInspector] public GameObject FollowTarget;
    private Renderer[] mRenderers;
    private float _disDelta = 0.4f;
    private bool _visable;
	// Use this for initialization
	public void XStart () 
    {
        ResumeLevel(1);
        mRenderers = FollowTarget.GetComponentsInChildren<Renderer>();
        _visable = true;
	}
	
	// Update is called once per frame
    public void XUpdate () 
    {
        if (mRenderers [0].enabled != _visable)
        {
            _visable = !_visable;
            if(_visable)
                GetComponent<UISprite>().color = new Color(0,0,0,1);
            else
                GetComponent<UISprite>().color = new Color(0,0,0,0);
        }
        if(_visable)
            transform.position = GeneralData.WorldPosition2UIPosition(FollowTarget.transform.position) + new Vector3(0, _disDelta);
    }

    public void ResumeLevel(int level)
    {
        transform.FindChild("Label").GetComponent<UILabel>().text = "Lv" + level.ToString() + "\n" + GeneralData.PlayerName[int.Parse(FollowTarget.name)];
    }
}
