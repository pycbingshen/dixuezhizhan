using UnityEngine;
using System.Collections;

public class Chuanxinjian : MonoBehaviour 
{
    [HideInInspector][System.NonSerialized] public float angle;
	[HideInInspector][System.NonSerialized] public GameObject user;
    private Vector3 _moveDelta;
    
    public void Start()
    {
        _moveDelta = new Vector3(Mathf.Cos(angle) * 0.6f, 0, Mathf.Sin(angle) * 0.6f);
    }

    public void FixedUpdate () 
    {
        transform.localPosition += _moveDelta;
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player" && user != collider.gameObject)
            collider.gameObject.GetComponent<XUnit>().BeAttacked(user, 1);
    }
}
