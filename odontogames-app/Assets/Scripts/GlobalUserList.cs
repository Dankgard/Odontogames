using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StrapiForUnity;
using UnityEngine.UI;

public class GlobalUserList : MonoBehaviour
{
    public GameObject userPrefab;
    public StrapiComponent strapiComponent;
    
    void Start()
    {
        StrapiUser[] users = strapiComponent.GetUsersRequest("/api/users");

        for(int i=0;i<users.GetLength(0);i++)
        {
            GameObject user = Instantiate(userPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - i), gameObject.transform.rotation, gameObject.transform);
            user.transform.GetChild(0).GetComponent<Text>().text = users[i].username;
            user.transform.GetChild(1).GetComponent<Text>().text = users[i].name;
        }   

    }   
}
