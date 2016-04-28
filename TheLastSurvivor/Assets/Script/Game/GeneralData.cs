using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;

public static class GeneralData
{
    static public int myID;
    static public int PlayerNum;
    static public int charaID;
    static public int[] randSeed = new int[500];
    static public Vector3[] SpawnPos = new Vector3[16];
    static public int[] skill = new int[5];
    static public float[] skillCD = new float[5];
    static public bool choosed;
    static public int SkillNum;
    static public int[] AlivePlayerNum = new int[3];
    static public int[,] SkillLastUseFrame = new int[10,12];

    static public string myName = null;
    static public int myTeamId;
    static public string[] PlayerName =new string[9];
    //static public int[] playerId =new int[8];
    static public int[] TeamId =new int[9];
    static public int gameModeNum,teamModeNum;
    static public int gameOption;
    static public int roomId;
    
    static public int[] killnum = new int[9];
    static public int[] diednum = new int[9];

    static public void LoadSkillList()
    {
        
        if (Resources.Load ("xml/Character"))
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Resources.Load ("xml/Character").ToString());
            XmlNodeList xmlNodeList = xml.SelectSingleNode("character").ChildNodes;
            foreach (XmlElement xl1 in xmlNodeList)
            {
                if (xl1.GetAttribute("id") == charaID.ToString())
                {
                    skill[0] = int.Parse( xl1.GetAttribute("skill1_id") );
                    skill[1] = int.Parse( xl1.GetAttribute("skill2_id") );
                    skill[2] = int.Parse( xl1.GetAttribute("skill3_id") );
                    skill[3] = int.Parse( xl1.GetAttribute("skill4_id") );

                    if(SkillNum == 5)
                        skill[4] = int.Parse( xl1.GetAttribute("skill5_id") );

                    return ;
                }
            }
        }
    }

    static public void XStart()
    {
        choosed = false;
        for (int i = 1; i <= 8; i++)
            for (int j = 0; j <= 11; j++)
                SkillLastUseFrame [i,j] = -50;
        SpawnPos [0] = new Vector3(18, 1f, 72);
        SpawnPos [1] = new Vector3(18, 1f, 48);
        SpawnPos [2] = new Vector3(42, 1f, 72);
        SpawnPos [3] = new Vector3(42, 1f, 48);
        SpawnPos [4] = new Vector3(5, 3f, 30);
        SpawnPos [5] = new Vector3(55, 3f, 30);
        SpawnPos [6] = new Vector3(5, 3f, 90);
        SpawnPos [7] = new Vector3(55, 3f, 90);
        SpawnPos [8] = new Vector3(15, 5f, 115);
        SpawnPos [9] = new Vector3(45, 5f, 115);
        SpawnPos [10] = new Vector3(37, 5f, 102);
        SpawnPos [11] = new Vector3(23, 5f, 102);
        SpawnPos [12] = new Vector3(15, 5f, 5);
        SpawnPos [13] = new Vector3(45, 5f, 5);
        SpawnPos [14] = new Vector3(37, 5f, 18);
        SpawnPos [15] = new Vector3(23, 5f, 18);

        for(int i=0; i <= 8; i++)
        {
            killnum[i] = 0;
            diednum[i] = 0;
        }
    }

    static public Vector3 WorldPosition2UIPosition(Vector3 pos)
    {
        Camera mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector3 pos2 = mainCamera.WorldToScreenPoint(pos);
        pos2.z = 0f;
        Camera uiCamera = GameObject.Find("UI Root").transform.FindChild("Camera").GetComponent<Camera>();
        return uiCamera.ScreenToWorldPoint(pos2);
    }

    static public int XRandom(int index,int min, int max)
    {
        return min + XGetRandom(index) % (max - min);
    }

    static public int XGetRandom(int index)
    {
        randSeed[index] = (randSeed[index] * 23257 + 23812) % 10007;
        return randSeed[index];
    }

    static public float GetSkillCDTime(SkillType skillType)
    {
        Transform ts = GameObject.Find("UI Root/Skill_List").transform;
        System.Type type = typeof(SkillType);
        for (int i = 0; i < ts.childCount; i++)
            if(skillType == (SkillType)Enum.Parse(type, ts.GetChild(i).name))
                return skillCD[i];
        Debug.LogError("Error!!! Can not find skill CD time!");
        return 0;
    }
}
