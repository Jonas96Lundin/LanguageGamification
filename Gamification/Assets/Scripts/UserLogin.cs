using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserLogin : MonoBehaviour
{
    [SerializeField]
    TMP_InputField usernameInput;
    [SerializeField]
    TMP_InputField passwordInput;
    [SerializeField]
    TMP_Text wrongUsername;
    [SerializeField]
    TMP_Text wrongPassword;

    [SerializeField]
    Repository repository;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        if (repository.DoesUserExist(usernameInput.text.ToLower()))
        {
            wrongUsername.enabled = false;
            if (repository.UserLogin(usernameInput.text.ToLower(), passwordInput.text))
            {
                wrongUsername.enabled = false;
                wrongPassword.enabled = false;
                Debug.Log("Login successful!");
            }
            else
            {
                wrongPassword.enabled = true;
                Debug.Log("Wrong password!");
            }
        }
        else
        {
            wrongUsername.enabled = true;
            wrongPassword.enabled = false;
            Debug.Log("User does not exist!");
        }
    }
}