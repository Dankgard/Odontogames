using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using StrapiForUnity;

public class TeamsList : MonoBehaviour
{
    public GameObject userPrefab;

    private StrapiGroup[] groups = null;

    void Start()
    {
        groups = new StrapiGroup[40];

        //StrapiComponent._instance.GetGroupsRequest("api/users-permissions/roles");
        //StartCoroutine(waiter());

        groups = StrapiComponent._instance.GetGroups();
        Debug.Log(groups);

        for (int i = 2; i < groups.Length; i++)
        {
            GameObject group = Instantiate(userPrefab, new Vector3(this.transform.position.x, this.transform.position.y - 40 * i, this.transform.position.z), this.transform.rotation, this.transform);
            group.transform.GetChild(0).GetComponent<Text>().text = groups[i].name;
            group.transform.GetChild(1).GetComponent<Text>().text = groups[i].nb_users.ToString();
        }
    }

    public void OnBackPressed()
    {
        MySceneManager.instance.LoadScene("MainMenu");
    }

    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(5);
    }
}
