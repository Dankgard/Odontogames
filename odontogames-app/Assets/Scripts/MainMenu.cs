using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    public Button globalUserListButton;
    public Button teamsButton;
    public Button playMenuButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (globalUserListButton.IsActive())
            {
                globalUserListButton.onClick.Invoke();
            }
            else if (teamsButton.IsActive())
            {
                teamsButton.onClick.Invoke();
            }
            else if (playMenuButton.IsActive())
            {
                playMenuButton.onClick.Invoke();
            }
        }
    }

    public void OnGlobalUserList()
    {
        MySceneManager.instance.LoadScene("GlobalUserList");
    }

    public void OnProfileMenu()
    {
        MySceneManager.instance.LoadScene("ProfileMenu");
    }

    public void OnTeamsListMenu()
    {
        MySceneManager.instance.LoadScene("TeamsLists");
    }

    public void OnPlayLevelMenu()
    {
        //SceneHandler.instance.LoadScene("CreateNewGroup");
        MySceneManager.instance.LoadScene("PlayLevelMenu");
    }
}
