using UnityEngine;
using System.Collections;

public class MonsterSpawn : MonoBehaviour 
{
    private Transform MonsterParent;
    private Transform HPParent;
    public GameObject[] MonsterPrefab;
    public GameObject HPPrefab;
    private int _currentMonsterID;

    public void RandomSpawn(int num)
    {
        int tol = 0;
        while (tol < num)
        {
            float x = GeneralData.XRandom(_currentMonsterID,0,10000) / 100f;
            float z = GeneralData.XRandom(_currentMonsterID,0,10000) / 100f;
            Vector3 pos = new Vector3(x,0,z);
            NavMeshHit hit;
            if(NavMesh.SamplePosition(pos, out hit, 1.0f, 1<<NavMesh.GetNavMeshLayerFromName("Default")))
            {
                GameObject newMonster = Instantiate(MonsterPrefab[0]) as GameObject;
                newMonster.name = _currentMonsterID.ToString();
                newMonster.transform.parent = MonsterParent;
                newMonster.transform.localScale = Vector3.one;
                newMonster.transform.position = new Vector3(hit.position.x,0.2f,hit.position.z);
                Debug.Log(newMonster.transform.position);

                newMonster.AddComponent<NavMeshAgent>();
                NavMeshAgent nav = newMonster.GetComponent<NavMeshAgent>();
                nav.radius = 0.7f;
                nav.speed = 3.5f;

                GameObject newHP = Instantiate(HPPrefab) as GameObject;
                newHP.transform.parent = HPParent;
                newHP.transform.localScale = Vector3.one;
                newMonster.GetComponent<Monster>().HP = newHP.GetComponent<UISlider>();

                newHP.GetComponent<HPFollow>().FollowTarget = newMonster;
                newMonster.GetComponent<Monster>().XStart();
                newHP.GetComponent<HPFollow>().XStart();
                _currentMonsterID++;
                tol++;
            }
        }
    }

	// Use this for initialization
	public void XStart () 
    {
        _currentMonsterID = 11;
        HPParent = GameObject.Find("UI Root/HP/MonsterHP").transform;
        MonsterParent = GameObject.Find("Monster").transform;
        //GameObject newMonster = Instantiate(MonsterPrefab[0]) as GameObject;
        RandomSpawn(0);
	}

    public void XFixedUpdate()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<Monster>().XFixedUpdate();
    }
}
