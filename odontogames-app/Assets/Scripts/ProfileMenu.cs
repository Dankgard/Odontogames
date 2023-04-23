using System.Collections;
using System.Collections.Generic;
using StrapiForUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileMenu : MonoBehaviour
{
    public GameObject editProfileMenu;
    public GameObject scoresMenu;

    public InputField EditUsernameInput;
    public InputField EditEmailInput;
    public InputField EditNameInput;
    public InputField EditSurnameInput;

    public Text headerText;

    void Start()
    {
        editProfileMenu.SetActive(false);
        scoresMenu.SetActive(false);

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

    public void OnCancelPressed()
    {
        if (editProfileMenu.activeSelf)
            editProfileMenu.SetActive(false);
        if (scoresMenu.activeSelf)
            scoresMenu.SetActive(false);
    }

    public void OnCheckScoresPressed()
    {
        GameObject scores = scoresMenu.transform.GetChild(2).GetChild(1).gameObject;

        int score = StrapiComponent._instance.GetCurrentUser().firstgamescore;
        scores.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Minijuego 1: ";
        scores.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = score <= 0 ? Color.red : Color.green;
        scores.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += score.ToString() + " puntos";

        score = StrapiComponent._instance.GetCurrentUser().secondgamescore;
        scores.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Minijuego 2: ";
        scores.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = score <= 0 ? Color.red : Color.green;
        scores.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += score.ToString() + " puntos";

        score = StrapiComponent._instance.GetCurrentUser().thirdgamescore;
        scores.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Minijuego 3: ";
        scores.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = score <= 0 ? Color.red : Color.green;
        scores.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text += score.ToString() + " puntos";

        score = StrapiComponent._instance.GetCurrentUser().fourthgamescore;
        scores.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Minijuego 4: ";
        scores.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = score <= 0 ? Color.red : Color.green;
        scores.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text += score.ToString() + " puntos";

        score = StrapiComponent._instance.GetCurrentUser().fifthgamescore;
        scores.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Minijuego 5: ";
        scores.transform.GetChild(4).GetComponent<TextMeshProUGUI>().color = score <= 0 ? Color.red : Color.green;
        scores.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text += score.ToString() + " puntos";

        score = StrapiComponent._instance.GetCurrentUser().sixthgamescore;
        scores.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Minijuego 6: ";
        scores.transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = score <= 0 ? Color.red : Color.green;
        scores.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text += score.ToString() + " puntos";

        score = StrapiComponent._instance.GetCurrentUser().seventhgamescore;
        scores.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Minijuego 7: ";
        scores.transform.GetChild(6).GetComponent<TextMeshProUGUI>().color = score <= 0 ? Color.red : Color.green;
        scores.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text += score.ToString() + " puntos";

        scoresMenu.SetActive(true);
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
