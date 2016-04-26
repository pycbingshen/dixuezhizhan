using UnityEngine;
using System.Collections;

public class Joystick : MonoBehaviour 
{
    [HideInInspector][System.NonSerialized] public bool IsActive;
	// Use this for initialization
	void Start () 
    {
        IsActive = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (!IsActive)
            return;
        transform.localPosition = Vector3.ClampMagnitude( transform.localPosition, 70);
	}

    void OnPress(bool isDown)
    {
        IsActive = isDown;
        if(!isDown)
            transform.localPosition = Vector3.zero;
    }
}
