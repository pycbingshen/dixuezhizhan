using UnityEngine;
using System.Collections;

public class KillSE : MonoBehaviour {

    AudioSource se;

    void Awake()
    {
        se = GetComponent<AudioSource>();
    }

    public void PlaySE(AudioClip clip)
    {
        se.clip = clip;
        se.Play();
    }
}
