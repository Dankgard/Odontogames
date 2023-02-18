using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditProfilePlaceholderText : MonoBehaviour
{
    public string dataName;

    public void ShowText()
    {
        switch (dataName)
        {
            case "Username":
                GetComponent<InputField>().text = StrapiComponent._instance.GetUsername();
                break;
            case "Email":
                GetComponent<InputField>().text = StrapiComponent._instance.GetEmail();
                break;
            case "Password":
                GetComponent<InputField>().text = StrapiComponent._instance.GetPassword();
                break;
            case "FirstName":
                GetComponent<InputField>().text = StrapiComponent._instance.GetName();
                break;
            case "LastName":
                GetComponent<InputField>().text = StrapiComponent._instance.GetSurname();
                break;
            default:
                break;
        }
    }
}
