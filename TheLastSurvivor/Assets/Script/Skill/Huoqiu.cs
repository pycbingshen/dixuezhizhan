using UnityEngine;
using System.Collections;

public class Huoqiu : MonoBehaviour 
{

    [HideInInspector][System.NonSerialized] public GameObject userGo;
    float deltaY = -0.5f;

	
	// Update is called once per frame
	void FixedUpdate () 
    {
        Vector3 pos = transform.position;
        pos.y += deltaY;
        transform.position = pos;
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Terrain")
            return;

        deltaY = 0;
        transform.Find("Benti").GetComponent<ParticleSystem>().Stop();
        transform.Find("Hit").gameObject.SetActive(true);
        GameObject.Find("Skill_Effect").GetComponent<Skill>().RangeOfSkillHurt(userGo, transform.position, 6);
    }
}
