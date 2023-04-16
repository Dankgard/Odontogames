using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class escaperoom1_2_player : MonoBehaviour
{
    private bool finishedGame = false;

    public Light[] lights;
    public Material material;

    public Texture[] questions;
    public GameObject[] answers;

    public Texture[] textures;

    private int currentQuestion = 0;
    private int score = 0;

    private List<int> indexes;

    private void Start()
    {
        if (answers.Length != textures.Length || questions.Length != textures.Length || questions.Length != answers.Length)
        {
            Debug.LogError("all arrays must have the same lenght");
            return;
        }

        // here we create a list of indexes and randomize them
        indexes = new List<int>();
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

    private void Update()
    {
        if (currentQuestion == questions.Length && !finishedGame)
        {
            finishedGame = true;
            EndGame();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "image" + (currentQuestion + 1).ToString())
        {
            // We update the player's texture
            GetComponent<Renderer>().material.mainTexture = questions[currentQuestion];
            currentQuestion++;
            score++;
            TurnLightOff();
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag != "wall")
        {
            // penalizar al jugador porque se equivoco de imagen
            score--;
        }
    }

    // auxiliar method to randomize the list of indexes
    private List<int> Shuffle(List<int> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    private List<int> RandomizeList(List<int> list)
    {
        List<int> randomizedList = new List<int>();
        System.Random random = new System.Random();

        while (list.Count > 0)
        {
            int index = random.Next(0, list.Count);
            randomizedList.Add(list[index]);
            list.RemoveAt(index);
        }

        return randomizedList;
    }

    private void TurnLightOff()
    {
        if (lights.Length > 0)
        {
            if (indexes == null || indexes.Count == 0)
            {
                indexes = new List<int>();

                for (int i = 0; i < lights.Length; i++)
                {
                    indexes.Add(i);
                }

                indexes = RandomizeList(indexes);
            }

            int randomIndex = indexes[0];
            indexes.RemoveAt(0);

            StartCoroutine(TurnLightOffCoroutine(randomIndex));
        }
    }

    private IEnumerator TurnLightOffCoroutine(int index)
    {
        Vector3 playerPos = transform.position;
        CamerasManager.camerasManagerInstance.SwapCamera(1);
        GetComponent<DragWithMouse>().StopMovement();
        yield return new WaitForSeconds(1f);
        lights[index].color = Color.green;
        lights[index].GetComponent<Renderer>().material = material;
        yield return new WaitForSeconds(1.5f);
        CamerasManager.camerasManagerInstance.SwapCamera(0);
        transform.position = playerPos;
    }

    private void EndGame()
    {
        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {
        CamerasManager.camerasManagerInstance.SwapCamera(1);
        //SoundManager.instance.PlaySound(2);
        //StrapiComponent._instance.UpdatePlayerScore(score);
        yield return new WaitForSeconds(1.5f);
        MySceneManager.instance.LoadScene("MinigameEnd");
    }
}
