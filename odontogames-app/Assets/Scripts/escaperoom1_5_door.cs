using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_5_door : MonoBehaviour
{
    GameObject camera;
    escaperoom1_5_player player;
    public string answer;

    void Awake()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        player = camera.GetComponent<escaperoom1_5_player>();
    }

    void OnMouseDown()
    {
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z + 40);
        player.ChooseAnswer(answer);
    }
}
