using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StrapiForUnity;
using UnityEngine.UI;

public class GlobalUserList : MonoBehaviour
{
    public GameObject userPrefab;

    private StrapiUser[] users = null;
    private GameObject[] userID;

    void Start()
    {
        users = new StrapiUser[40];
        userID = new GameObject[40];

        //StrapiComponent._instance.GetUsersRequest("api/users");
        //StartCoroutine(waiter());

        users = StrapiComponent._instance.getUsers();

        for (int i = 0; i < users.Length; i++)
        {
            GameObject user = Instantiate(userPrefab, new Vector3(this.transform.position.x, this.transform.position.y - 40 * i, this.transform.position.z), this.transform.rotation, this.transform);
            user.transform.GetChild(0).GetComponent<Text>().text = users[i].username;
            user.transform.GetChild(1).GetComponent<Text>().text = users[i].Firstname;
            userID[i] = user;
        }
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

        return users;
    }

    public void OnBackPressed()
    {
        SceneHandler.instance.LoadScene("MainMenu");
    }

    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(5);
    }
}
