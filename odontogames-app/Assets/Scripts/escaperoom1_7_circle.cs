using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_7_circle : MonoBehaviour
{
    public GameObject letter;
    int letters = 27;
    public float radius = 5f;

    void Awake()
    {
        SpawnCircle();
    }

    void SpawnCircle()
    {
        float angleIncrement = 360f / letters;

        for(int i=0;i<letters;i++)
        {
            float angle = i * angleIncrement * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            GameObject spawnedLetter = Instantiate(letter, transform.position + position, Quaternion.identity);
            spawnedLetter.transform.SetParent(transform);
        }

        transform.Rotate(Vector3.right, 90f);
    }
}
