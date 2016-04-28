using UnityEngine;
using System.Collections;

public enum SkillType
{
    pugong,
    feibiao,
    wudi,
    shanxian,
    leiting,
    yinshen,
    huoqiu,
    leidian,
    chuanxinjian,
}

public class Skill : MonoBehaviour 
{

    public GameObject FeibiaoPrefab;
    public GameObject WudiPrefab;
    public GameObject ShanxianPrefab1;
    public GameObject shanxianPrefab2;
    public GameObject LeitingPrefab;
    public GameObject HuoqiuWeibaPrefab;
    public GameObject HuoqiuPrefab;
    public GameObject LeidianPrefab;
    public GameObject LeidianCirclePrefab;
    public GameObject ChuanxinjianPrefab;

    public void CreatSkillEffect(GameObject userGo, SkillType skillType, Vector3 pos, GameObject targetGo = null)
    {
        Vector3 playerPos = userGo.transform.position;
        userGo.GetComponent<Hero>().SetVisible();

        if (userGo.GetComponent<Hero>()._isDead)
            return;

        if (Controller.CurrentFrameNum - GeneralData.SkillLastUseFrame [int.Parse(userGo.name), (int)skillType] < 50)
            return;

        if (userGo.name == GeneralData.myID.ToString())
        {
            if(skillType == SkillType.feibiao && userGo.GetComponent<Hero>().FeibiaoNumber <= 0)
                return ;
            GameObject go = GameObject.Find("UI Root/Skill_List/" + skillType.ToString() + "/Icon");
//            go.GetComponent<ButtonBase>().LastUseFrame = Controller.CurrentFrameNum;
            GameObject go2 = go.transform.parent.FindChild("CD").gameObject;
            if(go2.activeSelf)
                return ;
            go2.GetComponent<SkillCD>().CDTime = GeneralData.GetSkillCDTime(skillType); 
            go2.SetActive(true);
//            if (Controller.CurrentFrameNum - go.GetComponent<ButtonBase>().LastUseFrame < 50)
//                return;
//            else
//            {
//            }
        }

        GeneralData.SkillLastUseFrame [int.Parse(userGo.name), (int)skillType] = Controller.CurrentFrameNum;

        switch (skillType)
        {
            case SkillType.pugong:
                userGo.GetComponent<XUnit>()._attackTargetGo = targetGo;
                userGo.GetComponent<XUnit>().animator.SetBool("Attack",true);
                break;

            case SkillType.feibiao:
                userGo.GetComponent<Hero>().ResumeFeibiaoNumber(-1);
                GameObject newFeibiao = Instantiate(FeibiaoPrefab) as GameObject;
                newFeibiao.transform.parent = transform;
                playerPos.y += 2.1f;
                newFeibiao.transform.position = playerPos;
                newFeibiao.GetComponent<Feibiao>().user = userGo;
                newFeibiao.GetComponent<Feibiao>().angle = CalculateAngle(pos) * Mathf.Deg2Rad;
                break;

            case SkillType.wudi:
                GameObject newWudi = Instantiate(WudiPrefab) as GameObject;
                newWudi.transform.parent = transform;
                newWudi.transform.position = playerPos;
                newWudi.GetComponent<Wudi>()._followTargetGo = userGo;
                userGo.GetComponent<Hero>().m_Wudi = newWudi;
                break;

            case SkillType.shanxian:
                playerPos.y += 2f;
                GameObject newShanxian1 = Instantiate(ShanxianPrefab1) as GameObject;
                newShanxian1.transform.parent = transform;
                newShanxian1.transform.position = playerPos;

                RaycastHit hit;
//                float angel = (-userGo.transform.eulerAngles.y + 90) * Mathf.Deg2Rad;
//                Vector3 newPos = playerPos + new Vector3(Mathf.Cos(angel) * 12, 10, Mathf.Sin(angel) * 12);
                Vector3 newPos = pos + new Vector3(0, 10, 0);
                Vector3 fwd = transform.TransformDirection(Vector3.down);
//                Debug.Log("fwd = " + fwd);
                if(Physics.Raycast(newPos, fwd, out hit, 20, 1 << LayerMask.NameToLayer("Terrain")))
                {
                    Debug.Log(hit.point);
                    newPos = hit.point + new Vector3(0, 0.5f, 0);
                }
                else
                    Debug.Log("Can not find Raycast hit point!!!");
                StartCoroutine(shanxian(userGo, newPos, 20));

                GameObject newShanxian2 = Instantiate(shanxianPrefab2) as GameObject;
                newShanxian2.transform.parent = transform;
                newShanxian2.transform.position = newPos;
                break;

            case SkillType.leiting:
                RangeOfSkillHurt(userGo, 6);
                GameObject newLeiting = Instantiate(LeitingPrefab) as GameObject;
                playerPos.y += 2f;
                newLeiting.transform.parent = transform;
                newLeiting.transform.position = playerPos;
                break;
            
            case SkillType.yinshen:
                userGo.GetComponent<Hero>().StartHidding(300);
                break;

            case SkillType.huoqiu:
                pos = Vector3.ClampMagnitude(pos * 0.3f, 12);
                Vector3 newPos2 = playerPos + pos;
                newPos2.y = 30;
                GameObject newHuoqiuWeiba = Instantiate(HuoqiuWeibaPrefab) as GameObject;
                newHuoqiuWeiba.transform.parent = transform;
                newHuoqiuWeiba.transform.position = newPos2;
                GameObject newHuoqiu = Instantiate(HuoqiuPrefab) as GameObject;
                newHuoqiu.transform.parent = transform;
                newHuoqiu.transform.position = newPos2;
                newHuoqiu.GetComponent<Huoqiu>().userGo = userGo;
                break;

            case SkillType.leidian:
                GameObject newThunderCircle = Instantiate(LeidianCirclePrefab) as GameObject;
                newThunderCircle.transform.parent = transform;
                newThunderCircle.transform.position = userGo.transform.position + new Vector3(0, 2.1f, 0);
                for(int i = 1 ; i <= GeneralData.PlayerNum ; i++)
                {
                    GameObject thunderGo = GameObject.Find("Player/" + i.ToString());
                    if(thunderGo == null)
                        continue;
                    if(thunderGo == userGo)
                        continue;
                    if (thunderGo.GetComponent<Hero>().m_Wudi != null)
                        continue;
                    if(thunderGo.GetComponent<Hero>().TeamID == userGo.GetComponent<Hero>().TeamID)
                        continue;
                    if(Vector3.Distance(userGo.transform.position, thunderGo.transform.position) < 6)
                    {
                        if(thunderGo.GetComponent<Hero>()._isMyHero)
                            StartCoroutine(GameObject.Find("Controller").GetComponent<PlayerInput>().CanNotControll(75));
                        GameObject.Find("Player/" + i.ToString()).GetComponent<Hero>().Beikongzhi(75);
                        GameObject newThunder = Instantiate(LeidianPrefab) as GameObject;
                        newThunder.transform.parent = transform;
                        newThunder.GetComponent<ThunderLightningBolt>().target = thunderGo.transform;
                        newThunder.GetComponent<ThunderLightningBolt>().startTs = userGo.transform;
                    }
                }
                break;

            case SkillType.chuanxinjian:
                GameObject newChuanxinjian = Instantiate(ChuanxinjianPrefab) as GameObject;
                newChuanxinjian.transform.parent = transform;
                newChuanxinjian.transform.position = userGo.transform.position + new Vector3(0, 4.1f, 0);
                newChuanxinjian.GetComponent<Chuanxinjian>().user = userGo;
                float angel = CalculateAngle(pos);
                newChuanxinjian.GetComponent<Chuanxinjian>().angle = angel * Mathf.Deg2Rad;
                newChuanxinjian.transform.rotation = Quaternion.Euler(0, -angel + 180, 0);
                break;

            default :
                Debug.Log("Get Server UnDefine Skill Type!!!");
                break;

        }

    }

