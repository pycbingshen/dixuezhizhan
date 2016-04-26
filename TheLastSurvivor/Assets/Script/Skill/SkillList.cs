using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class SkillList : MonoBehaviour 
{
    public GameObject DragSkillPrefab;
    public GameObject ClickSkillPrefab;
    [HideInInspector][System.NonSerialized] public UIButton m_PuGongBut;
    [HideInInspector][System.NonSerialized] public GameObject m_PuGongCD;
    [HideInInspector][System.NonSerialized] public Hero[] m_Hero = new Hero[10];
    private PlayerInput m_PlayerInput;
    private UIButton m_FeibiaoBut;
    private float turnBase = 3.5f;
    private float turnAdd = 0.5f;

    public void LoadData()
    {
        
        if (Resources.Load ("xml/Skill"))
        {
            XmlDocument xml = new XmlDocument();
            //Debug.Log(Resources.Load ("xml/strike"));
            xml.LoadXml(Resources.Load ("xml/Skill").ToString());
            XmlNodeList xmlNodeList = xml.SelectSingleNode("skill").ChildNodes;
            for(int i = 0 ; i < GeneralData.SkillNum ; i++ )
            {
                foreach (XmlElement xl1 in xmlNodeList)
                {
                    if (xl1.GetAttribute("id") == GeneralData.skill[i].ToString())
                    {
                        GeneralData.skillCD[i] = float.Parse( xl1.GetAttribute("CD") );
                        if(xl1.GetAttribute("but_type") == "0")
                        {
                            GameObject newBut = Instantiate(ClickSkillPrefab) as GameObject;
                            GameObject icon = newBut.transform.FindChild("Icon").gameObject;
                            icon.GetComponent<UISprite>().spriteName = xl1.GetAttribute("icon_name");
                            icon.GetComponent<UIButton>().normalSprite = xl1.GetAttribute("icon_name");
                            icon.GetComponent<ClickSkillButton>().skillName = (SkillType)GeneralData.skill[i];
                            newBut.transform.parent = transform;
                            newBut.transform.localScale = Vector3.one;
//                            newBut.transform.localPosition = new Vector3 (0, 250 - 120 * i, 0);
                            if(GeneralData.skill[i] == 0)
                            {
                                newBut.transform.localScale = Vector3.one * 1.5f;
                                newBut.transform.localPosition = transform.position;
                            }
                            else
                            {
                                float turnDis = turnBase - turnAdd * 3f / (GeneralData.SkillNum - 1);
                                newBut.transform.localPosition = transform.position + new Vector3(Mathf.Cos(turnDis) * 200, Mathf.Sin(turnDis) * 200, 0);
                                turnAdd += 1f;
                            }
                            newBut.name = xl1.GetAttribute("name");

                            if(xl1.GetAttribute("id") == "0" &&
                               GameObject.Find("Player/" + GeneralData.myID.ToString()).GetComponent<Hero>().isRangeHero )
                            {
                                icon.GetComponent<UISprite>().spriteName = "2_jianshugeming";
                                icon.GetComponent<UIButton>().normalSprite = "2_jianshugeming";
                                GeneralData.skillCD[i] = 6;
                            }
                        }

                        if(xl1.GetAttribute("but_type") == "1")
                        {
                            GameObject newBut = Instantiate(DragSkillPrefab) as GameObject;
                            GameObject icon = newBut.transform.FindChild("Icon").gameObject;
                            icon.GetComponent<UISprite>().spriteName = xl1.GetAttribute("icon_name");
                            icon.GetComponent<UIButton>().normalSprite = xl1.GetAttribute("icon_name");
                            icon.GetComponent<DragSkillButton>().skillName = (SkillType)GeneralData.skill[i];
                            icon.GetComponent<DragSkillButton>().IndicatorType = int.Parse(xl1.GetAttribute("ind_type"));
                            newBut.transform.parent = transform;
                            newBut.transform.localScale = Vector3.one;
//                            newBut.transform.localPosition = new Vector3 (0, 250 - 120 * i, 0);
                            if(GeneralData.skill[i] == 0)
                            {
                                newBut.transform.localScale = Vector3.one * 1.5f;
                                newBut.transform.localPosition = transform.position;
                            }
                            else
                            {
                                float turnDis = turnBase - turnAdd * 3f / (GeneralData.SkillNum - 1);
                                newBut.transform.localPosition = transform.position + new Vector3(Mathf.Cos(turnDis) * 200, Mathf.Sin(turnDis) * 200, 0);
                                turnAdd += 1f;
                            }
                            newBut.name = xl1.GetAttribute("name");

                            if(xl1.GetAttribute("name") == "feibiao")
                                newBut.transform.Find("Number").gameObject.SetActive(true);
                        }

                        break;
                    }
                }
            }
        }

        m_PuGongBut = transform.FindChild("pugong/Icon").GetComponent<UIButton>();
        m_PuGongCD = transform.FindChild("pugong/CD").gameObject;
        m_PlayerInput = GameObject.Find("Controller").GetComponent<PlayerInput>();
        m_FeibiaoBut = GameObject.Find("UI Root/Skill_List/feibiao/Icon").GetComponent<UIButton>();
    }
	// Use this for initialization
	public void XStart () 
    {
//        LoadData();
//        for (int i = 1; i <= GeneralData.PlayerNum; i++)
//            GameObjectGet(i);

	}

    public void GameObjectGet(int index, GameObject go)
    {
        m_Hero [index] = go.GetComponent<Hero>();
    }

    public void XFixedUpdate()
    {
        if (GeneralData.choosed)
        {
            m_PuGongBut.isEnabled = m_PlayerInput.CanControll && !m_PuGongCD.activeSelf && PugongPD();
            Hero hero = GameObject.Find("Player/" + GeneralData.myID.ToString()).GetComponent<Hero>();
            m_FeibiaoBut.isEnabled = !hero._isDead && hero.FeibiaoNumber > 0;
        }
    }

    public bool PugongPD()
    {
        float range = m_Hero[GeneralData.myID].GetComponent<Hero>()._attackRange;
        for (int i = 1; i <= GeneralData.PlayerNum; i ++)
        {
            if(m_Hero[i] == null)
            {
                continue;
            }

            if(m_Hero[i].TeamID == m_Hero[GeneralData.myID].TeamID)
                continue;

            Renderer[] mRenderers = m_Hero[i].gameObject.GetComponentsInChildren<Renderer>();
            if(mRenderers [0].enabled == false)
                continue;

            if(i == GeneralData.myID)
                continue ;

            if(m_Hero[i].HP.value <= 0)
                continue ;

            if(m_Hero[i].Visible == false)
                continue ;

            if(Vector3.Distance(m_Hero[i].transform.position, m_Hero[GeneralData.myID].transform.position) < range)
            {
                float angle1 = m_Hero[GeneralData.myID].transform.eulerAngles.y;
                float angle2 = CalculateAngle(m_Hero[i].transform.position - m_Hero[GeneralData.myID].transform.position);
                if(Mathf.Abs(angle1 - angle2) < 60 || Mathf.Abs(angle1 - angle2) > 300  )
                {
                    m_PuGongBut.gameObject.GetComponent<ClickSkillButton>().TargetID = i;
                    return true;
                }
            }
        }
        return false;
    }

    public float CalculateAngle(Vector3 vec)
    {
        float res;
        if (Mathf.Abs(vec.x) > 0.1f)
        {
            //计算角度
            res = Mathf.Atan(vec.z / vec.x);
            if (vec.x < 0f)
                res += Mathf.PI;
            res = res * Mathf.Rad2Deg;
            res = -res + 90;
            while (res > 360f)
                res -= 360f;
            while (res < 0)
                res += 360f;
            return res;
        }
        else
        {
            if(vec.y > 0)
                return 0f;
            else
                return 180f;
        }
    }
}
