using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using StrapiForUnity;
using UnityEditor;
using UnityEngine;

public class LoginOrRegisterScreen : MonoBehaviour {
    public Text HeaderText;
    public Text ErrorText;
    public VerticalLayoutGroup ContainerLayout;

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
    public InputField specialPasswordInput;

    public Toggle userRole;

    public StrapiComponent strapiComponent;

    // Start is called before the first frame update
    void Start() {
        if (strapiComponent == null) {
            Debug.LogError("No Strapi component found. Please make sure you've got an active Strapi component assigned to the LoginOrRegisterForm");
            return;
        }

        LoginToggleButton.onClick.Invoke();

        registerEventHandlers();
    }

    private void registerEventHandlers() {
        strapiComponent.OnAuthSuccess += handleSuccessfulAuthentication;
        strapiComponent.OnAuthFail += handleUnsuccessfulAuthentication;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (LoginSubmitButton.IsActive())
            {
                LoginSubmitButton.onClick.Invoke();
            }
            else if (RegisterSubmitButton.IsActive())
            {
                RegisterSubmitButton.onClick.Invoke();
            }
        }

        if (userRole.isOn)
        {
            specialPasswordInput.transform.parent.gameObject.SetActive(true);
        }
        else if (!userRole.isOn)
        {
            specialPasswordInput.transform.parent.gameObject.SetActive(false);
        }
    }

    public void OnLoginToggle() {
        RegisterSubmitButton.gameObject.SetActive(false);
        LoginSubmitButton.gameObject.SetActive(true);
        EmailInput.transform.parent.gameObject.SetActive(false);
        RegisterToggleButton.gameObject.SetActive(true);
        nameInput.transform.parent.gameObject.SetActive(false);
        surnameInput.transform.parent.gameObject.SetActive(false);
        userRole.transform.parent.gameObject.SetActive(false);
        LoginToggleButton.gameObject.SetActive(false);
        specialPasswordInput.transform.parent.gameObject.SetActive(false);

        HeaderText.text = "Login";

        forceLayoutUpdate();
    }

    public void OnRegisterToggle() {
        RegisterSubmitButton.gameObject.SetActive(true);
        LoginSubmitButton.gameObject.SetActive(false);
        EmailInput.transform.parent.gameObject.SetActive(true);
        nameInput.transform.parent.gameObject.SetActive(true);
        surnameInput.transform.parent.gameObject.SetActive(true);
        userRole.transform.parent.gameObject.SetActive(true);
        RegisterToggleButton.gameObject.SetActive(false);
        LoginToggleButton.gameObject.SetActive(true);

        HeaderText.text = "Register";

        forceLayoutUpdate();
    }


    private void forceLayoutUpdate() {
        Canvas.ForceUpdateCanvases();
        ContainerLayout.enabled = false;
        ContainerLayout.enabled = true;
    }

    public void OnLoginSubmit() {
        if (string.IsNullOrEmpty(UsernameInput.text))
        {
            HandleError("Identifier");
            return;
        }

        if (string.IsNullOrEmpty(PasswordInput.text))
        {
            HandleError("Password");
            return;
        }

        strapiComponent.Login(UsernameInput.text, PasswordInput.text);
    }

    public void OnRegisterSubmit()
    {
        if (string.IsNullOrEmpty(UsernameInput.text))
        {
            HandleError("Username");
            return;
        }

        if (string.IsNullOrEmpty(nameInput.text))
        {
            HandleError("Name");
            return;
        }

        if (string.IsNullOrEmpty(surnameInput.text))
        {
            HandleError("Surname");
            return;
        }

        if (string.IsNullOrEmpty(EmailInput.text))
        {
            HandleError("Email");
            return;
        }

        if (string.IsNullOrEmpty(PasswordInput.text))
        {
            HandleError("Password");
            return;
        }

        if (userRole.isOn && string.IsNullOrEmpty(specialPasswordInput.text))
        {
            HandleError("Special password");
            return;
        }

        strapiComponent.Register(UsernameInput.text, nameInput.text, surnameInput.text, EmailInput.text, PasswordInput.text, userRole.isOn, specialPasswordInput.text);
    }

    private void handleSuccessfulAuthentication(AuthResponse authUser) {
        MySceneManager.instance.LoadScene("MainMenu");
    }

    private void handleUnsuccessfulAuthentication(Exception error) {
        HeaderText.text = $"Authentication Error: {error.Message}";
    }

    private void HandleError(string missingInput)
    {
        ErrorText.transform.gameObject.SetActive(true);
        ErrorText.text = $"Empty {missingInput} form";
    }
}
