using UnityEngine;
using System.Collections;

public static class GameJudgement
{
    static public int KillTargetNum;
    static public int AlivePlayerNum;
    static public int[] FlagGetNum = new int[2];

    static public void GameEnd(bool win)
    {
        RankList ranklist =GameObject.Find("UI Root/RankList").GetComponent<RankList>();
        GameObject.Find("Controller").GetComponent<PlayerInput>().CanControll = false;
        GameObject root = GameObject.Find("UI Root");
        root.transform.Find("GameEnd").gameObject.SetActive(true);
        if (win)
            root.transform.Find("GameEnd/win").gameObject.SetActive(true);
        else
            root.transform.Find("GameEnd/lose").gameObject.SetActive(true);

        int num = GeneralData.PlayerNum;
        if (GeneralData.teamModeNum == 2) num+=2;
        for (int i = 1; i <= 10; i++)
        {
            Transform item=root.transform.Find("GameEnd/List/item" + i);
            if(i<= num)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
                continue;
            }
            UILabel nick = item.Find("nickname").GetComponent<UILabel>();
            nick.text = ranklist.nameItem[i].text;
            if (ranklist.nameItem[i].text == "队伍1" || ranklist.nameItem[i].text == "队伍2")
            {
                nick.color = new Color(1,1,0);
                item.Find("killnum").GetComponent<UILabel>().text = "";
                item.Find("diednum").GetComponent<UILabel>().text = "";
                item.Find("score").GetComponent<UILabel>().text = "";
                continue;
            }
            int kill = -1, die = -1;
            float score =-1;
            for(int j=1; j <= 8; j++)
            {
                if (ranklist.playerIdToRankID[j] == i)
                {
                    kill = GeneralData.killnum[j];
                    die = GeneralData.diednum[j];
                    //Debug.Log("玩家"+j+" 击杀"+kill+" 死亡"+die);
                    int val = die;
                    if (die > 40) val = 40;
                    score = kill * 1.5f *(1- val*0.02f);
                    score=(int)(score * 10) / 10;
                }
            }
            
            item.Find("killnum").GetComponent<UILabel>().text = kill.ToString();
            item.Find("diednum").GetComponent<UILabel>().text = die.ToString();
            item.Find("score").GetComponent<UILabel>().text = score.ToString();
        }

        Debug.Log("Game End!!!Game End!!!Game End!!!Game End!!!Game End!!!Game End!!!");
    }

    static public void DealWith()
    {
        RankList list = GameObject.Find("UI Root/RankList").GetComponent<RankList>();

        if (GeneralData.gameModeNum == 1)
        {
            if(GeneralData.teamModeNum == 1)
            {
                int i;
                for(i = 1 ; i <= GeneralData.PlayerNum ; i++)
                {
                    if(list.num[list.playerIdToRankID[i]] >= GeneralData.gameOption)
                    {
                        if(i == GeneralData.myID)
                            GameEnd(true);
                        else
                            GameEnd(false);
                        return;
                    }
                }
            }

            if(GeneralData.teamModeNum == 2)
            {
                if(list.num[1] >= GeneralData.gameOption || list.num[list.team2num] >= GeneralData.gameOption)
                {
                    if(list.num[1] >= GeneralData.gameOption)
                    {
                        if(GeneralData.myTeamId == 1)
                            GameEnd(true);
                        else
                            GameEnd(false);
                    }
                    else
                    {
                        if(GeneralData.myTeamId == 2)
                            GameEnd(true);
                        else
                            GameEnd(false);
                    }
                }
            }
        }

        if(GeneralData.gameModeNum == 2)
        {
            if(GeneralData.teamModeNum == 1)
            {
                if(list.num[2] <= 0)
                {
                    int i;
                    for(i = 1; i <= GeneralData.PlayerNum ; i++)
                    {
                        if(list.playerIdToRankID[i] == 1)
                            break;
                    }
                    if(i == GeneralData.myTeamId)
                        GameEnd(true);
                    else
                        GameEnd(false);
                }
            }

            if(GeneralData.teamModeNum == 2)
            {
                if(list.num[1] == 0 || list.num[list.team2num]==0)
                {
                    if(list.num[1]!= 0)
                    {
                        if(GeneralData.myTeamId == 1)
                            GameEnd(true);
                        else
                            GameEnd(false);
                    }
                    else
                    {
                        if(GeneralData.myTeamId == 2)
                            GameEnd(true);
                        else
                            GameEnd(false);
                    }
                }
            }
        }
    }
}
