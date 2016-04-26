using UnityEngine;
using System.Collections;

public class Info : MonoBehaviour {
    string[] qInfo = new string[100];
    int head = 0, rear = 0, size = 0;
    UILabel[] label = new UILabel[3];
    float lastTime = 1;
    //float nowTime=-1;
    float[] lTime = new float[3];
    int[] se = new int[3];

    float showTime = 5;
    float delayTime = 1.5f;

    float[] killTime = new float[9];
    int[] killNum = new int[9];
    float conKillDelayTime = 5;
    int[] conKillNum = new int[9];

    public AudioClip[] KillSe;

    void Awake()
    {
        label[0] = gameObject.transform.Find("Label3").GetComponent<UILabel>();
        label[1] = gameObject.transform.Find("Label2").GetComponent<UILabel>();
        label[2] = gameObject.transform.Find("Label1").GetComponent<UILabel>();

        label[0].text = "";
        label[1].text = "";
        label[2].text = "";

        for (int i = 0; i < 9; i++)
        {
            killTime[i] = -100;
            killNum[i] = 0;
            conKillNum[i] = 0;
        }
    }

    public void AddInfo(string info)
    {
        qInfo[rearPlus()] = info;
    }

    public void AddInfo(int killId, int dieId)
    {
        //string info = "玩家" + killId + " 击杀 玩家" + dieId;
        string info = GeneralData.PlayerName[killId] + " 击杀了 " + GeneralData.PlayerName[dieId];

        killNum[killId]++;
        killNum[dieId] = 0;

        conKillNum[dieId] = 0;
        if (Time.time - killTime[killId] < conKillDelayTime)
        {
            conKillNum[killId]++;
        }
        else
        {
            conKillNum[killId] = 0;
        }
        killTime[killId] = Time.time;

        if (conKillNum[killId] == 0) qInfo[rearPlus()] = info;
        if (conKillNum[killId] == 1) qInfo[rearPlus()] = info + " ，完成双杀";
        if (conKillNum[killId] == 2) qInfo[rearPlus()] = info + " ，完成三杀";
        if (conKillNum[killId] == 3) qInfo[rearPlus()] = info + " ，完成四杀";
        if (conKillNum[killId] == 4) qInfo[rearPlus()] = info + " ，完成五杀";

        if (killNum[killId] == 6) qInfo[rearPlus()] = killId + " 已经无人能挡！";
        if (killNum[killId] == 7) qInfo[rearPlus()] = killId + " 已经如同神一般！";
        if (killNum[killId] == 8) qInfo[rearPlus()] = killId + " 已经超神了！";
    }

    int rearPlus(){
        int tmp = rear;
        rear++;
        size++;
        if (rear >= 100) rear = 0;
        return tmp;
    }

    int headPlus()
    {
        int tmp = head;
        head++;
        size--;
        if (head >= 100) head = 0;
        return tmp;
    }

    void ShowInfo()
    {
        lastTime = Time.time;
        label[0].text = label[1].text;
        label[1].text = label[2].text;
        label[2].text = qInfo[headPlus()];
        lTime[0] = lTime[1];
        lTime[1] = lTime[2];
        lTime[2] = Time.time;
    }

    void FixedUpdate()
    {
        if (size>0 && Time.time - lastTime > delayTime)ShowInfo();

        for (int i = 0; i < 3; i++)
        {
            if (Time.time - lTime[i] < showTime)
            {
                label[i].gameObject.SetActive(true);
            }
            else
            {
                label[i].gameObject.SetActive(false);
            }
        }

        //if (Input.GetKeyDown(KeyCode.A)) AddInfo("A");
        //if (Input.GetKeyDown(KeyCode.S)) AddInfo("S");
        //if (Input.GetKeyDown(KeyCode.D)) AddInfo("D");
        //if (Input.GetKeyDown(KeyCode.F)) AddInfo("F");
        //if (Input.GetKeyDown(KeyCode.G)) AddInfo("G");
        //if (Input.GetKeyDown(KeyCode.H)) {
        //    print(Time.time);
        //    print(lTime[0]);
        //    print(showTime);
        //    print(Time.time - lTime[0] < showTime);
        //}
    }
}
