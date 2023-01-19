using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadEscapeRoomGame : MonoBehaviour
{
    public int escapeRoom;
    public int game;

    public void LoadGame()
    {
       SceneHandler.instance.LoadScene("escaperoom" + escapeRoom + "_" + game);
    }
}
