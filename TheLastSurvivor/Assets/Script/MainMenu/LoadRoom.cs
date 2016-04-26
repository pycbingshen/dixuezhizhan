using UnityEngine;
using System.Collections;
using client;
using proto_islandsurvival;

public class LoadRoom : MonoBehaviour 
{
    void OnClick()
    {
        Application.LoadLevel("Room");
    }
}
