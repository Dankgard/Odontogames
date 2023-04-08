using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using StrapiForUnity;
using UnityEngine.UI;

public class GlobalUserList : MonoBehaviour
{
    public GameObject userPrefab;
    public GameObject viewport;

    public GameObject userPanel;

    private StrapiUser[] users = null;
    private GameObject[] userID = null;

    public event Action OnStarted = delegate { };

    void Start()
    {
        OnStarted += BuildUserScreen;

        StartCoroutine(GetUsersCoroutine());
    }

    public StrapiUser[] GetSelectedUsers()
    {
        int count = 0;
        //for (int i = 0; i < userID.Length; i++)
        //{
        //    if (userID[i].transform.GetChild(2).GetComponent<Toggle>().isOn)
        //        count++;
        //}

        //Debug.Log(count);

        //StrapiUser[] selectedUsers = new StrapiUser[count];
        //int index = 0;
        //for (int i = 0; i < userID.Length; i++)
        //{
        //    if (userID[i].transform.GetChild(2) != null)
        //    {
        //        selectedUsers[index] = users[i];
        //        index++;
        //    }
        //}

        return null;
    }

    public void BuildUserScreen()
    {
        userPanel.SetActive(false);
        users = StrapiComponent._instance.GetUsers();
        Debug.Log(users.Length);
        userID = new GameObject[users.Length];

        for (int i = 0; i < users.Length; i++)
        {
            GameObject user = Instantiate(userPrefab, new Vector3(0, viewport.transform.position.y - 50 * i, 0), Quaternion.identity, viewport.transform);
            user.transform.GetChild(0).GetComponent<Text>().text = users[i].username;
            user.transform.GetChild(1).GetComponent<Text>().text = users[i].firstname;
            user.GetComponent<Button>().onClick.AddListener(() => OnUserButtonkPressed());

            userID[i] = user;
        }
    }

    public void OnUserButtonkPressed()
    {
        Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int id = System.Array.IndexOf(userID, button.gameObject);

        userPanel.transform.GetChild(0).GetComponent<Text>().text = "Username: " + users[id].username;
        userPanel.transform.GetChild(1).GetComponent<Text>().text = "Name: " + users[id].firstname + " " + users[id].lastname;
        userPanel.transform.GetChild(2).GetComponent<Text>().text = "Email: " + users[id].email;
        userPanel.transform.GetChild(3).GetComponent<Text>().text = "Team: " + users[id].group;
        userPanel.transform.GetChild(4).GetComponent<Text>().text = "Joined: " + users[id].CreatedAt();
        userPanel.SetActive(true);
    }

    public void OnBackPressed()
    {
        userPanel.SetActive(false);
    }

    public void OnClosePressed()
    {
        MySceneManager.instance.LoadScene("MainMenu");
    }

    private IEnumerator GetUsersCoroutine()
    {
        yield return StartCoroutine(StrapiComponent._instance.GetListOfUsersFromServerCoroutine());
        OnStarted?.Invoke();
    }
}
