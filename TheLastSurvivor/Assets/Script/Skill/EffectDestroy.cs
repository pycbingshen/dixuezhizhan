using UnityEngine;
using System.Collections;

public class EffectDestroy : MonoBehaviour 
{
    public float DestroyTime;
    private float _spawnTime;
	// Use this for initialization
	void Start () 
    {
        _spawnTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
        if (Time.time - _spawnTime > DestroyTime)
            Destroy(gameObject);
    }
}
