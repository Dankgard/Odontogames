using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelManager : MonoBehaviour
{
    public void OnBackPressed()
    {
        MySceneManager.instance.LoadScene("MainMenu");
    }
}
