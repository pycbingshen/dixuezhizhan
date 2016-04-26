using UnityEngine;
using System.Collections;

public class HurtNum : MonoBehaviour 
{
    float lifeTime = 2f;
    float spawnTime;
	// Use this for initialization
	public void XStart () 
    {
        spawnTime = Time.time;
	}
	
	// Update is called once per frame
	public void XFixedUpdate () 
    {
	    if (Time.time - spawnTime > lifeTime)
            Destroy(gameObject);
	}
}
