using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginHandler : MonoBehaviour
{
    public GameObject loginPanel;
    public TMP_InputField userNameInputField;
    public TMP_InputField passwordInputField;

    public void LoginButtonDown()
    {
        NetworkClient.instance.SendDataToServer("CallLogin", new string[,] { { "username", userNameInputField.text }, { "password", passwordInputField.text } });
    }

    private void OnEnable()
    {
        NetworkEvents.login += OnLogin;
    }
    private void OnDisable()
    {
        NetworkEvents.login -= OnLogin;
    }

    private void OnLogin(bool set, string msg)
    {
        if(set)
        {
            loginPanel.SetActive(false);
            SceneManager.LoadScene("SelectCharacterScene");
        }
    }
}
