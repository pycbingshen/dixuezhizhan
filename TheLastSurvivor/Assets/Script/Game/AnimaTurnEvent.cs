using UnityEngine;
using System.Collections;

public class AnimaTurnEvent : MonoBehaviour 
{
    public Animator animator;
    public GameObject JianPrefab;

    void Attack( )
    {
        if (gameObject.GetComponent<Hero>().isRangeHero)
        {
            GameObject jian = Instantiate(JianPrefab) as GameObject;
            jian.transform.parent = GameObject.Find("Skill_Effect").transform;
            jian.transform.position = gameObject.transform.position + new Vector3(0, 1.1f, 0);
            jian.GetComponent<Jian>().UserGo = gameObject;
            jian.GetComponent<Jian>().TargetGo = GetComponent<XUnit>()._attackTargetGo;
        }
        else
        {
            GameObject beAttackGo = GetComponent<XUnit>()._attackTargetGo;
            if (beAttackGo != null)
            {
                Debug.Log("普攻\n");
                beAttackGo.GetComponent<XUnit>().BeAttacked(gameObject, gameObject.GetComponent<XUnit>()._attackValue);
            }
        }
    }
}
