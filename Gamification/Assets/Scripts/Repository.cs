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
        ConnectToDatabase("aj8015");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ConnectToDatabase(string dataBase)
    {
        conn = new NpgsqlConnection("Server=pgserver.mah.se; User Id=aj8015; Password=fbninmt0; Database=" + dataBase);
        conn.Open();
    }

    public void CreateUser(string email, int age, string gender, string nativeLanguage, int languageLevel, string username, string password)
    {
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("INSERT INTO users (email, age, gender, nlanguage, username, password, languageLevel) VALUES(:email, :age, :gender, :nlanguage, :username, :password, :languageLevel)", conn);
        //cmd = new NpgsqlCommand("insert into band (bandnamn, land, anstalldpnr) values(:bandnamn, :land, :kontakt)", conn);
        cmd.Parameters.Add(new NpgsqlParameter("email", email));
        cmd.Parameters.Add(new NpgsqlParameter("age", age));
        cmd.Parameters.Add(new NpgsqlParameter("gender", gender));
        cmd.Parameters.Add(new NpgsqlParameter("nlanguage", nativeLanguage));
        cmd.Parameters.Add(new NpgsqlParameter("username", username));
        cmd.Parameters.Add(new NpgsqlParameter("password", password));
        cmd.Parameters.Add(new NpgsqlParameter("languageLevel", languageLevel));
        cmd.ExecuteNonQuery();
    }

    public bool DoesUserExist(string username)
    {
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("SELECT COUNT(*) from users WHERE username = @p", conn);
        cmd.Parameters.AddWithValue("p", username);
        Int64 count = (Int64)cmd.ExecuteScalar();
        Debug.Log("Users with that username: " + count);
        return (count > 0);
    }
}
