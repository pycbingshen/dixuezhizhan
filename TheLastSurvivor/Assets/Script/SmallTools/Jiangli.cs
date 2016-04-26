using UnityEngine;
using System.Collections;

public class Jiangli : MonoBehaviour 
{
    void FixedUpdate()
    {
        transform.Rotate(0, 5, 0);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Player")
            return;

        collider.GetComponent<Hero>().ResumeFeibiaoNumber(1);
        GameObject.Find("Reward").GetComponent<Reward>().Need2Spawn = true;
        GameObject.Find("Reward").GetComponent<Reward>().LastDelFrame = Controller.CurrentFrameNum;
        Destroy(gameObject);
    }
}
