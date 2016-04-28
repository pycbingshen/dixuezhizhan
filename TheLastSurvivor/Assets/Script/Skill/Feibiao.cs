using UnityEngine;
using System.Collections;

public class Feibiao : MonoBehaviour 
{
    [HideInInspector][System.NonSerialized] public float angle;
    [HideInInspector][System.NonSerialized] public GameObject user;
    private Vector3 _moveDelta;
    private Transform _model;

    public void Start()
    {
        _model = transform.FindChild("Model");
        _moveDelta = new Vector3(Mathf.Cos(angle) * 0.4f, 0, Mathf.Sin(angle) * 0.4f);
    }
	
	// Update is called once per frame
    public void FixedUpdate () 
    {
        _model.Rotate(0, 0, 15);
        transform.localPosition += _moveDelta;
	}

//    void OnControllerColliderHit(ControllerColliderHit collider)
//    {
//        Debug.Log("aaaaaaaaaaaaaaaaaattack!!!");
//        collider.gameObject.GetComponent<XUnit>().BeAttacked(gameObject, 1);
//    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && user != collider.gameObject)
        {
            Debug.Log("飞镖\n");
            collider.gameObject.GetComponent<XUnit>().BeAttacked(user, 1);
        }
    }
}
