using UnityEngine;
using System.Collections;

public class XGame : MonoBehaviour 
{
    Controller XController;
    PlayerSpawn XPlayerSpawn;
//    MonsterSpawn XMonsterSpawn;
    CameraFollow XCameraFollow;
    HPManage XHPManage;
    NameManage XNameManage;
    RankList XRankList;
    PlayerInput XPlayerInput;
    MIndicator XMIndicator;
    SkillList XSkillList;
    Reward XReward;

    private bool _isStart = false;

    void GetScript()
    {
        XController = GameObject.Find("Controller").GetComponent<Controller>();
        XPlayerSpawn = GameObject.Find("Player").GetComponent<PlayerSpawn>();
//        XMonsterSpawn = GameObject.Find("Monster").GetComponent<MonsterSpawn>();
        XCameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        XHPManage = GameObject.Find("UI Root/HP").GetComponent<HPManage>();
        XNameManage = GameObject.Find("UI Root/Name").GetComponent<NameManage>();
        XRankList = GameObject.Find("UI Root/RankList").GetComponent<RankList>();
        XPlayerInput = GameObject.Find("Controller").GetComponent<PlayerInput>();
        XMIndicator = GameObject.Find("Indicator").GetComponent<MIndicator>();
        XSkillList = GameObject.Find("UI Root/Skill_List").GetComponent<SkillList>();
        XReward = GameObject.Find("Reward").GetComponent<Reward>();
    }

	// Use this for initialization
	void Start () 
    {
//        GameObject tmp = GameObject.Find("Player").GetComponent<PlayerSpawn>().PlayerGoPrefab [0];
//        GameObject newPlayer = Instantiate(tmp) as GameObject;
//        newPlayer.transform.localPosition = new Vector3(90, 0, 90);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (GameObject.Find("Controller").GetComponent<Controller>().IsGamePause)
        {
            return;
        }

        if (!_isStart)
        {
            _isStart = true;

            GeneralData.XStart();
            GetScript();

            XController.XStart();
            XPlayerSpawn.XStart();
            XPlayerInput.XStart();
            XMIndicator.XStart();
//            XMonsterSpawn.XStart();
            XCameraFollow.XStart();
            XHPManage.XStart();
            XRankList.XStart();
            XSkillList.XStart();
            XReward.XStart();
        }

        XPlayerInput.XUpdate();
        XMIndicator.XUpdate();
        XController.XUpdate();
        XCameraFollow.XUpdate();
        XHPManage.XUpdate();
        XNameManage.XUpdate();
	}

    void FixedUpdate()
    {
        if (GameObject.Find("Controller").GetComponent<Controller>().IsGamePause)
        {
            return;
        }

        if (_isStart)
        {

            XController.XFixedUpdate();
            XPlayerSpawn.XFixedUpdate();
            XSkillList.XFixedUpdate();
//            XMonsterSpawn.XFixedUpdate();
            XHPManage.XFixedUpdate();
            XReward.XFixedUpdate();
        }
    }
}
