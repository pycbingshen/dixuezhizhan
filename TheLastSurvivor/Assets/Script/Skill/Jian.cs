using UnityEngine;
using System.Collections;

public class Jian : MonoBehaviour 
{
    [HideInInspector][System.NonSerialized] public GameObject UserGo;
    [HideInInspector][System.NonSerialized] public GameObject TargetGo;

	void FixedUpdate()
    {
        if (TargetGo.GetComponent<Hero>()._currHP <= 0)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, TargetGo.transform.position + new Vector3(0, 1.1f, 0), 0.4f);
        Vector3 tmp = Vector3.zero;
        tmp.x = TargetGo.transform.position.x - transform.position.x;
        tmp.y = TargetGo.transform.position.z - transform.position.z;
        Quaternion mmm = Quaternion.Euler(0, -CalculateAngle(tmp) + 180  , 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, mmm, 360);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject != TargetGo)
            return;
        Debug.Log("弓箭手的箭\n");
        TargetGo.GetComponent<Hero>().BeAttacked(UserGo, 1);
        Destroy(gameObject);
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
