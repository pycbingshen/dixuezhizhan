using UnityEngine;
using System.Collections;

public static class MyTool {
    public static int GameModeToNum(string gamemode){
        if (gamemode == "战斗")return 1;
        if (gamemode == "生存")return 2;
        if (gamemode == "夺旗")return 3;
        return -1;
    }

    public static int TeamModeToNum(string teammode){
        if (teammode == "混战")return 1;
        if (teammode == "组队")return 2;
        //if (teammode == "随机")return 3;
        return -1;
    }

    public static string NumToGameMode(int num){
        if (num == 1)return "战斗";
        if (num == 2)return "生存";
        if (num == 3)return "夺旗";
        return null;
    }
    
    public static string NumToTeamMode(int num){
        if (num == 1)return "混战";
        if (num == 2)return "组队";
        //if (num == 3)return "随机";
        return null;
    }

    public static string GameModeNumToWinCondition(int num){
        if (num == 1)return "击杀数";
        if (num == 2)return "生命数";
        if (num == 3)return "回合数";
        return null;
    }
}