    IEnumerator shanxian(GameObject go, Vector3 pos, int time)
    {
        while (time-- > 0)
            yield return new WaitForFixedUpdate();
        go.transform.position = pos;
    }

    public void RangeOfSkillHurt(GameObject attackerGo, float range)
    {
        for (int i = 1; i <= GeneralData.PlayerNum; i++)
        {
            GameObject beAttackGo = GameObject.Find("Player/" + i.ToString());
            if(beAttackGo == null)
                continue;
            if(attackerGo == beAttackGo)
                continue;
            if (Vector3.Distance(attackerGo.transform.position, beAttackGo.transform.position) < range)
            {
                Debug.Log("雷霆\n");
                beAttackGo.GetComponent<Hero>().BeAttacked(attackerGo, 1);
            }
        }
    }

    public void RangeOfSkillHurt(GameObject attackerGo, Vector3 pos, float range)
    {
        pos.y = attackerGo.transform.position.y;
        for (int i = 1; i <= GeneralData.PlayerNum; i++)
        {
            GameObject beAttackGo = GameObject.Find("Player/" + i.ToString());
            if(beAttackGo == null)
                continue;
            if(attackerGo == beAttackGo)
                continue;
            if (Vector3.Distance(pos, beAttackGo.transform.position) < range)
            {
                Debug.Log("火球\n");
                beAttackGo.GetComponent<Hero>().BeAttacked(attackerGo, 1);
            }
        }
    }

    public float CalculateAngle(Vector3 vec)
    {
        float res;
        if (Mathf.Abs(vec.x) > 0.1f)
        {
            //计算角度
            res = Mathf.Atan(vec.y / vec.x);
            if (vec.x < 0f)
                res += Mathf.PI;
            res = res * Mathf.Rad2Deg;
            while (res > 360f)
                res -= 360f;
            while (res < 0)
                res += 360f;
            return res;
        }
        else
        {
            if(vec.y > 0)
                return 90f;
            else
                return 270f;
        }
    }
}
