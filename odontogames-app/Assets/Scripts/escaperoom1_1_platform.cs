using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_1_platform : MonoBehaviour
{
    public int requiredImages;
    int images = 0;

    void OnCollisionEnter(Collision col)
    {
        if(gameObject.tag == col.gameObject.tag)
        {
            Destroy(col.gameObject);
            images++;
            if(images == requiredImages)
            {
                GrowPlatform();
            }
        }
    }

    void GrowPlatform()
    {
        transform.localScale = new Vector3(4, 0.2f, 1);
        GameObject.FindGameObjectWithTag("robot").GetComponent<escaperoom1_1_robot>().AddCompletedPlatform();
    }
}
