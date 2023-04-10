using System.Collections;
using System.Collections.Generic;
using StrapiForUnity;
using UnityEngine;
using UnityEngine.UI;

public class ProfileMenu : MonoBehaviour
{
    public GameObject editProfileMenu;

    public InputField EditUsernameInput;
    public InputField EditEmailInput;
    public InputField EditNameInput;
    public InputField EditSurnameInput;

    public Text headerText;

    void Start()
    {
        editProfileMenu.SetActive(false);

        headerText.text = "Profile";

        EditUsernameInput.GetComponent<EditProfilePlaceholderText>().ShowText();
        EditEmailInput.GetComponent<EditProfilePlaceholderText>().ShowText();
        EditNameInput.GetComponent<EditProfilePlaceholderText>().ShowText();
        EditSurnameInput.GetComponent<EditProfilePlaceholderText>().ShowText();
    }

    public void EditProfile()
    {
        editProfileMenu.SetActive(true);
        
        EditUsernameInput.GetComponent<EditProfilePlaceholderText>().ShowText();
        EditEmailInput.GetComponent<EditProfilePlaceholderText>().ShowText();
        EditNameInput.GetComponent<EditProfilePlaceholderText>().ShowText();
        EditSurnameInput.GetComponent<EditProfilePlaceholderText>().ShowText();
    }

    public void deleteAccount()
    {
        headerText.text = "Succesfully deleted account. App will now close to submit changes.";
        StrapiComponent._instance.DeleteAccount();

        Invoke("CloseApp", 3.0f);
    }

    public void submitRequest()
    {
        StrapiComponent._instance.EditProfile(EditUsernameInput.text, EditEmailInput.text, EditNameInput.text, EditSurnameInput.text);

        headerText.text = "Profile updated";
        editProfileMenu.SetActive(false);
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void backToMainMenu()
    {
        MySceneManager.instance.LoadScene("MainMenu");
    }
}
