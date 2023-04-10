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
    public GameObject editTeamPlayers;
    public GameObject addTeamPlayers;

    public GameObject deleteGroupsButton;

    public GameObject usersViewport;

    private StrapiTeamsData[] teams = null;
    private List<GameObject> teamID = null;

    public event Action OnStarted = delegate { };
    public event Action OnPlayersRetrieved = delegate { };
    public event Action OnFreePlayersRetrieved = delegate { };

    private string newTeamName = "";
    private int currentTeamIndex = -1;

    private StrapiUser[] users = null;
    private GameObject[] userID = null;
    private StrapiUserTeam team;
    private List<StrapiUser> freeUsers = null;

    void Start()
    {
        OnStarted += BuildUserScreen;
        OnPlayersRetrieved += BuildTeamMembersList;
        OnFreePlayersRetrieved += BuildFreePlayerList;

        StartCoroutine(GetTeamsCoroutine());
    }

    private void Update()
    {
        if (teamID != null)
            if (!deleteGroupsButton.activeSelf)
            {
                for (int i = 0; i < teamID.Count; i++)
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
        teamID = new List<GameObject>();

        for (int i = 0; i < teams.Length; i++)
        {
            StrapiUserTeam team = teams[i].attributes;
            GameObject teamData = Instantiate(groupPrefab, new Vector3(0, viewport.transform.position.y - 50 * i, 0), Quaternion.identity, viewport.transform);
            teamData.transform.GetChild(0).GetComponent<Text>().text = team.teamname;
            teamData.transform.GetChild(1).GetComponent<Text>().text = team.numplayers.ToString();

            teamData.GetComponent<Button>().onClick.AddListener(() => OnTeamButtonkPressed());
            teamID.Add(teamData);
        }

        teamPanel.SetActive(false);
        newTeamPanelOne.SetActive(false);
        newTeamPanelTwo.SetActive(false);

        editTeamPlayers.SetActive(false);
        addTeamPlayers.SetActive(false);
        deleteGroupsButton.SetActive(false);
}

    public void OnTeamButtonkPressed()
    {
        Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        currentTeamIndex= teamID.IndexOf(button.gameObject);

        StrapiUserTeam team = teams[currentTeamIndex].attributes;
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
        yield return StartCoroutine(StrapiComponent._instance.GetListOfUsersFromServerCoroutine("api/users"));
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
        if (editTeamPlayers.activeSelf)
        {
            editTeamPlayers.SetActive(false);

            for (int i = 0; i < userID.Length; i++)
                Destroy(userID[i]);
            userID = null;
        }
        if (addTeamPlayers.activeSelf)
        {
            addTeamPlayers.SetActive(false);
            for (int i = 0; i < userID.Length; i++)
                Destroy(userID[i]);
            userID = null;
        }
    }

    private IEnumerator TeamMembersCoroutine(int id)
    {
        yield return StartCoroutine(StrapiComponent._instance.GetStrapiUserTeamData(id));
    }

    public void OnEditPlayersPressed()
    {
        StartCoroutine(OnEditPlayersPressedCoroutine());
    }
    private IEnumerator OnEditPlayersPressedCoroutine()
    {
        editTeamPlayers.SetActive(true);
        teamPanel.SetActive(false);

        yield return StartCoroutine(TeamMembersCoroutine(int.Parse(teams[currentTeamIndex].id)));
        OnPlayersRetrieved?.Invoke();
    }

    public void OnAddPlayersPressed()
    {
        addTeamPlayers.SetActive(true);
        teamPanel.SetActive(false);
        BuildFreePlayerList();
    }

    public void BuildTeamMembersList()
    {
        team = StrapiComponent._instance.GetGroup();
        userID = new GameObject[team.members.data.Length];
        for (int i = 0; i < team.members.data.Length; i++)
        {
            GameObject user = Instantiate(userPrefab, new Vector3(0, usersViewport.transform.position.y - 50 * i, 0), Quaternion.identity, usersViewport.transform);
            user.transform.GetChild(0).GetComponent<Text>().text = team.members.data[i].attributes.username;
            user.transform.GetChild(1).GetComponent<Text>().text = team.members.data[i].attributes.firstname;
            user.transform.parent = editTeamPlayers.transform.GetChild(2).GetChild(0).GetChild(0);
            userID[i] = user;
        }
    }

    public void BuildFreePlayerList()
    {
        freeUsers = new List<StrapiUser>();
        users = StrapiComponent._instance.GetUsers();

        for (int i = 0; i < users.Length; i++)
        {
            if (users[i].group == "None")
            {
                freeUsers.Add(users[i]);
            }
        }

        userID = new GameObject[freeUsers.Count];
        for (int i = 0; i < freeUsers.Count; i++)
        {
            GameObject user = Instantiate(userPrefab, new Vector3(0, usersViewport.transform.position.y - 50 * i, 0), Quaternion.identity, usersViewport.transform);
            user.transform.GetChild(0).GetComponent<Text>().text = freeUsers[i].username;
            user.transform.GetChild(1).GetComponent<Text>().text = freeUsers[i].firstname;
            user.transform.parent = addTeamPlayers.transform.GetChild(3).GetChild(0).GetChild(0);
            userID[i] = user;
        }
    }

    public void OnDeleteMembersPressed()
    {
        List<int> deletedPlayersId = new List<int>();
        for (int i = 0; i < userID.Length; i++)
        {
            if (userID[i].transform.GetChild(2).GetComponent<Toggle>().isOn)
            {
                Debug.Log("elegido");
                Debug.Log(team.members.data[i].id);
                deletedPlayersId.Add(team.members.data[i].id);
            }
        }
        int teamID = int.Parse(teams[currentTeamIndex].id);
        Debug.Log("deletedPlayersId " + deletedPlayersId.Count);
        StrapiComponent._instance.UpdateStrapiUserTeam(teamID, deletedPlayersId, false);
        MySceneManager.instance.LoadScene("MainMenu");
    }

    public void OnAddMembersPressed()
    {
        List<int> addedPlayersID = new List<int>();
        for (int i = 0; i < userID.Length; i++)
        {
            if (userID[i].transform.GetChild(2).GetComponent<Toggle>().isOn)
            {
                addedPlayersID.Add(freeUsers[i].id);
            }
        }
        int teamID = int.Parse(teams[currentTeamIndex].id);
        StrapiComponent._instance.UpdateStrapiUserTeam(teamID, addedPlayersID, true);
        MySceneManager.instance.LoadScene("MainMenu");
    }

    public void OnNewTeamPressed()
    {
        newTeamPanelOne.SetActive(true);
        newTeamPanelOne.transform.GetChild(4).gameObject.SetActive(false);
    }

    public void OnDeletePressed()
    {
        StrapiComponent._instance.DeleteGroup(int.Parse(teams[currentTeamIndex].id));
        MySceneManager.instance.LoadScene("MainMenu");
    }
    public void OnDeleteMultiplePressed()
    {
        for (int i = 0; i < teamID.Count; i++)
        {
            if (teamID[i].transform.GetChild(2).GetComponent<Toggle>().isOn)
            {
                StrapiComponent._instance.DeleteGroup(int.Parse(teams[i].id));
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
                Debug.Log(users[i].id);
                playersID.Add(users[i].id);
            }
        }

        StrapiComponent._instance.CreateStrapiUserTeam(newTeamName, playersID);
        MySceneManager.instance.LoadScene("MainMenu");
    }
}
