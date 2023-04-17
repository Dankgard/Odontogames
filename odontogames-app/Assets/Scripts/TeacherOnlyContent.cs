using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherOnlyContent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!StrapiComponent._instance.UserIsTeacher())
        {

            gameObject.SetActive(false);
        }
    }
}
