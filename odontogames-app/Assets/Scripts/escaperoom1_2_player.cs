using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class escaperoom1_2_player : MonoBehaviour
{
    private bool finishedGame = false;

    public Texture[] questions;
    public GameObject[] answers;

    public Texture[] textures;

    private int currentQuestion;

    void Start()
    {
        currentQuestion = 1;

        if (answers.Length != textures.Length || questions.Length != textures.Length || questions.Length != answers.Length)
        {
            Debug.LogError("all arrays must have the same lenght");
            return;
        }

        // here we create a list of indexes and randomize them
        List<int> indexes = new List<int>();
        for (int i = 0; i < answers.Length; i++)
        {
            indexes.Add(i);
        }
        indexes = Shuffle(indexes);

        // using the randomized indexes, we assign the textures at random
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].GetComponent<Renderer>().material.mainTexture = textures[indexes[i]];
            answers[i].tag = "image" + (i + 1).ToString();
        }

        // We set the player's texture using our index named currentQuestion with the 'questions' texture array
        GetComponent<Renderer>().material.mainTexture = questions[currentQuestion];
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "image" + currentQuestion)
        {
            currentQuestion++;
            Destroy(col.gameObject);

            // We update the player's texture
            GetComponent<Renderer>().material.mainTexture = questions[currentQuestion];

            if (currentQuestion == 11)
            {
                finishedGame = true;
                MySceneManager.instance.LoadScene("MinigameEnd");
            }
                
        }
        else if (col.gameObject.tag != "wall")
        {
            // penalizar al jugador porque se equivoco de imagen
        }
    }

    // auxiliar method to randomize the list of indexes
    private List<int> Shuffle(List<int> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
}
