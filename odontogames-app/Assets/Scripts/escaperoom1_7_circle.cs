using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_7_circle : MonoBehaviour
{
    public GameObject letter;
    int numberOfLetters = 26;
    public float radius = 5f;
    string circleLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    GameObject[] letters;

    int activeLetter = 0;

    public Material[] colors;

    void Awake()
    {
        letters = new GameObject[27];
        SpawnCircle();
    }

    void SpawnCircle()
    {
        float angleIncrement = 360f / numberOfLetters;

        for(int i=0;i<numberOfLetters;i++)
        {
            float angle = (270f + i * angleIncrement) * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            letters[i] = Instantiate(letter, transform.position + position, Quaternion.identity);
            letters[i].transform.SetParent(transform);
            letters[i].transform.GetChild(1).GetComponent<TextMesh>().text = circleLetters[i].ToString();
        }

        transform.Rotate(Vector3.right, 90f);
        LoadLetterMaterial(1);
    }

    void LoadLetterMaterial(int color)
    {
        letters[activeLetter].transform.GetChild(0).GetComponent<Renderer>().material = colors[color];
    }
}
