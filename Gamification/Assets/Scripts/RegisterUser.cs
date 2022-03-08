using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityNpgsql;
using TMPro;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour
{
    [SerializeField]
    TMP_InputField emailInput;
    [SerializeField]
    TMP_Dropdown ageInput;
    [SerializeField]
    ToggleGroup genderInput;
    [SerializeField]
    ToggleGroup languageLevelInput;
    [SerializeField]
    TMP_InputField nLanguageInput;
    [SerializeField]
    TMP_InputField userNameInput;
    [SerializeField]
    TMP_InputField passwordInput;
    [SerializeField]
    TMP_InputField confirmPasswordInput;
    [SerializeField]
    TMP_Text differentPasswords;
    [SerializeField]
    GameObject texts;
    [SerializeField]
    GameObject registerPanel;
    [SerializeField]
    GameObject loginPanel;

    string email;
    int age = -1;
    string gender/* = "Male"*/;
    string nLanguage;
    int languageLevel = -1;
    string username;
    string password;
    string confirmPassword;


    //private NpgsqlConnection conn;
    int counter = 0;
    static int usernameMaxLength = 20;

    [SerializeField]
    Repository repository;

    // Start is called before the first frame update
    void Start()
    {
        //ConnectToDatabase("aj8015");
        emailInput.onEndEdit.AddListener(EditEmail);
        ageInput.onValueChanged.AddListener(EditAge);
        nLanguageInput.onEndEdit.AddListener(EditNLanguage);
        userNameInput.onEndEdit.AddListener(EditUsername);
        passwordInput.onEndEdit.AddListener(EditPassword);
        confirmPasswordInput.onEndEdit.AddListener(ConfirmPassword);
        differentPasswords.enabled = false;
        userNameInput.characterLimit = usernameMaxLength;

        genderInput.allowSwitchOff = true;
        genderInput.SetAllTogglesOff();
        gender = "";

        languageLevelInput.allowSwitchOff = true;
        languageLevelInput.SetAllTogglesOff();
        languageLevel = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    NpgsqlCommand cmd;
        //    cmd = new NpgsqlCommand("INSERT INTO test (personid, namn) values(:personid, :namn)", conn);
        //    cmd.Parameters.Add(new NpgsqlParameter("personid", counter));
        //    cmd.Parameters.Add(new NpgsqlParameter("namn", "TestNamn" + counter++));
        //    cmd.ExecuteNonQuery();
        //    Debug.Log("Added new test person");
        //}
    }

    //private void ConnectToDatabase(string dataBase)
    //{
    //    conn = new NpgsqlConnection("Server=pgserver.mah.se; User Id=aj8015; Password=fbninmt0; Database=" + dataBase);
    //    conn.Open();
    //}

    public void OnGenderToggleChange(Toggle newGender)
    {
        Debug.Log(gender);
        gender = newGender.GetComponentInChildren<Text>().text;
        Debug.Log(gender);
        genderInput.allowSwitchOff = false;
        texts.transform.Find("Text_gender").transform.Find("asterisk").gameObject.SetActive(false);
    }
    public void OnLanguageLevelToggleChange(Toggle newLevel)
    {
        Debug.Log(languageLevel);
        switch (newLevel.GetComponentInChildren<Text>().text)
        {
            case "Beginner (A1-A2)":
                languageLevel = 0;
                break;
            case "Intermediate (B1-B2)":
                languageLevel = 1;
                break;
            case "Advanced (C1-C2)":
                languageLevel = 2;
                break;
        }
        Debug.Log(languageLevel);
        languageLevelInput.allowSwitchOff = false;
        texts.transform.Find("Text_languageLevel").transform.Find("asterisk").gameObject.SetActive(false);
    }

    public void EditEmail(string newEmail)
    {
        email = newEmail.ToLower();
        texts.transform.Find("Text_email").transform.Find("asterisk").gameObject.SetActive(false);
        Debug.Log(email);
    }
    public void EditAge(int newAge)
    {
        age = newAge;
        texts.transform.Find("Text_age").transform.Find("asterisk").gameObject.SetActive(false);
        Debug.Log(age);
    }
    public void EditNLanguage(string newNLanguage)
    {
        nLanguage = newNLanguage;
        texts.transform.Find("Text_nativeLanguage").transform.Find("asterisk").gameObject.SetActive(false);
        Debug.Log(nLanguage);
    }
    public void EditUsername(string newUsername)
    {
        username = newUsername.ToLower();
        texts.transform.Find("Text_username").transform.Find("asterisk").gameObject.SetActive(false);
        Debug.Log(username);
    }
    public void EditPassword(string newPassword)
    {
        password = newPassword;
        texts.transform.Find("Text_password").transform.Find("asterisk").gameObject.SetActive(false);
        Debug.Log(password);
    }
    public void ConfirmPassword(string newConfirmPassword)
    {
        confirmPassword = newConfirmPassword;
        if (confirmPassword != password)
        {
            Debug.Log("Password not the same");
            differentPasswords.enabled = true;
        }
        else
        {
            Debug.Log(password);
            differentPasswords.enabled = false;
            texts.transform.Find("Text_confirmPassword").transform.Find("asterisk").gameObject.SetActive(false);
        }
    }

    public void RegisterNewUser()
    {
        if (!IsStringFieldFilled(email, "Text_email") |
            !IsStringFieldFilled(gender, "Text_gender") |
            !IsStringFieldFilled(nLanguage, "Text_nativeLanguage") |
            !IsStringFieldFilled(username, "Text_username") |
            !IsStringFieldFilled(password, "Text_password") |
            !IsStringFieldFilled(confirmPassword, "Text_confirmPassword") |
            !IsIntegerFieldFilled(age, "Text_age") |
            !IsIntegerFieldFilled(languageLevel, "Text_languageLevel")
            )
        {
            texts.transform.Find("Text_fieldsNotFilled").gameObject.SetActive(true);
        }
        else
        {
            texts.transform.Find("Text_fieldsNotFilled").gameObject.SetActive(false);
            if (repository.DoesUserExist(username))
            {
                texts.transform.Find("Text_usernameTaken").gameObject.SetActive(true);
            }
            else
            {
                // User successfully created
                texts.transform.Find("Text_usernameTaken").gameObject.SetActive(false);
                repository.CreateUser(email, age, gender, nLanguage, languageLevel, username, password);
                registerPanel.SetActive(false);
                loginPanel.SetActive(true);
            }
        }
    }

    private bool IsStringFieldFilled(string memberVariable, string nameOfField)
    {
        if (string.IsNullOrEmpty(memberVariable))
        {
            texts.transform.Find(nameOfField).transform.Find("asterisk").gameObject.SetActive(true);
            return false;
        }
        return true;
    }

    private bool IsIntegerFieldFilled(int memberVariable, string nameOfField)
    {
        if (memberVariable < 0)
        {
            texts.transform.Find(nameOfField).transform.Find("asterisk").gameObject.SetActive(true);
            return false;
        }
        return true;
    }


}