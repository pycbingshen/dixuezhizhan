using UnityEngine;
using System.Collections;

public class HPFollow : MonoBehaviour 
{
    [HideInInspector] public GameObject FollowTarget;
    public GameObject HurtNumPrefab;
    private Renderer[] mRenderers;
    private float _disDelta;
    private bool _visable;

    public void XStart()
    {
        CharacterController cc = FollowTarget.GetComponent<CharacterController>();
        _disDelta = (cc.radius * 2 + cc.height - 0.3f) / 13;
        mRenderers = FollowTarget.GetComponentsInChildren<Renderer>();
        _visable = true;
    }

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

    public void AppearHurtNum(float hurtNum)
    {
        GameObject newLabel = Instantiate(HurtNumPrefab) as GameObject;
        newLabel.GetComponent<HurtNum>().XStart();
        newLabel.transform.parent = GameObject.Find("UI Root/HP/HurtLabel").transform;
        newLabel.transform.localScale = Vector3.one;
        newLabel.GetComponent<UILabel>().text = "-" + hurtNum.ToString();
        TweenPosition tPos = newLabel.GetComponent<TweenPosition>();
        tPos.from = transform.localPosition + new Vector3(0, 30);
        tPos.to = transform.localPosition + new Vector3(0, 60);
    }
}
