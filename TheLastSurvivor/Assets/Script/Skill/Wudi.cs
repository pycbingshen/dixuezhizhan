using UnityEngine;
using System.Collections;

public class Wudi : MonoBehaviour
{
    [HideInInspector][System.NonSerialized] public GameObject _followTargetGo;

	// Use this for initialization
	
	// Update is called once per frame
	public void FixedUpdate () 
    {
        if(null != _followTargetGo)
            transform.position = _followTargetGo.transform.position;
	}
}
