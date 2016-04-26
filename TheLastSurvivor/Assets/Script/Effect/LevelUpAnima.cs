using UnityEngine;
using System.Collections;

public class LevelUpAnima : MonoBehaviour 
{
    public GameObject LevelUpEffect;

    public void Play()
    {
        if (transform.childCount != 0)
            Destroy(transform.GetChild(0).gameObject);
        GameObject effect = Instantiate(LevelUpEffect) as GameObject;

        effect.transform.parent = transform;
        effect.transform.localScale = Vector3.one;
        effect.transform.localPosition = Vector3.zero;
    }
}
