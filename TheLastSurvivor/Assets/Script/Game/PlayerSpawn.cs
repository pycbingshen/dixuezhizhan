using UnityEngine;
using System.Collections;

public class PlayerSpawn: MonoBehaviour {

    public GameObject[] PlayerGoPrefab;
    public GameObject HPPrefab;
    public GameObject NamePrefab;
	// Use this for initialization
    public void Spawn(int index, int zhiye)
    {
        Vector3 spawnPos;
        while (true)
        {
            spawnPos = GeneralData.SpawnPos[ GeneralData.XRandom(0,0,9) ];
            int i;
            for( i = 0 ; i < transform.childCount ; i++)
                if( Vector3.Distance(transform.GetChild(i).position , spawnPos) < 5)
                    break;
            if( i >= transform.childCount)
                break;
        }

        Transform HPParent = GameObject.Find("UI Root/HP/PlayerHP").transform;
        Transform NameParent = GameObject.Find("UI Root/Name").transform;

        string name = index.ToString();
        GameObject newPlayer = Instantiate(PlayerGoPrefab[zhiye]) as GameObject;

        if (GeneralData.teamModeNum == 2)
            newPlayer.GetComponent<Hero>().TeamID = GeneralData.TeamId [index];
        else
            newPlayer.GetComponent<Hero>().TeamID = index;

        if (zhiye == 3)
        {
            newPlayer.GetComponent<Hero>().isRangeHero = true;
            newPlayer.GetComponent<Hero>()._attackRange = 15f;
        }
        newPlayer.name = name;
        newPlayer.transform.parent = transform;
        newPlayer.transform.localScale = Vector3.one;
        newPlayer.transform.localPosition = spawnPos;
        //Debug.Log("++++++++++++++" + spawnPos);

//        newPlayer.AddComponent<NavMeshAgent>();
//        NavMeshAgent nav = newPlayer.GetComponent<NavMeshAgent>();
//        nav.radius = 0.7f;
//        nav.speed = 8;
//        nav.acceleration = 1000;
//        nav.angularSpeed = 360;
//        nav.autoRepath = false;
        
        GameObject newHP = Instantiate(HPPrefab) as GameObject;
        if (GeneralData.TeamId [index] != GeneralData.TeamId [GeneralData.myID])
            newHP.transform.GetChild(0).GetComponent<UISprite>().color = new Color(1, 1, 0, 1);
        newHP.name = name;
        newHP.transform.parent = HPParent;
        newHP.transform.localScale = Vector3.one;
        newHP.GetComponent<HPFollow>().FollowTarget = newPlayer;
        newPlayer.GetComponent<Hero>().HP = newHP.GetComponent<UISlider>();

        GameObject newName = Instantiate(NamePrefab) as GameObject;
        newName.name = name;
        newName.transform.parent = NameParent;
        newName.transform.localScale = Vector3.one;
        newName.GetComponent<NameFollow>().FollowTarget = newPlayer;
        newPlayer.GetComponent<Hero>().NameLabel = newName;

        newPlayer.GetComponent<Hero>().XStart();
        newHP.GetComponent<HPFollow>().XStart();
        newName.GetComponent<NameFollow>().XStart();
        
        if (index == GeneralData.myID)
        {

            CameraFollow cf = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
            cf.MyHero = newPlayer;

            GameObject sight = newPlayer.transform.FindChild("Sight").gameObject;
            sight.AddComponent<FOWRevealer>();
            sight.GetComponent<FOWRevealer>().range = new Vector2(0.1f, 20);
            sight.GetComponent<FOWRevealer>().lineOfSightCheck = FOWSystem.LOSChecks.EveryUpdate;
            sight.GetComponent<FOWRevealer>().isActive = true;

            if (zhiye == 3)
                GeneralData.SkillNum = 4;
            else
                GeneralData.SkillNum = 5;
            GeneralData.charaID = zhiye;
            GeneralData.LoadSkillList();
            GeneralData.choosed = true;

            GameObject.Find("UI Root/Skill_List").GetComponent<SkillList>().LoadData();
            GameObject.Find("Controller").GetComponent<PlayerInput>().CanControll = true;

        }
        else
            if (GeneralData.teamModeNum == 2 && GeneralData.TeamId [index] == GeneralData.TeamId [GeneralData.myID])
            {
                GameObject sight = newPlayer.transform.FindChild("Sight").gameObject;
                sight.AddComponent<FOWRevealer>();
                sight.GetComponent<FOWRevealer>().range = new Vector2(0.1f, 20);
                sight.GetComponent<FOWRevealer>().lineOfSightCheck = FOWSystem.LOSChecks.EveryUpdate;
                sight.GetComponent<FOWRevealer>().isActive = true;
            }
            else
            {
                GameObject sight = newPlayer.transform.FindChild("Sight").gameObject;
                sight.AddComponent<FOWRenderers>();
            }

        GameObject.Find("UI Root/Skill_List").GetComponent<SkillList>().GameObjectGet(index, newPlayer);
    }

    public void Resume(int index, int spawnIndex)
    {
        Vector3 Pos = GeneralData.SpawnPos[ spawnIndex ];

        GameObject go = GameObject.Find("Player/" + index.ToString());
        go.transform.position = Pos;
        Hero hero = go.GetComponent<Hero>();
        hero.animator.SetBool("Death", false);
        hero.HP.gameObject.SetActive(true);
        hero._isDead = false;
        hero.HP.value = 1;
        hero._currHP = hero._maxHP;
        hero.ResumeFeibiaoNumber(2);
    }

	public void XStart () 
    {
//        for (int i = 0; i < GeneralData.PlayerNum; i++)
//        {
//            Spawn(i, 0);
//        }
	}

    public void XFixedUpdate()
    {
        for (int i = 0; i < transform.childCount; i ++)
            transform.GetChild(i).GetComponent<Hero>().XFixedUpdate();
    }
}
