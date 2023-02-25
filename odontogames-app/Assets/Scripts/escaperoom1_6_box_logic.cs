using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_6_box_logic : MonoBehaviour
{
    private bool notMoving;
    private string letter;

    void Awake()
    {
        notMoving = false;
        letter = " ";
    }

    void OnTriggerEnter(Collider collision)
    {
        //Debug.Log(notMoving);
        //if (notMoving)
        //{
        //    Debug.Log("A acomodar");
            collision.gameObject.transform.position = transform.position;
            letter = collision.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text;
            notMoving = false;
        //}
    }

    public void LettersMoved()
    {
        notMoving = true;
        Debug.Log("Ya se mueve " + notMoving);
    }

    public string GetLetter()
    {
        return letter;
    }
}
