using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_6_GM : MonoBehaviour
{
    private bool movingLetters;

    private void Start()
    {
        movingLetters = false;
    }

    public void MovingLetters()
    {
        movingLetters = !movingLetters;
    }

    public bool isMovingLetters()
    {
        return movingLetters;
    }
}
