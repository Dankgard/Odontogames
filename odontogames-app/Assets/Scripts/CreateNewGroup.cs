using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

using StrapiForUnity;

public class CreateNewGroup : MonoBehaviour {
    public InputField groupNameInput;
    public Button selectMembersButton;
    public Button submitButton;

    public GameObject usersSelectionInterface;
    public GameObject groupCreationInterface;

    public Text headerText;

    public GlobalUserList userList;

    private StrapiUser[] selectedUsers;

    // Start is called before the first frame update
    void Start() {
        groupCreationInterface.SetActive(false);
        headerText.text = "";
    }

    public void OnSelectMembersPressed()
    {
        groupCreationInterface.SetActive(true);
        usersSelectionInterface.SetActive(false);
        selectedUsers = new StrapiUser[40];
        selectedUsers = userList.GetSelectedUsers();
    }

    public void OnSubmitPressed() {
        headerText.text = "Grupo " + groupNameInput.text + " creado.";
        groupCreationInterface.SetActive(false);
        usersSelectionInterface.SetActive(true);

        // Aqui "se crea" el grupo
        StrapiComponent._instance.CreateRole(groupNameInput.text);

        for (int i = 0; i < selectedUsers.Length; i++)
        {
            // aqui
            StrapiComponent._instance.SetUserGroup(groupNameInput.text, selectedUsers[i]);
        }
    }

    public void OnBackPressed() {
        MySceneManager.instance.LoadScene("MainMenu");
    }
}
