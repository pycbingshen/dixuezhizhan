using UnityEngine;
using System.Collections;

public class RankList : MonoBehaviour
{
    private UIPanel panel;
    [HideInInspector][System.NonSerialized]public UILabel[] nameItem=new UILabel[11];
    private UILabel[] levelItem=new UILabel[11];
    private Transform playername,level;
    private UILabel target;
    private UISprite border;
    [HideInInspector][System.NonSerialized]public int[] playerIdToRankID = new int[9];
    [HideInInspector][System.NonSerialized]public int[] num = new int[11];
    [HideInInspector][System.NonSerialized]public int team2num=0;
    private Info info;

    void Awake(){
        panel = gameObject.transform.Find("Header/Panel").GetComponent<UIPanel>();
        border = panel.transform.FindChild("Border").GetComponent<UISprite>();
        
        playername = panel.transform.FindChild("Name");
        level = panel.transform.FindChild("Level");
        target=panel.transform.FindChild("Target").GetComponent<UILabel>();

        info = GameObject.Find("UI Root/Info").GetComponent<Info>();
    }

    public void XStart () {
        if (GeneralData.gameModeNum == 1)
        {
            target.text="目标:"+GeneralData.gameOption+"击杀数";
        }
        if (GeneralData.gameModeNum == 2)
        {
            target.text="目标:击杀所有对手";
        }
        
        if (GeneralData.teamModeNum == 1)
        {
            border.SetDimensions(200, 50 + GeneralData.PlayerNum * 20);
            for (int i=1; i<=10; i++)
            {
                if (i <= GeneralData.PlayerNum)
                {
                    nameItem [i] = playername.FindChild("item" + i).gameObject.GetComponent<UILabel>();
                    levelItem [i] = level.FindChild("item" + i).gameObject.GetComponent<UILabel>();
                    
                    nameItem [i].gameObject.SetActive(true);
                    levelItem [i].gameObject.SetActive(true);
                    
                    nameItem [i].applyGradient = false;
                    levelItem [i].applyGradient = false;
                    
                    nameItem [i].text = GeneralData.PlayerName[i];//"Player" + GameObject.Find("Player").transform.GetChild(i-1).name;
                    playerIdToRankID[i] = i;

                    if (GeneralData.gameModeNum == 1)
                    {
                        num[i] = 0;
                    }
                    if (GeneralData.gameModeNum == 2)
                    {
                        num[i] = GeneralData.gameOption;
                    }
                    levelItem[i].text = num[i].ToString();
                } else
                {
                    playername.FindChild("item" + i).gameObject.SetActive(false);
                    level.FindChild("item" + i).gameObject.SetActive(false);
                }
            }
        }
        
        if (GeneralData.teamModeNum == 2)
        {
            team2num = GeneralData.PlayerNum / 2 + 2;
            border.SetDimensions(200, 90 + GeneralData.PlayerNum * 20);
            int j=0,k=0;
            for (int i=1; i<=10; i++)
            {
                nameItem [i] = playername.FindChild("item" + i).gameObject.GetComponent<UILabel>();
                levelItem [i] = level.FindChild("item" + i).gameObject.GetComponent<UILabel>();

                nameItem [i].gameObject.SetActive(true);
                levelItem [i].gameObject.SetActive(true);

                if (i == 1 || i == team2num)
                {
                    nameItem [i].applyGradient = true;
                    levelItem [i].applyGradient = true;
                    if(i==1)nameItem [i].text ="队伍1";
                    else nameItem [i].text ="队伍2";
                    if (GeneralData.gameModeNum == 1)
                    {
                        num[i] = 0;
                    }
                    if (GeneralData.gameModeNum == 2)
                    {
                        num[i] = GeneralData.gameOption * GeneralData.PlayerNum / 2;
                    }
                    levelItem[i].text = num[i].ToString();
                    continue;
                }
                if(i<=GeneralData.PlayerNum+2){
                    nameItem [i].applyGradient = false;
                    levelItem [i].applyGradient = false;
                    
                    if (GeneralData.gameModeNum == 1)
                    {
                        num[i] = 0;
                    }
                    if (GeneralData.gameModeNum == 2)
                    {
                        num[i] = GeneralData.gameOption;
                    }
                    levelItem[i].text = num[i].ToString();
                    if (i< team2num)
                    {
                        while(GeneralData.TeamId[++j]!=1);
                        nameItem [i].text = GeneralData.PlayerName[j];
                        playerIdToRankID[j] = i;
                    }
                    else{
                        while(GeneralData.TeamId[++k]!=2);
                        nameItem [i].text = GeneralData.PlayerName[k];
                        playerIdToRankID[k] = i;
                    }
                    continue;
                }
                nameItem [i].gameObject.SetActive(false);
                levelItem [i].gameObject.SetActive(false);
            }
        }
        //for (int i = 1; i <= GeneralData.PlayerNum; i++)
        //{
        //    print("----");
        //    print(playerIdToRankID[i]);
        //}
    }
	
