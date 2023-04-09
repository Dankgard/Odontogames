using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using StrapiForUnity;
using System.Collections.Generic;

public class TeamsList : MonoBehaviour
{
    public GameObject groupPrefab;
    public GameObject userPrefab;
    public GameObject viewport;

    public GameObject teamPanel;
    public GameObject newTeamPanelOne;
    public GameObject newTeamPanelTwo;

    public GameObject deleteGroupsButton;

    public GameObject usersViewport;

    private StrapiTeamsData[] teams = null;
    private GameObject[] teamID = null;

    public event Action OnStarted = delegate { };

    private string newTeamName = "";

    private StrapiUser[] users = null;
    private GameObject[] userID = null;
    private StrapiUser[] newTeamUsers = null;

    void Start()
    {
        OnStarted += BuildUserScreen;

        StartCoroutine(GetTeamsCoroutine());
    }

    private void Update()
    {
        if (teamID != null)
            if (!deleteGroupsButton.activeSelf)
            {
                for (int i = 0; i < teamID.Length; i++)
                {
                    if (teamID[i].transform.GetChild(2).GetComponent<Toggle>().isOn)
                    {
                        deleteGroupsButton.SetActive(true);
                        return;
                    }
                }
                deleteGroupsButton.SetActive(false);
            }
    }

    public void BuildUserScreen()
    {
        teams = StrapiComponent._instance.GetTeams();
        teamID = new GameObject[teams.Length];

        for (int i = 0; i < teams.Length; i++)
        {
            StrapiUserTeam team = teams[i].attributes;
            GameObject teamData = Instantiate(groupPrefab, new Vector3(0, viewport.transform.position.y - 50 * i, 0), Quaternion.identity, viewport.transform);
            teamData.transform.GetChild(0).GetComponent<Text>().text = team.teamname;
            teamData.transform.GetChild(1).GetComponent<Text>().text = team.numplayers.ToString();

            teamData.GetComponent<Button>().onClick.AddListener(() => OnTeamButtonkPressed());

            teamID[i] = teamData;
        }

        teamPanel.SetActive(false);
        newTeamPanelOne.SetActive(false);
        newTeamPanelTwo.SetActive(false);
    }

    public void OnTeamButtonkPressed()
    {
        Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int id = System.Array.IndexOf(teamID, button.gameObject);

        StrapiUserTeam team = teams[id].attributes;
        teamPanel.transform.GetChild(0).GetComponent<Text>().text = "Team name: " + team.teamname;
        teamPanel.transform.GetChild(1).GetComponent<Text>().text = "Creator: " + team.creator;
        teamPanel.transform.GetChild(2).GetComponent<Text>().text = "Score: " + team.teamscore.ToString();
        teamPanel.transform.GetChild(3).GetComponent<Text>().text = "Players: " + team.numplayers.ToString();
        teamPanel.transform.GetChild(4).GetComponent<Text>().text = "Joined: " + team.CreatedAt();
        teamPanel.SetActive(true);
    }

    private IEnumerator GetTeamsCoroutine()
    {
        yield return StartCoroutine(StrapiComponent._instance.GetListOfTeamsFromServerCoroutine());
        yield return StartCoroutine(StrapiComponent._instance.GetListOfUsersFromServerCoroutine("api/users?group=none"));
        OnStarted?.Invoke();
    }

    public void OnBackPressed()
    {
        MySceneManager.instance.LoadScene("MainMenu");
    }

    public void OnClosePressed()
    {
        if (teamPanel.activeSelf)
        {
            teamPanel.SetActive(false);
        }
        if (newTeamPanelOne.activeSelf)
        {
            newTeamPanelOne.SetActive(false);
        }
        if (newTeamPanelTwo.activeSelf)
        {
            newTeamPanelTwo.SetActive(false);
        }
    }

    public void OnNewTeamPressed()
    {
        newTeamPanelOne.SetActive(true);
        newTeamPanelOne.transform.GetChild(4).gameObject.SetActive(false);
    }

    public void OnDeletePressed()
    {
        Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int id = System.Array.IndexOf(teamID, button.gameObject);
        StrapiComponent._instance.DeleteGroup(int.Parse(teams[id].id));
        MySceneManager.instance.LoadScene("MainMenu");
    }
    public void OnDeleteMultiplePressed()
    {
        for (int i = 0; i < teamID.Length; i++)
        {
            if (teamID[i].transform.GetChild(2).GetComponent<Toggle>().isOn)
            {
                int id = System.Array.IndexOf(teamID, teamID[i]);
                StrapiComponent._instance.DeleteGroup(int.Parse(teams[id].id));
            }
        }
        MySceneManager.instance.LoadScene("MainMenu");
    }

    public void OnNextPressed()
    {
        if (newTeamPanelOne.transform.GetChild(1).GetComponent<InputField>().text != "")
        {
            newTeamName = newTeamPanelOne.transform.GetChild(1).GetComponent<InputField>().text;
            newTeamPanelOne.SetActive(false);
            newTeamPanelTwo.SetActive(true);

            users = StrapiComponent._instance.GetUsers();
            userID = new GameObject[users.Length];

            for (int i = 0; i < users.Length; i++)
            {
                GameObject user = Instantiate(userPrefab, new Vector3(0, usersViewport.transform.position.y - 50 * i, 0), Quaternion.identity, usersViewport.transform);
                user.transform.GetChild(0).GetComponent<Text>().text = users[i].username;
                user.transform.GetChild(1).GetComponent<Text>().text = users[i].firstname;
                userID[i] = user;
            }
        }
        else
        {
            newTeamPanelOne.transform.GetChild(4).gameObject.SetActive(true);
        }        
    }

    public void OnSubmitTeamPressed()
    {
        List<int> playersID = new List<int>();
        for (int i = 0; i < userID.Length; i++)
        {
            if (userID[i].transform.GetChild(2).GetComponent<Toggle>().isOn)
            {
                playersID.Add(users[i].id);
            }
        }

        StrapiComponent._instance.CreateStrapiUserTeam(newTeamName, playersID);
        MySceneManager.instance.LoadScene("MainMenu");
    }
}
