using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelManager : MonoBehaviour
{
    public int numBlocks = 1;
    public int numLevels = 7;

    public GameObject levelButtonPrefab;
    public Transform scrollView;


    private void Start()
    {
        //levelButtonPrefab.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelButtonPrefab.GetComponent<LoadEscapeRoomGame>().GetLevel()));

        //GameObject button = null;
        //// Iterate through the number of levels and create a button for each one
        //for (int i = 0; i < numBlocks; i++)
        //{
        //    for (int j = 0; j < numLevels; j++)
        //    {
        //        button = Instantiate(levelButtonPrefab, scrollView);

        //        string level = "escaperoom" + (i + 1) + "_" + (j + 1);
        //        button.transform.GetComponent<LoadEscapeRoomGame>().SetLevel(level);
        //    }
        //} 
            
    }

    public void LoadLevel(string level)
    {
        SceneHandler.instance.LoadScene(level);
    }

}
