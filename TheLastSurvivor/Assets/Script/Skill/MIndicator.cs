using UnityEngine;
using System.Collections;

public class MIndicator : MonoBehaviour 
{
    public GameObject Arrow;
    public GameObject Magic;

    [HideInInspector][System.NonSerialized] public GameObject DragIcon;
    [HideInInspector][System.NonSerialized] public bool ArrowActive;
    [HideInInspector][System.NonSerialized] public bool MagicActive;
    private GameObject _myHero;

	// Use this for initialization
	public void XStart () 
    {
        ArrowActive = false;
        MagicActive = false;
	}
	
	// Update is called once per frame
	public void XUpdate () 
    {
	    if (ArrowActive)
        {
            float angle = CalculateAngle(DragIcon.transform.localPosition);
            Arrow.transform.rotation = Quaternion.Euler(90, -angle + 90, 0);
            _myHero = GameObject.Find("Player/" + GeneralData.myID.ToString());
            Arrow.transform.position = _myHero.transform.position + new Vector3(0, 2.1f, 0);
        }

        if (MagicActive)
        {
            _myHero = GameObject.Find("Player/" + GeneralData.myID.ToString());
            Vector3 pos = new Vector3(DragIcon.transform.localPosition.x * 0.3f, 0, DragIcon.transform.localPosition.y * 0.3f);
            pos = Vector3.ClampMagnitude(pos, 12);
            pos.y += 2.1f;
            Magic.transform.position = _myHero.transform.position + pos;
        }
	}

    public void SetState(GameObject icon, GameObject indGo,out bool indState, bool state)
    {
        DragIcon = icon;
        indGo.SetActive(state);
        indState = state;
    }

    public float CalculateAngle(Vector3 vec)
    {
        float res;
        if (Mathf.Abs(vec.x) > 0.1f)
        {
            //计算角度
            res = Mathf.Atan(vec.y / vec.x);
            if (vec.x < 0f)
                res += Mathf.PI;
            res = res * Mathf.Rad2Deg;
            while (res > 360f)
                res -= 360f;
            while (res < 0)
                res += 360f;
            return res;
        }
        else
        {
            if(vec.y > 0)
                return 90f;
            else
                return 270f;
        }
    }
}
