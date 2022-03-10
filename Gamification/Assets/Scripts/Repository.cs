using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityNpgsql;

public class Repository : MonoBehaviour
{
    private NpgsqlConnection conn;

    // Start is called before the first frame update
    void Start()
    {
        //ConnectToDatabase("aj8015");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ConnectToDatabase(string dataBase)
    {
        conn = new NpgsqlConnection("Server=pgserver.mau.se; User Id=aj8015; Password=mau; Database=" + dataBase);
        conn.Open();
    }

    public void CreateUser(string email, int age, string gender, string nativeLanguage, int languageLevel, string username, string password)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("INSERT INTO person (email, age, gender, nlanguage, username, password, languageLevel) VALUES(:email, :age, :gender, :nlanguage, :username, crypt(:password, gen_salt('bf', 8)), :languageLevel)", conn);
        //cmd = new NpgsqlCommand("INSERT INTO users (email, age, gender, nlanguage, username, password, languageLevel) VALUES(:email, :age, :gender, :nlanguage, :username, :password, :languageLevel)", conn);
        cmd.Parameters.Add(new NpgsqlParameter("email", email));
        cmd.Parameters.Add(new NpgsqlParameter("age", age));
        cmd.Parameters.Add(new NpgsqlParameter("gender", gender));
        cmd.Parameters.Add(new NpgsqlParameter("nlanguage", nativeLanguage));
        cmd.Parameters.Add(new NpgsqlParameter("username", username));
        cmd.Parameters.Add(new NpgsqlParameter("password", password));
        cmd.Parameters.Add(new NpgsqlParameter("languageLevel", languageLevel));
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public bool DoesUserExist(string username)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("SELECT COUNT(*) from person WHERE username = @p", conn);
        cmd.Parameters.AddWithValue("p", username);
        Int64 count = (Int64)cmd.ExecuteScalar();
        Debug.Log("Users with that username: " + count);
        conn.Close();
        return (count > 0);
    }

    public bool UserLogin(string username, string password)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("SELECT COUNT(*) from person WHERE username = @u AND password = crypt(@p, password)", conn);
        cmd.Parameters.AddWithValue("u", username);
        cmd.Parameters.AddWithValue("p", password);
        Int64 count = (Int64)cmd.ExecuteScalar();
        Debug.Log("Users with that username and password: " + count);
        conn.Close();
        return (count > 0);
    }

    public void AddBadge(string badge)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("INSERT INTO aquiredbadges(username, badge) VALUES(:username, :badge)", conn);
        //cmd = new NpgsqlCommand("INSERT INTO users (email, age, gender, nlanguage, username, password, languageLevel) VALUES(:email, :age, :gender, :nlanguage, :username, :password, :languageLevel)", conn);
        cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
        cmd.Parameters.Add(new NpgsqlParameter("badge", badge));
        System.Object res = cmd.ExecuteScalar();
        conn.Close();
        Debug.Log(res);
    } 
}
