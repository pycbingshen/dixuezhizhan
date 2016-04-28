using UnityEngine;
using System.Collections;
using client;
using proto_islandsurvival;

public class ClickSkillButton : ButtonBase 
{
    
    [HideInInspector][System.NonSerialized] public Skill m_skill;
    [HideInInspector][System.NonSerialized] public SkillType skillName;
    [HideInInspector][System.NonSerialized] public int TargetID;


    // Use this for initialization
    void Start () 
    {
//        skillName = SkillType.wudi;
        m_skill = GameObject.Find("Skill_Effect").GetComponent<Skill>();
    }
    
    void OnPress(bool isDown)
    {
//        GameObject go = GameObject.Find("Player/" + GeneralData.myID.ToString());
//        m_skill.CreatSkillEffect(SkillType.wudi, Vector3.zero, go);
        if (isDown)
        {
            CMessage mess = new CMessage();
            mess.m_head.m_framenum = Controller.CurrentFrameNum;
            mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSSkill));
            CSSkill proto = new CSSkill();
            proto.skill_id = (int)skillName;
            if (skillName == SkillType.pugong)
                proto.target_id = TargetID;
            if(skillName == SkillType.shanxian)
            {
                RaycastHit hit;
                Vector3 fwd, pos;
                Transform ts = GameObject.Find("Player/" + GeneralData.myID.ToString()).transform;
                float angel;
                int moveNum = GameObject.Find("Controller").GetComponent<PlayerInput>().moveNum;
                if( moveNum == -1)
                    angel = (-ts.eulerAngles.y + 90) * Mathf.Deg2Rad;
                else
                    angel = (moveNum * 22.5f) * Mathf.Deg2Rad;
                fwd = new Vector3(Mathf.Cos(angel), 0, Mathf.Sin(angel));
                if(Physics.Raycast(ts.position, fwd, out hit, 12, 1 << LayerMask.NameToLayer("Border")))
                {
                    float dis = Vector3.Distance(ts.position, hit.point);
                    float bl = 1f - 0.8f / dis;
                    pos = Vector3.Lerp(ts.position, hit.point, bl);
                }
                else
                    pos = ts.position + new Vector3(Mathf.Cos(angel) * 12, 0, Mathf.Sin(angel) * 12);

                proto.pos_x = pos.x;
                proto.pos_z = pos.z;
            }
            mess.m_proto = proto;
            program.SendQueue.push(mess);

            transform.parent.FindChild("EndCD").gameObject.SetActive(false);
//            transform.parent.FindChild("CD").gameObject.SetActive(true);
        }
    }
}
