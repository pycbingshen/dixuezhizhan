using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using client;
using proto_islandsurvival;

public class SelectRolePanel : MonoBehaviour {
    int role = -1;
    Transform Introduce, Content, Skill, Skill1, Skill2, Skill3, Skill4;
    UISprite Role0, Role1, Role2, Role3,ok;
    void Awake() {
        Introduce = gameObject.transform.Find("Explanation/Introduce");
        Content = gameObject.transform.Find("Explanation/Introduce/Content");
        Skill = gameObject.transform.Find("Explanation/Skill");
        Skill1 = gameObject.transform.Find("Explanation/Skill/Skill1");
        Skill2 = gameObject.transform.Find("Explanation/Skill/Skill2");
        Skill3 = gameObject.transform.Find("Explanation/Skill/Skill3");
        Skill4 = gameObject.transform.Find("Explanation/Skill/Skill4");

        Role0 = gameObject.transform.Find("Role/Top/Role0").GetComponent<UISprite>();
        Role1 = gameObject.transform.Find("Role/Top/Role1").GetComponent<UISprite>();
        Role2 = gameObject.transform.Find("Role/Top/Role2").GetComponent<UISprite>();
        Role3 = gameObject.transform.Find("Role/Top/Role3").GetComponent<UISprite>();

        ok= gameObject.transform.Find("Role/Bottom/OK").GetComponent<UISprite>();
        ChangeRole(-1);
    }

    public void ChangeRole(int id)
    {
        if (id != -1)
        {
            role = id;
            //设置按钮
            Color color = new Color(1, 1, 1);
            Role0.color = color;
            Role1.color = color;
            Role2.color = color;
            Role3.color = color;
            ok.color = color;
            color = new Color(0.5f, 0.5f, 0.5f);
            if (id == 0) Role0.color = color;
            if (id == 1) Role1.color = color;
            if (id == 2) Role2.color = color;
            if (id == 3) Role3.color = color;
            //print(color);
        }

        if (Resources.Load("xml/Role"))
        {
            XmlDocument role = new XmlDocument();
            role.LoadXml(Resources.Load("xml/Role").ToString());
            XmlNodeList roleList = role.SelectSingleNode("role").ChildNodes;

            foreach (XmlElement item in roleList)
            {
                if (int.Parse(item.GetAttribute("id")) == id)
                {
                    UILabel label;
                    UISprite sprite;
                    if (id != -1)
                    {
                        label =Introduce.GetComponent<UILabel>();
                        label.text = "职业说明";
                        label = Content.GetComponent<UILabel>();
                        label.text = item.GetAttribute("explanation");
                        label = Skill.GetComponent<UILabel>();
                        label.text = "职业技能";
                    }
                    else
                    {
                        label = Introduce.GetComponent<UILabel>();
                        label.text = "技能说明";
                        label = Content.GetComponent<UILabel>();
                        label.text = "每个职业技能由4个通用技能以及2个职业技能组成";
                        label = Skill.GetComponent<UILabel>();
                        label.text = "通用技能";
                    }

                    label = Skill1.Find("Label").GetComponent<UILabel>();
                    label.text = item.GetAttribute("skill1s");
                    sprite = Skill1.GetComponent<UISprite>();
                    sprite.spriteName = item.GetAttribute("skill1icon");

                    label = Skill2.Find("Label").GetComponent<UILabel>();
                    label.text = item.GetAttribute("skill2s");
                    sprite = Skill2.GetComponent<UISprite>();
                    sprite.spriteName = item.GetAttribute("skill2icon");

                    if (id != -1)
                    {
                        Skill3.gameObject.SetActive(false);
                        Skill4.gameObject.SetActive(false);
                    }
                    else
                    {
                        Skill3.gameObject.SetActive(true);
                        Skill4.gameObject.SetActive(true);
                        label = Skill3.Find("Label").GetComponent<UILabel>();
                        label.text = item.GetAttribute("skill3s");
                        sprite = Skill3.GetComponent<UISprite>();
                        sprite.spriteName = item.GetAttribute("skill3icon");

                        label = Skill4.Find("Label").GetComponent<UILabel>();
                        label.text = item.GetAttribute("skill4s");
                        sprite = Skill4.GetComponent<UISprite>();
                        sprite.spriteName = item.GetAttribute("skill4icon");
                    }

                    break;
                }
            }
        }
    }

    public void SelectRoleOK()
    {
        if (role == -1) return;
        GeneralData.charaID = role;
        gameObject.SetActive(false);

        CMessage mess = new CMessage();
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSChoseRole));
        CSChoseRole proto = new CSChoseRole();
        proto.role_id = GeneralData.charaID;
        mess.m_proto = proto;
        program.SendQueue.push(mess);

        print("发送职业ID："+GeneralData.charaID);
    }

    private bool IsContent(string str)
    {
        if(str==null)return false;
        if(str=="") return false;
        return true;
    }
}