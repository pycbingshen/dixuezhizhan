using UnityEngine;
using System.Collections;
using client;
using proto_islandsurvival;

public class DragSkillButton : ButtonBase 
{
    [HideInInspector][System.NonSerialized] public MIndicator Indicator;
    [HideInInspector][System.NonSerialized] public int IndicatorType;
    [HideInInspector][System.NonSerialized] public SkillType skillName;

    private bool _isActive;
    private GameObject _circle;


    // Use this for initialization
    void Start () 
    {
//        skillName = SkillType.feibiao;
        Indicator = GameObject.Find("Indicator").GetComponent<MIndicator>();
//        IndicatorType = 1;
        _circle = transform.parent.FindChild("Circle").gameObject;
        _isActive = false;
    }
    
    // Update is called once per frame
    void Update () 
    {
        if (!_isActive)
            return;
        transform.localPosition = Vector3.ClampMagnitude( transform.localPosition, 40);
    }
    
    void OnPress(bool isDown)
    {
        _isActive = isDown;
        if (isDown)
        {
            if(IndicatorType == 1)
                Indicator.SetState(gameObject, Indicator.Arrow, out Indicator.ArrowActive, true);
            else
                Indicator.SetState(gameObject, Indicator.Magic, out Indicator.MagicActive, true);
            _circle.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            transform.parent.FindChild("EndCD").gameObject.SetActive(false);
        }
        else
        {
//            m_skill.CreatSkillEffect(SkillType.feibiao, transform.localPosition);
            CMessage mess = new CMessage();
            mess.m_head.m_framenum = Controller.CurrentFrameNum;
            mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSSkill));
            CSSkill proto = new CSSkill();
            proto.pos_x = transform.localPosition.x;
            proto.pos_z = transform.localPosition.y;
            proto.skill_id = (int) skillName;
            mess.m_proto = proto;
            program.SendQueue.push(mess);

            if(IndicatorType == 1)
                Indicator.SetState(gameObject, Indicator.Arrow,out Indicator.ArrowActive, false);
            else
                Indicator.SetState(gameObject, Indicator.Magic,out Indicator.MagicActive, false);
//            transform.parent.FindChild("CD").gameObject.SetActive(true);
            _circle.transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
        }
    }
}
