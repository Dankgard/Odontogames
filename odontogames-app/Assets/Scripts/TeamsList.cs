using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using StrapiForUnity;

public class TeamsList : MonoBehaviour
{
    public GameObject groupPrefab;
    public GameObject viewport;

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

            teamID[i] = teamData;
        }
    }

    private IEnumerator GetTeamsCoroutine()
    {
        yield return StartCoroutine(StrapiComponent._instance.GetListOfTeamsFromServerCoroutine());
        OnStarted?.Invoke();
    }

    public void OnBackPressed()
    {
        SceneHandler.instance.LoadScene("MainMenu");
    }
}
