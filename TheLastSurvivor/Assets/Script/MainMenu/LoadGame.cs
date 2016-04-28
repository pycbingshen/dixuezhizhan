using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour 
{
    public UISlider slide;
    AsyncOperation async;

	// Use this for initialization
	void Start () 
    {
        StartCoroutine(LoadScene());
	}
	
	// Update is called once per frame
	void Update () 
    {
        slide.value = async.progress;
	}

    IEnumerator LoadScene()
    {
        Debug.Log(GeneralData.myTeamId);
        async = Application.LoadLevelAsync("Fighting");

        yield return async;
    }
}
