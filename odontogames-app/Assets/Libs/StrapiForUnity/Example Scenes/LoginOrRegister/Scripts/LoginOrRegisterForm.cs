using System;
using System.Collections;
using System.Collections.Generic;
using StrapiForUnity;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoginOrRegisterForm : MonoBehaviour {
    public Text HeaderText;
    public VerticalLayoutGroup ContainerLayout;
    public GameObject LoadingObject;

    // Elementos para iniciar sesion
    public Button LoginSubmitButton;
    public Button LoginToggleButton;

    // Elementos para registrar usuario
    public Button RegisterToggleButton;
    public Button RegisterSubmitButton;
    public InputField UsernameInput;
    public InputField PasswordInput;
    public InputField EmailInput;
    public InputField nameInput;
    public InputField surnameInput;

    // Elementos para eliminar una cuenta
    public Button DeleteAccountButton;

    // Elementos para editar el perfil del usuario
    public Button EditProfileButton;
    public InputField EditUsernameInput;
    public InputField EditEmailInput;
    public InputField EditPasswordInput;
    public InputField EditNameInput;
    public InputField EditSurnameInput;
    public Button SubmitEditRequest;
    
    public StrapiComponent Strapi;

    private bool isLoading = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Strapi == null)
        {
            Debug.LogError("No Strapi component found. Please make sure you've got an active Strapi component assigned to the LoginOrRegisterForm");
            return;
        }
        
        LoginToggleButton.onClick.Invoke();
        LoadingObject.SetActive(false);
        
        registerEventHandlers();
    }

    private void registerEventHandlers()
    {
        Strapi.OnAuthSuccess += handleSuccessfulAuthentication;
        Strapi.OnAuthFail += handleUnsuccessfulAuthentication;
        Strapi.OnAuthStarted += toggleLoading;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (LoginSubmitButton.IsActive())
            {
                LoginSubmitButton.onClick.Invoke();
            }
            else if (RegisterSubmitButton.IsActive())
            {
                RegisterSubmitButton.onClick.Invoke();
            }
            else if (DeleteAccountButton.IsActive())
            {
                DeleteAccountButton.onClick.Invoke();
            }
            else if (EditProfileButton.IsActive())
            {
                EditProfileButton.onClick.Invoke();
            }
            else if (SubmitEditRequest.IsActive())
            {
                SubmitEditRequest.onClick.Invoke();
            }
        }
    }

    public void OnLoginToggle()
    {
        RegisterSubmitButton.gameObject.SetActive(false);
        LoginSubmitButton.gameObject.SetActive(true);
        EmailInput.transform.parent.gameObject.SetActive(false);
        RegisterToggleButton.gameObject.SetActive(true);
        nameInput.transform.parent.gameObject.SetActive(false);
        surnameInput.transform.parent.gameObject.SetActive(false);
        LoginToggleButton.gameObject.SetActive(false);
        DeleteAccountButton.gameObject.SetActive(false);

        EditProfileButton.gameObject.SetActive(false);
        SubmitEditRequest.gameObject.SetActive(false);
        EditUsernameInput.transform.parent.gameObject.SetActive(false);
        EditEmailInput.transform.parent.gameObject.SetActive(false);
        EditPasswordInput.transform.parent.gameObject.SetActive(false);
        EditNameInput.transform.parent.gameObject.SetActive(false);
        EditSurnameInput.transform.parent.gameObject.SetActive(false);

        HeaderText.text = "Login";
        
        forceLayoutUpdate();
    }

    public void OnRegisterToggle()
    {
        RegisterSubmitButton.gameObject.SetActive(true);
        LoginSubmitButton.gameObject.SetActive(false);
        EmailInput.transform.parent.gameObject.SetActive(true);
        nameInput.transform.parent.gameObject.SetActive(true);
        surnameInput.transform.parent.gameObject.SetActive(true);
        RegisterToggleButton.gameObject.SetActive(false);
        LoginToggleButton.gameObject.SetActive(true);
        DeleteAccountButton.gameObject.SetActive(false);

        EditProfileButton.gameObject.SetActive(false);
        SubmitEditRequest.gameObject.SetActive(false);
        EditUsernameInput.transform.parent.gameObject.SetActive(false);
        EditEmailInput.transform.parent.gameObject.SetActive(false);
        EditPasswordInput.transform.parent.gameObject.SetActive(false);
        EditNameInput.transform.parent.gameObject.SetActive(false);
        EditSurnameInput.transform.parent.gameObject.SetActive(false);

        HeaderText.text = "Register";
        
        forceLayoutUpdate();
    }

    public void deleteAccount()
    {
        Strapi.DeleteAccount();
        UsernameInput.transform.parent.gameObject.SetActive(false);
        EmailInput.transform.parent.gameObject.SetActive(false);
        PasswordInput.transform.parent.gameObject.SetActive(false);
        nameInput.transform.parent.gameObject.SetActive(false);
        surnameInput.transform.parent.gameObject.SetActive(false);

        RegisterSubmitButton.gameObject.SetActive(false);
        LoginSubmitButton.gameObject.SetActive(true);
        RegisterToggleButton.gameObject.SetActive(true);
        LoginToggleButton.gameObject.SetActive(false);
        LoadingObject.SetActive(false);

        EditProfileButton.gameObject.SetActive(false);
        SubmitEditRequest.gameObject.SetActive(false);
        EditUsernameInput.transform.parent.gameObject.SetActive(false);
        EditEmailInput.transform.parent.gameObject.SetActive(false);
        EditPasswordInput.transform.parent.gameObject.SetActive(false);
        EditNameInput.transform.parent.gameObject.SetActive(false);
        EditSurnameInput.transform.parent.gameObject.SetActive(false);

        HeaderText.text = "Successfully deleted account. App will now close to submit changes.";

        forceLayoutUpdate();

        Invoke("CloseApp", 3.0f);   
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void EditProfile()
    {
        UsernameInput.transform.parent.gameObject.SetActive(false);
        EmailInput.transform.parent.gameObject.SetActive(false);
        PasswordInput.transform.parent.gameObject.SetActive(false);
        nameInput.transform.parent.gameObject.SetActive(false);
        surnameInput.transform.parent.gameObject.SetActive(false);

        RegisterSubmitButton.gameObject.SetActive(false);
        LoginSubmitButton.gameObject.SetActive(false);
        RegisterToggleButton.gameObject.SetActive(false);
        LoginToggleButton.gameObject.SetActive(false);

        EditProfileButton.gameObject.SetActive(false);
        SubmitEditRequest.gameObject.SetActive(true);
        EditUsernameInput.transform.parent.gameObject.SetActive(true);
        EditEmailInput.transform.parent.gameObject.SetActive(true);
        EditPasswordInput.transform.parent.gameObject.SetActive(true);
        EditNameInput.transform.parent.gameObject.SetActive(true);
        EditSurnameInput.transform.parent.gameObject.SetActive(true);

        EditUsernameInput.GetComponent<EditProfilePlaceholderText>().ShowText();
        EditEmailInput.GetComponent<EditProfilePlaceholderText>().ShowText();
        EditPasswordInput.GetComponent<EditProfilePlaceholderText>().ShowText();
        EditNameInput.GetComponent<EditProfilePlaceholderText>().ShowText();
        EditSurnameInput.GetComponent<EditProfilePlaceholderText>().ShowText();

        HeaderText.text = "Profile Settings";

        forceLayoutUpdate();
    }

    public void SubmitRequest()
    {
        Strapi.EditProfile(EditUsernameInput.text);

        UsernameInput.transform.parent.gameObject.SetActive(false);
        EmailInput.transform.parent.gameObject.SetActive(false);
        PasswordInput.transform.parent.gameObject.SetActive(false);
        nameInput.transform.parent.gameObject.SetActive(false);
        surnameInput.transform.parent.gameObject.SetActive(false);

        RegisterSubmitButton.gameObject.SetActive(false);
        LoginSubmitButton.gameObject.SetActive(false);
        RegisterToggleButton.gameObject.SetActive(false);
        LoginToggleButton.gameObject.SetActive(false);

        EditProfileButton.gameObject.SetActive(false);
        SubmitEditRequest.gameObject.SetActive(false);
        EditUsernameInput.transform.parent.gameObject.SetActive(false);
        EditEmailInput.transform.parent.gameObject.SetActive(false);
        EditPasswordInput.transform.parent.gameObject.SetActive(false);
        EditNameInput.transform.parent.gameObject.SetActive(false);
        EditSurnameInput.transform.parent.gameObject.SetActive(false);

        HeaderText.text = "Profile updated";
        forceLayoutUpdate();
    }
    
    private void forceLayoutUpdate()
    {
        Canvas.ForceUpdateCanvases();
        ContainerLayout.enabled = false;
        ContainerLayout.enabled = true;
    }

    public void OnLoginSubmit()
    {
        Strapi.Login(UsernameInput.text, PasswordInput.text);
    }

    public void OnRegisterSubmit()
    {
        Strapi.Register(UsernameInput.text, nameInput.text, surnameInput.text, EmailInput.text, PasswordInput.text);
    }

    private void toggleLoading()
    {
        isLoading = !isLoading;
        LoadingObject.SetActive(isLoading);
    }

    private void handleSuccessfulAuthentication(AuthResponse authUser)
    {
        toggleLoading();
        HeaderText.text = $"Success. Welcome {authUser.user.username}";
        RegisterSubmitButton.gameObject.SetActive(false);
        LoginSubmitButton.gameObject.SetActive(false);
        UsernameInput.transform.parent.gameObject.SetActive(false);
        PasswordInput.transform.parent.gameObject.SetActive(false);
        EmailInput.transform.parent.gameObject.SetActive(false);
        nameInput.transform.parent.gameObject.SetActive(false);
        surnameInput.transform.parent.gameObject.SetActive(false);
        RegisterToggleButton.gameObject.SetActive(false);
        LoginToggleButton.gameObject.SetActive(false);

        DeleteAccountButton.gameObject.SetActive(true);
        EditProfileButton.gameObject.SetActive(true);
    }

    private void handleUnsuccessfulAuthentication(Exception error)
    {
        toggleLoading();
        HeaderText.text = $"Authentication Error: {error.Message}";
    }
}
