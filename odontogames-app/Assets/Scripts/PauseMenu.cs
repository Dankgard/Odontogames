using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseButton;

    private bool openMenu = false;
    private bool menuIsClosed = true;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    public void OnPausePressed()
    {
        openMenu = !openMenu;
        if (openMenu)
        {
            Pause();
        }
        else if (!openMenu)
        {
            if (!menuIsClosed)
            {
                Continue();
            }
        }
    }

    public void OnContinuePressed()
    {
        Continue();
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);
        menuIsClosed = true;
        Time.timeScale = 0f;
    }

    private void Continue()
    {
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);
        menuIsClosed = false;
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        MySceneManager.instance.LoadScene("PlayLevelMenu");
    }
}
