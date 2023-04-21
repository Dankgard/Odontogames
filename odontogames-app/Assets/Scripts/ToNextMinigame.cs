using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToNextMinigame : MonoBehaviour
{
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(NextMinigame);
    }

    public void NextMinigame()
    {
        GameManager.instance.NextMinigame();
    }
}
