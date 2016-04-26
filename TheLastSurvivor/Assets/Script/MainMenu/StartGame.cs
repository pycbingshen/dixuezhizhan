using UnityEngine;
using System.Collections;
using proto_islandsurvival;
using client;

public class StartGame : MonoBehaviour 
{
    private bool _isConnnecting = false;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (_isConnnecting)
        {
            /*
            if(!program.RecvQueue.empty())
            {
                CMessage mess = program.RecvQueue.front();
                program.RecvQueue.pop();
                if (mess.m_proto is SCGameLoading)
                {
                    SCGameLoading gameMess = (SCGameLoading)mess.m_proto;
                    GeneralData.myID = gameMess.player_id;
                    GeneralData.PlayerNum = gameMess.player_cnt;
                    GeneralData.randSeed[0] = gameMess.rand;
                    for(int i = 0 ; i < 80 ; i++)
                        GeneralData.randSeed[i] = GeneralData.XGetRandom(0);
                    Debug.Log("random seed = " + GeneralData.randSeed);
                    Application.LoadLevel("Fighting");
                }
            }*/
        }
	}

    void OnClick()
    {
        if (!_isConnnecting)
        {
            program.StartConnect();
            Debug.Log("connecting...");
            _isConnnecting = true;
        }
    }
}
