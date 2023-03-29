using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadEscapeRoomGame : MonoBehaviour
{
    public string level;

    public void Start()
    {
        transform.GetChild(0).transform.GetComponent<Text>().text = level;
    }

    public void LoadLevel()
    {
        SceneHandler.instance.LoadScene(level);
    }
}
