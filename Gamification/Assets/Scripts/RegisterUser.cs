using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityNpgsql;
using TMPro;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour
{
    [SerializeField]
    TMP_InputField emailInput;
    [SerializeField]
    TMP_Dropdown ageInput;
    //[SerializeField]
    //ToggleGroup genderInput;
    [SerializeField]
    TMP_InputField nLanguageInput;
    [SerializeField]
    TMP_InputField userNameInput;
    [SerializeField]
    TMP_InputField passwordInput;
    [SerializeField]
    TMP_InputField confirmPasswordInput;

    string email;
    int age;
    string gender = "Male";
    string nLanguage;
    int languageLevel = 0;
    string username;
    string password;
    string confirmPassword;


    private NpgsqlConnection conn;
    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        ConnectToDatabase("aj8015");
        emailInput.onEndEdit.AddListener(EditEmail);
        ageInput.onValueChanged.AddListener(EditAge);
        nLanguageInput.onEndEdit.AddListener(EditNLanguage);
        userNameInput.onEndEdit.AddListener(EditUsername);
        passwordInput.onEndEdit.AddListener(EditPassword);
        confirmPasswordInput.onEndEdit.AddListener(ConfirmPassword);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NpgsqlCommand cmd;
            cmd = new NpgsqlCommand("INSERT INTO test (personid, namn) values(:personid, :namn)", conn);
            cmd.Parameters.Add(new NpgsqlParameter("personid", counter));
            cmd.Parameters.Add(new NpgsqlParameter("namn", "TestNamn" + counter++));
            cmd.ExecuteNonQuery();
            Debug.Log("Added new test person");
        }
    }

    private void ConnectToDatabase(string dataBase)
    {
        conn = new NpgsqlConnection("Server=pgserver.mah.se; User Id=aj8015; Password=fbninmt0; Database=" + dataBase);
        conn.Open();
    }

    public void OnGenderToggleChange(Toggle newGender)
    {
        gender = newGender.GetComponentInChildren<Text>().text;
        Debug.Log(gender);
    }
    public void OnLanguageLevelToggleChange(Toggle newLevel)
    {
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
    }

    public void EditEmail(string newEmail)
    {
        email = newEmail;
        Debug.Log(email);
    }
    public void EditAge(int newAge)
    {
        age = newAge;
        Debug.Log(age);
    }
    public void EditGender(string newGender)
    {
        gender = newGender;
        Debug.Log(gender);
    }
    public void EditNLanguage(string newNLanguage)
    {
        nLanguage = newNLanguage;
        Debug.Log(nLanguage);
    }
    public void EditUsername(string newUsername)
    {
        username = newUsername;
        Debug.Log(username);
    }
    public void EditPassword(string newPassword)
    {
        password = newPassword;
        Debug.Log(password);
    }
    public void ConfirmPassword(string newConfirmPassword)
    {
        confirmPassword = newConfirmPassword;
        if(confirmPassword != password)
        {
            Debug.Log("Password not the same");
        }
        Debug.Log(password);
    }
}