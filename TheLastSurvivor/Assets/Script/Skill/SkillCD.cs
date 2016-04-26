using UnityEngine;
using System.Collections;

public class SkillCD : MonoBehaviour 
{

    private UISprite _sp;
    private UILabel _lab;
    private UIButton _but;
    private GameObject _endCD;
    
    [HideInInspector][System.NonSerialized] public float CDTime;
    private float CurrentCDTime;
    private bool _isInit;

    void Start()
    {
        _isInit = false;
//        CDTime = 2;
        _sp = GetComponent<UISprite>();
        _lab = transform.FindChild("Label").GetComponent<UILabel>();
        _but = transform.parent.FindChild("Icon").GetComponent<UIButton>();
        _endCD = transform.parent.FindChild("EndCD").gameObject;
    }

	void XInit() 
    {
        _but.isEnabled = false;
        _sp.fillAmount = 1;
        CurrentCDTime = CDTime;
        _lab.text = CDTime.ToString("f1");
	}

	void FixedUpdate () 
    {
        if (!_isInit)
        {
            XInit();
            _isInit = true;
        }
        CurrentCDTime -= 0.02f;
        _sp.fillAmount = CurrentCDTime / CDTime;
        _lab.text = CurrentCDTime.ToString("f1");
        if (CurrentCDTime <= 0)
        {
            _but.isEnabled = true;
            gameObject.SetActive(false);
            _endCD.GetComponent<UIPlayTween>().Play(true);
            _isInit = false;
        }
	}
}
