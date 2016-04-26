using UnityEngine;
using System.Collections;

public class SmallMapDraw : MonoBehaviour 
{
    private float _nowColor = 1.0f;
    private float _colorUpdate = 0.02f;
    private SpriteRenderer _spr;
	public void XStart () 
    {
        _spr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	public void XFixedUpdate () 
    {
	    if (_nowColor > 0.3f && _nowColor < 1.0f)
            _nowColor += _colorUpdate;
        else
        {
            _colorUpdate = -_colorUpdate;
            _nowColor += _colorUpdate;
        }

        _spr.color = new Color(1, 1, 1, _nowColor);
	}
}
