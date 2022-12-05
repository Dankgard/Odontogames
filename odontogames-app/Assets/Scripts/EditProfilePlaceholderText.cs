using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditProfilePlaceholderText : MonoBehaviour
{
    public StrapiComponent strapiComponent;
    public string dataName;

    public void ShowText()
    {
        switch (dataName)
        {
            case "Username":
                GetComponent<InputField>().text = strapiComponent.GetUsername();
                break;
            case "Email":
                GetComponent<InputField>().text = strapiComponent.GetEmail();
                break;
            case "Password":
                GetComponent<InputField>().text = strapiComponent.GetPassword();
                break;
            case "Name":
                GetComponent<InputField>().text = strapiComponent.GetName();
                break;
            case "Surname":
                GetComponent<InputField>().text = strapiComponent.GetSurname();
                break;
            default:
                break;
        }
    }
}
