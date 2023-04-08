using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using StrapiForUnity;

public class TeamsList : MonoBehaviour
{
    public GameObject groupPrefab;
    public GameObject viewport;

    public GameObject teamsPanel;
    public GameObject newTeamPanel;

    private StrapiTeamsData[] teams = null;
    private GameObject[] teamID = null;

    public event Action OnStarted = delegate { };

    void Start()
    {
        OnStarted += BuildUserScreen;

        StartCoroutine(GetTeamsCoroutine());
    }

    public void BuildUserScreen()
    {
        teams = StrapiComponent._instance.GetTeams();
        Debug.Log(teams.Length);
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
    }

    public void OnTeamButtonkPressed()
    {
        Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int id = System.Array.IndexOf(teamID, button.gameObject);

        StrapiUserTeam team = teams[id].attributes;
        teamsPanel.transform.GetChild(0).GetComponent<Text>().text = "Team name: " + team.teamname;
        teamsPanel.transform.GetChild(1).GetComponent<Text>().text = "Creator: " + team.creator;
        teamsPanel.transform.GetChild(2).GetComponent<Text>().text = "Score: " + team.teamscore.ToString();
        teamsPanel.transform.GetChild(3).GetComponent<Text>().text = "Players: " + team.numplayers.ToString();
        teamsPanel.transform.GetChild(4).GetComponent<Text>().text = "Joined: " + team.CreatedAt();
        teamsPanel.SetActive(true);
    }

    public void OnClosePressed()
    {
        teamsPanel.SetActive(false);
    }

    private IEnumerator GetTeamsCoroutine()
    {
        yield return StartCoroutine(StrapiComponent._instance.GetListOfTeamsFromServerCoroutine());
        OnStarted?.Invoke();
    }

    public void OnBackPressed()
    {
        MySceneManager.instance.LoadScene("MainMenu");
    }

    public void OnNewTeamPressed()
    {
        newTeamPanel.SetActive(false);
    }
}
