using UnityEngine;
using System.Collections;

public class Tip : MonoBehaviour {

    UILabel label;

    void Awake()
    {
        label = gameObject.GetComponent<UILabel>();
    }

    public void Show(string tip)
    {
        gameObject.GetComponent<UIPlayTween>().Play(true);
        label.text = tip;
    }
}
