using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class EscapeRoom1_1_AnswerBox : MonoBehaviour
{
    public string name;
    public GameObject platform;
    public int numImages;

    private bool acceptingImages;
    private bool platformMoving;

    void Start()
    {
        string category = name.Substring(0, 1).ToUpper() + name.Substring(1);
        if (Regex.IsMatch(name.Substring(name.Length - 1), "[bcdfghjklmnpqrstvwxyz]"))
        {
            category += "es";
        }
        else
        {
            category += "s";
        }

        transform.GetChild(0).GetChild(0).GetComponent<TextMesh>().text = category;
        acceptingImages = true;
    }

    void Update()
    {
        if (numImages <= 0 && acceptingImages)
        {
            transform.GetChild(2).gameObject.SetActive(true);
            acceptingImages = false;
            platform.GetComponent<escaperoom1_1_platform>().PlatformFinished();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (acceptingImages)
        {
            if (other.gameObject.tag == "image")
            {
                platform.GetComponent<escaperoom1_1_platform>().UpdatePlatform(2);
                platformMoving = true;
                numImages--;
            }
        }
    }

    public bool IsPlatformMoving()
    {
        if (platformMoving)
        {
            platformMoving = !platformMoving;
            return !platformMoving;
        }
        else return platformMoving;
    }

    public string GetPlatformName()
    {
        return name;
    }
}
