using UnityEngine;
using System.Collections;
using client;
using proto_islandsurvival;
enum MessageType
{
    e_start,
    e_controNum,
    e_move2Pos,
    e_attackGo
};
public class Controller: MonoBehaviour 
{
    
    [HideInInspector][System.NonSerialized] public Skill m_skill;
    static public int CurrentFrameNum;
    public int[] head = new int[500];
    public int p,top;
   
    private int DT = 7;
    private int AD = 3;
    private bool _isGameStart = false;
    private bool _isLoadSucceed = false;
    private bool _addSpeed = false;
    private int _addSpeedNum;
    public bool IsGamePause = false;

    public void XStart()
    {
        m_skill = GameObject.Find("Skill_Effect").GetComponent<Skill>();
        Time.timeScale = 0;
//        Debug.Log("Waiting for other player.");
//        CMessage mess = new CMessage();
//        mess.m_head.m_framenum = 0;
//        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSLoadSucceed));
//        CSLoadSucceed proto = new CSLoadSucceed();
//        mess.m_proto = proto;
//        program.SendQueue.push(mess);

        CurrentFrameNum = 0;
        p = 0;
        top = 0;
    }

    public void XUpdate()
    {

        if (!_isLoadSucceed)
        {
            _isLoadSucceed = true;
            Debug.Log("Waiting for other player.");
            CMessage mess = new CMessage();
            mess.m_head.m_framenum = 0;
            mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSLoadSucceed));
            CSLoadSucceed proto = new CSLoadSucceed();
            mess.m_proto = proto;
            program.SendQueue.push(mess);
        }
        //Debug.Log(Time.realtimeSinceStartup);
        if (Time.timeScale == 0)
        {
            if(_isGameStart)
            {
                CMessage mess = program.RecvQueue.front();
                int maxFrameNum = program.RecvQueue.GetLastFrame();
                if(mess.m_head.m_framenum != maxFrameNum)
                {
                    Debug.Log("OK");
                    Time.timeScale = 1;
                }
            }
            else
            {
                if(!program.RecvQueue.empty())
                {
                    CMessage mess = program.RecvQueue.front();
                    program.RecvQueue.pop();
                    if(mess.m_proto is SCGameStart)
                    {
                        GameObject.Find("UI Root/load").SetActive(false);
                        Debug.Log("Start");
                        Time.timeScale = 1;
                        _isGameStart = true;
                    }
                }
            }
        }
    }

    public void XFixedUpdate () 
    {
        CurrentFrameNum ++;
        if (CurrentFrameNum <= DT)
            return;

        while (true)
        {
            if(!program.RecvQueue.empty())
            {
                CMessage mess = program.RecvQueue.front();
                //Debug.Log("head = " + mess.m_head.m_framenum + "tatil = " + program.RecvQueue.GetLastFrame()+"curr = "+ CurrentFrameNum);
                if(mess.m_head.m_framenum + DT == CurrentFrameNum)
                {
                    DealWith(mess);
                    program.RecvQueue.pop();
                }
                else
                {
                    int maxFrameNum = program.RecvQueue.GetLastFrame();
                    if(maxFrameNum == mess.m_head.m_framenum)
                    {
                        Time.timeScale = 0;
                        Debug.Log("Next frame haven't enough!");
                        break ;
                    }

                    if(!_addSpeed && maxFrameNum > mess.m_head.m_framenum + DT)
                    {
                        Debug.Log(maxFrameNum + "  " + mess.m_head.m_framenum + "*******************");
                        Time.timeScale = 3;
                        _addSpeed = true;
                        _addSpeedNum = AD;

                        Debug.Log("Too slowly! Quicken!");
                        return ;
                    }

                    break;
                }
            }
        }

        if (_addSpeed)
        {
            _addSpeedNum--;
            if(_addSpeedNum == 0)
            {
                _addSpeed = false;
                Time.timeScale = 1;
                Debug.Log("resume");
            }
        }
    }

    private void DealWith(CMessage mess)
    {
        if (mess.m_proto is SCMove)
        {
            SCMove move = (SCMove)mess.m_proto;
            GameObject.Find("Player/" + move.player_id.ToString()).GetComponent<Hero>().CurrentMoveDirection = move.dir;
            return ;
        }
       
        /*
        if (mess.m_proto is SCAttack)
        {
            SCAttack move = (SCAttack)mess.m_proto;
            GameObject go;
            if(move.target_id < 10)
                go = GameObject.Find("Player" + move.target_id.ToString());
            else
                go = GameObject.Find("Monster/" + move.target_id.ToString());

            if(move.attacker_id < 10)
            {
                Hero hero = GameObject.Find("Player" + move.attacker_id.ToString()).GetComponent<Hero>();
                //Debug.Log(go);
                hero._attackTargetGo = go;
                hero._isAttackingGo = true;
                hero._isMovingToPos = false;
            }
            else
            {
                Debug.Log("Attacker Type Error!");
            }

            return ;
        }*/

        if (mess.m_proto is SCFrameMess)
        {
            return ;
        }

        if (mess.m_proto is SCGamePause)
        {
            Debug.Log("GAME PAUSE!!!");
            Time.timeScale = 0;
            IsGamePause = true;
            return ;
        }

        if (mess.m_proto is SCSkill)
        {
            SCSkill skill = (SCSkill)mess.m_proto;
            SkillType skillType = (SkillType)skill.skill_id;
            GameObject userGo = GameObject.Find("Player/"+skill.attacker_id.ToString());
            Vector3 pos;
            switch(skillType)
            {
                case SkillType.pugong:
                    GameObject go = GameObject.Find("Player/" + skill.target_id.ToString());
                    m_skill.CreatSkillEffect(userGo, SkillType.pugong, Vector3.zero, go);
                    break;

                case SkillType.feibiao :
                    pos = new Vector3(skill.pos_x, skill.pos_z, 0);
                    m_skill.CreatSkillEffect(userGo, SkillType.feibiao, pos);
                    break;

                case SkillType.wudi:
                    m_skill.CreatSkillEffect(userGo, SkillType.wudi, Vector3.zero);
                    break;

                case SkillType.shanxian:
                    pos = new Vector3(skill.pos_x, 0, skill.pos_z);
                    m_skill.CreatSkillEffect(userGo, SkillType.shanxian, pos);
                    break;

                case SkillType.leiting:
                    m_skill.CreatSkillEffect(userGo, SkillType.leiting, Vector3.zero);
                    break;

                case SkillType.yinshen:
                    m_skill.CreatSkillEffect(userGo, SkillType.yinshen, Vector3.zero);
                    break;
                
                case SkillType.huoqiu:
                    pos = new Vector3(skill.pos_x, 0, skill.pos_z);
                    m_skill.CreatSkillEffect(userGo, SkillType.huoqiu, pos);
                    break;

                case SkillType.leidian:
                    m_skill.CreatSkillEffect(userGo, SkillType.leidian, Vector3.zero);
                    break;
                
                case SkillType.chuanxinjian:
                    pos = new Vector3(skill.pos_x, skill.pos_z, 0);
                    m_skill.CreatSkillEffect(userGo, SkillType.chuanxinjian, pos);
                    break;

                default :
                    Debug.Log("Undefine Skill Type!!!");
                    break;
            }

            return ;
        }

        //这两个记得加内容
        if (mess.m_proto is SCChoseRole)
        {
            SCChoseRole gameMess = (SCChoseRole)mess.m_proto;
            Debug.Log(gameMess.player_id + " wanjiaxuanzelezhiye " + gameMess.role_id);
            GameObject.Find("Player").GetComponent<PlayerSpawn>().Spawn(gameMess.player_id, gameMess.role_id);
            return;
        }
        if (mess.m_proto is SCPlayerExitGame)
        {
            SCPlayerExitGame gameMess = (SCPlayerExitGame)mess.m_proto;
            GameObject go = GameObject.Find("Player/" + gameMess.player.ToString());
            if(go != null)
            {
                Hero hero = go.GetComponent<Hero>();

                RankList list = GameObject.Find("UI Root/RankList").GetComponent<RankList>();
                int listID = list.playerIdToRankID[gameMess.player];
                list.Refresh( listID , -list.num[listID] );
                GameJudgement.DealWith();
//                if(GeneralData.gameModeNum == 2 && list.num[list.playerIdToRankID[gameMess.player]] != 0)
//                {
//                    if(GeneralData.teamModeNum == 1)
//                    {
//                        GeneralData.AlivePlayerNum[1] --;
//                        if(GeneralData.AlivePlayerNum[1] <= 1)
//                            GameJudgement.GameEnd(true);
//                    }
//                    else
//                    {
//                        int teamID = GeneralData.TeamId[gameMess.player];
//                        GeneralData.AlivePlayerNum[teamID]--;
//                        if(GeneralData.AlivePlayerNum[teamID] <= 1)
//                            GameJudgement.GameEnd(true);
//                    }
//                }
                Destroy(hero.HP.gameObject);
                Destroy(hero.NameLabel);
                Destroy(go);
            }
            Info m_Info = GameObject.Find("UI Root/Info").GetComponent<Info>();
            m_Info.AddInfo("玩家 " + GeneralData.PlayerName[gameMess.player] + " 离开了游戏");
            print("玩家" + gameMess.player + "退出游戏");
            //player
            return;
        }
    }

}