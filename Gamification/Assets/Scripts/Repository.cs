using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityNpgsql;

public static class Repository
{
    private static NpgsqlConnection conn;

    private static void ConnectToDatabase(string dataBase)
    {
        conn = new NpgsqlConnection("Server=pgserver.mau.se; User Id=aj8015; Password=mau; Database=" + dataBase);
        conn.Open();
    }

    public static void CreateUser(string email, int age, string gender, string nativeLanguage, int languageLevel, string username, string password)
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

    public static bool DoesUserExist(string username)
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

    public static bool UserLogin(string username, string password)
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

    public static void AddBadge(string badge)
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

    public static void AddToColorwheelLeaderboard(int score)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("INSERT INTO colorwheelLeaderboard(username, score) VALUES(:username, :score);", conn);
        cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
        cmd.Parameters.Add(new NpgsqlParameter("score", score));
        System.Object res = cmd.ExecuteScalar();
        conn.Close();
        Debug.Log(res);
    }

    public static void AddToTraingameLeaderboard(int score, float time)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand(" INSERT INTO traingameLeaderboard(username, points, time) VALUES(:username, :score, :time); ", conn);
        cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
        cmd.Parameters.Add(new NpgsqlParameter("score", score));
        cmd.Parameters.Add(new NpgsqlParameter("time", time));
        System.Object res = cmd.ExecuteScalar();
        conn.Close();
        Debug.Log(res);
    }

    public static Dictionary<string, int> GetColorWheelLeaderboard()
    {
        Dictionary<string, int> leaderboard = new Dictionary<string, int>();
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("SELECT username, score FROM colorwheelLeaderboard ORDER BY score DESC", conn);

        NpgsqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            int tempValue;
            if (leaderboard.ContainsKey(dr[0].ToString()))
            {
                leaderboard.TryGetValue(dr[0].ToString(), out tempValue);

                if (tempValue < dr.GetInt32(1))
                {
                    leaderboard.Add(dr[0].ToString(), dr.GetInt32(1));
                }
            }
            else
            {
                leaderboard.Add(dr[0].ToString(), dr.GetInt32(1));
            }
        }
        dr.Close();
        conn.Close();
        for (int i = 0; i < leaderboard.Count; i++)
        {
            Debug.Log(leaderboard.Keys + " " + leaderboard.Values);
        }
        foreach (KeyValuePair<string, int> pair in leaderboard)
        {
            Debug.Log(pair.Key + " " + pair.Value);
        }
        return leaderboard;
    }

    public static Dictionary<string, List<float>> GetTraingameLeaderboard()
    {
        Dictionary<string, List<float>> leaderboard = new Dictionary<string, List<float>>();
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("SELECT username, score, time FROM traingameLeaderboard ORDER BY score DESC, time ASC ", conn);

        NpgsqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            int tempValue;
            float tempTime;
            List<float> tempValues;
            if (leaderboard.ContainsKey(dr[0].ToString()))
            {
                leaderboard.TryGetValue(dr[0].ToString(), out tempValues);

                if (tempValues[0] < dr.GetInt32(1))
                {
                    leaderboard.Add(dr[0].ToString(), new List<float>() { dr.GetInt32(1), (float)dr.GetDecimal(2) });
                }
                else if(tempValues[0] == dr.GetInt32(1))
                {
                    //leaderboard.TryGetValue(dr[0].ToString(), out tempTime);
                    if (tempValues[1] > dr.GetFloat(2))
                    {
                        leaderboard.Add(dr[0].ToString(), new List<float>() { dr.GetInt32(1), (float)dr.GetDecimal(2) });
                    }
                }
            }
            else
            {
                leaderboard.Add(dr[0].ToString(), new List<float>() { dr.GetInt32(1), (float)dr.GetDecimal(2) });
            }
        }
        dr.Close();
        conn.Close();
        for (int i = 0; i < leaderboard.Count; i++)
        {
            Debug.Log(leaderboard.Keys + " " + leaderboard.Values);
        }
        foreach (KeyValuePair<string, List<float>> pair in leaderboard)
        {
            Debug.Log(pair.Key + " " + pair.Value[0] + " " + pair.Value[1]);
        }
        return leaderboard;
    }
}
