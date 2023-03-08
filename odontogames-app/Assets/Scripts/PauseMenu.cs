using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
