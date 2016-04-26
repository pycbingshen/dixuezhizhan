using UnityEngine;
using System.Collections;

public static class GameJudgement
{
    static public int KillTargetNum;
    static public int AlivePlayerNum;
    static public int[] FlagGetNum = new int[2];

    static public void GameEnd(bool win)
    {
        GameObject.Find("Controller").GetComponent<PlayerInput>().CanControll = false;
        GameObject root = GameObject.Find("UI Root");
        root.transform.Find("GameEnd").gameObject.SetActive(true);
        if (win)
            root.transform.Find("GameEnd/win").gameObject.SetActive(true);
        else
            root.transform.Find("GameEnd/lose").gameObject.SetActive(true);

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
