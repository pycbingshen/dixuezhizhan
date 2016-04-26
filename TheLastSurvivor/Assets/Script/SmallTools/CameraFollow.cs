using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
    [HideInInspector] public Vector3 followDistance;
    [HideInInspector] public GameObject MyHero;

    public void XStart()
    {
        followDistance = new Vector3(0, 20, -10);
    }

	public void XUpdate () 
	{
        if(MyHero != null)
            transform.position = MyHero.transform.position + followDistance; 
	}
}
