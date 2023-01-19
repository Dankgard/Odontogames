using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    public Button globalUserListButton;
    public Button teamsButton;
    public Button newGroupMenuButton;

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
            else if (newGroupMenuButton.IsActive())
            {
                newGroupMenuButton.onClick.Invoke();
            }
        }
    }

    public void OnGlobalUserList()
    {
        SceneHandler.instance.LoadScene("GlobalUserList");
    }

    public void OnTeamsListMenu()
    {
        SceneHandler.instance.LoadScene("TeamsLists");
    }

    public void OnNewGroupMenu()
    {
        SceneHandler.instance.LoadScene("CreateNewGroup");
    }
}
