using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject playerCharacter;
    private bool menuOpen;
    private bool closeMenu;

    void Start()
    {
        menuOpen = false;
        closeMenu = true;
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            menuOpen = !menuOpen;
        }
        if (menuOpen)
        {
            Pause();
        }
        else if (!menuOpen)
        {
            if (!closeMenu)
            {
                Continue();
            }
        }
    }

    public void Pause()
    {
        playerCharacter.GetComponent<PlayerBehavior>().enabled = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        closeMenu = true;
    }

    public void Continue()
    {
        playerCharacter.GetComponent<PlayerBehavior>().enabled = true;
        pauseMenuUI.SetActive(false);
        closeMenu = false;
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        SceneHandler.instance.LoadScene("PlayLevelMenu");
    }
}
