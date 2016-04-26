using UnityEngine;
using System.Collections;

public class Test2 : MonoBehaviour 
{
    float spawnTime;

	void Start () 
    {
        spawnTime = Time.time;
        StartCoroutine(lalala());
	}
	

	void Update () 
    {
//        if (Input.GetKeyDown(KeyCode.K))
//            Time.timeScale = 0;
//        if (Input.GetKeyUp(KeyCode.K))
//            Time.timeScale = 1;
	}

    void FixedUpdate()
    {
        if (Time.time - spawnTime > 3)
        {
            Destroy(gameObject);
        }
//        transform.position =  Vector3.MoveTowards(transform.position, transform.position + Vector3.right, Time.deltaTime);
    }

    IEnumerator lalala()
    {
        while (true)
        {
            Debug.Log("..." + Time.time);
            yield return new WaitForSeconds(0.1f);
        }
    }

}