    public void Kill(int killId,int dieId){
        info.AddInfo(killId, dieId);
        GeneralData.killnum[killId]++;
        GeneralData.diednum[dieId]++;
        if (GeneralData.gameModeNum == 1)
        {
            if (GeneralData.teamModeNum == 2)
            {
                if (playerIdToRankID[killId] < team2num)
                {
                    levelItem[1].text = (++num[1]).ToString();
                }
                else
                {
                    levelItem[team2num].text = (++num[team2num]).ToString();
                }
            }
            Refresh(playerIdToRankID[killId], 1);
        }
        if (GeneralData.gameModeNum == 2)
        {
            if (GeneralData.teamModeNum == 2)
            {
                if (playerIdToRankID[dieId] < team2num)
                {
                    levelItem[1].text = (--num[1]).ToString();
                }
                else
                {
                    levelItem[team2num].text = (--num[team2num]).ToString();
                }
            }
            Refresh(playerIdToRankID[dieId], -1);
        }
        //for (int i = 1; i <= GeneralData.PlayerNum; i++)
        //{
        //    print("----");
        //    print(playerIdToRankID[i]);
        //}
    }

    public void Refresh(int rankid, int val)
    {
        num[rankid] += val;
        levelItem[rankid].text = num[rankid].ToString();

        if (val > 0)
        {
            for (int i = rankid-1; i >= 1; i--)
            {
                if (i == team2num) return;
                if (num[i+1] > num[i])
                {
                    swap(i, i + 1);
                }
                else return;
            }
        }
        if (val < 0)
        {
            if (rankid == team2num ||rankid==1) return;
            for (int i = rankid + 1; i <= GeneralData.PlayerNum+2; i++)
            {
                if (i == team2num) return;
                if (num[i - 1] < num[i])
                {
                    swap(i, i - 1);
                }
                else return;
            }
        }

    }
    /*
    public void Refresh(string nickname,int nowlevel){
        int rank = -1;
        for(int i=1;i<=GeneralData.PlayerNum;i++){
            if(nameItem[i].text==nickname){
                rank=i;
                break;
            }
        }

        int oldlevel = int.Parse(levelItem [rank].text);
        if (oldlevel == nowlevel)
            return;

        levelItem[rank].text = nowlevel.ToString();
        if (nowlevel > oldlevel)
        {
            for(int i=rank-1;i>=1;i--){
                int level=int.Parse(levelItem [i].text);
                if(nowlevel>level){
                    swap(i,i+1);
                }else return;
            }
        } else
        {
            for(int i=rank+1;i<=GeneralData.PlayerNum;i++){
                int level=int.Parse(levelItem [i].text);
                if(nowlevel<level){
                    swap(i,i-1);
                }else return;
            }
        }
    }*/

    private void swap(int x,int y){
        string tmp;
        tmp=nameItem[x].text;
        nameItem [x].text = nameItem [y].text;
        nameItem [y].text = tmp;

        tmp=levelItem[x].text;
        levelItem [x].text = levelItem [y].text;
        levelItem [y].text = tmp;

        int n;
        n= num[x];
        num[x] = num[y];
        num[y] = n;

        for(int i=1; i<=8; i++)
        {
            if (playerIdToRankID[i] == x)
            {
                playerIdToRankID[i] = y;
                continue;
            }
            if (playerIdToRankID[i] == y)
            {
                playerIdToRankID[i] = x;
                continue;
            }
        }
        //n = playerIdToRankID[x];
        //playerIdToRankID[x] = playerIdToRankID[y];
        //playerIdToRankID[y] = n;
    }
}
