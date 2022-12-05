using System;
using System.Collections;
using System.Collections.Generic;
using StrapiForUnity;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginOrRegisterScreen : MonoBehaviour {
    public Text HeaderText;
    public VerticalLayoutGroup ContainerLayout;
    public Toggle RememberMeToggle;

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

    public StrapiComponent Strapi;

    // Start is called before the first frame update
    void Start() {
        if (Strapi == null) {
            Debug.LogError("No Strapi component found. Please make sure you've got an active Strapi component assigned to the LoginOrRegisterForm");
            return;
        }

        LoginToggleButton.onClick.Invoke();

        registerEventHandlers();
    }

    private void registerEventHandlers() {
        Strapi.OnAuthSuccess += handleSuccessfulAuthentication;
        Strapi.OnAuthFail += handleUnsuccessfulAuthentication;
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
    }

    public void OnLoginToggle() {
        RegisterSubmitButton.gameObject.SetActive(false);
        LoginSubmitButton.gameObject.SetActive(true);
        EmailInput.transform.parent.gameObject.SetActive(false);
        RegisterToggleButton.gameObject.SetActive(true);
        nameInput.transform.parent.gameObject.SetActive(false);
        surnameInput.transform.parent.gameObject.SetActive(false);
        LoginToggleButton.gameObject.SetActive(false);

        HeaderText.text = "Login";

        forceLayoutUpdate();
    }

    public void OnRegisterToggle() {
        RegisterSubmitButton.gameObject.SetActive(true);
        LoginSubmitButton.gameObject.SetActive(false);
        EmailInput.transform.parent.gameObject.SetActive(true);
        nameInput.transform.parent.gameObject.SetActive(true);
        surnameInput.transform.parent.gameObject.SetActive(true);
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
        Strapi.Login(UsernameInput.text, PasswordInput.text, RememberMeToggle.isOn);
    }

    public void OnRegisterSubmit() {
        Strapi.Register(UsernameInput.text, nameInput.text, surnameInput.text, EmailInput.text, PasswordInput.text, RememberMeToggle.isOn);
    }

    private void handleSuccessfulAuthentication(AuthResponse authUser) {
        SceneManager.LoadScene("MainMenu");
    }

    private void handleUnsuccessfulAuthentication(Exception error) {
        HeaderText.text = $"Authentication Error: {error.Message}";
    }
}
