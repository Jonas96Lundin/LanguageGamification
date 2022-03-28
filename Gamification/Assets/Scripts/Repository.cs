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

        AddToGamesPlayed(username);
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

    private static void AddToGamesPlayed(string username)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("INSERT INTO gamesplayed(username, colorwheelcounter, traingamecounter) VALUES(:username, 0, 0)", conn);
        cmd.Parameters.Add(new NpgsqlParameter("username", username));
        cmd.ExecuteNonQuery();
        conn.Close();
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

    public static void AddToColorwheelLeaderboard(int score, float time)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("INSERT INTO colorwheelLeaderboard(username, score, time) VALUES(:username, :score, :time);", conn);
        cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
        cmd.Parameters.Add(new NpgsqlParameter("score", score));
        cmd.Parameters.Add(new NpgsqlParameter("time", time));
        System.Object res = cmd.ExecuteScalar();
        conn.Close();
        Debug.Log(res);
    }

    public static void AddToTraingameLeaderboard(int score, float time)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("INSERT INTO traingameLeaderboard(username, score, time) VALUES(:username, :score, :time); ", conn);
        cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
        cmd.Parameters.Add(new NpgsqlParameter("score", score));
        cmd.Parameters.Add(new NpgsqlParameter("time", time));
        System.Object res = cmd.ExecuteScalar();
        conn.Close();
        Debug.Log(res);
    }

    public static Dictionary<string, List<float>> GetColorWheelLeaderboard()
    {
        //Dictionary<string, int> leaderboard = new Dictionary<string, int>();
        //ConnectToDatabase("aj8015");
        //NpgsqlCommand cmd;
        //cmd = new NpgsqlCommand("SELECT username, score FROM colorwheelLeaderboard ORDER BY score DESC", conn);

        //NpgsqlDataReader dr = cmd.ExecuteReader();
        //while (dr.Read())
        //{
        //	int tempValue;
        //	if (leaderboard.ContainsKey(dr[0].ToString()))
        //	{
        //		leaderboard.TryGetValue(dr[0].ToString(), out tempValue);

        //		if (tempValue < dr.GetInt32(1))
        //		{
        //			leaderboard.Add(dr[0].ToString(), dr.GetInt32(1));
        //		}
        //	}
        //	else
        //	{
        //		leaderboard.Add(dr[0].ToString(), dr.GetInt32(1));
        //	}
        //}
        //dr.Close();
        //conn.Close();
        //for (int i = 0; i < leaderboard.Count; i++)
        //{
        //	Debug.Log(leaderboard.Keys + " " + leaderboard.Values);
        //}
        //foreach (KeyValuePair<string, int> pair in leaderboard)
        //{
        //	Debug.Log(pair.Key + " " + pair.Value);
        //}
        //return leaderboard;
        Dictionary<string, List<float>> leaderboard = new Dictionary<string, List<float>>();
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd;
        cmd = new NpgsqlCommand("SELECT username, score, time FROM colorwheelLeaderboard ORDER BY score DESC, time ASC ", conn);

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
                else if (tempValues[0] == dr.GetInt32(1))
                {
                    //leaderboard.TryGetValue(dr[0].ToString(), out tempTime);
                    if (tempValues[1] > (float)dr.GetDecimal(2))
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
                else if (tempValues[0] == dr.GetInt32(1))
                {
                    //leaderboard.TryGetValue(dr[0].ToString(), out tempTime);
                    if (tempValues[1] > (float)dr.GetDecimal(2))
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

    public static float[] GetBestResult(Games game)
    {
        float[] result = new float[2] { 0, 0 };
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd = new NpgsqlCommand();

        switch (game)
        {
            case Games.COLORWHEEL:
                cmd = new NpgsqlCommand("SELECT score, time FROM colorWheelLeaderboard WHERE username=:username ORDER BY score ASC, time  DESC", conn);
                cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
                break;
            case Games.TRAINGAME:
                cmd = new NpgsqlCommand("SELECT score, time FROM trainGameLeaderboard WHERE username=:username ORDER BY score ASC, time DESC", conn);
                cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
                break;
        }
        NpgsqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            result[0] = dr.GetInt32(0);
            result[1] = (float)dr.GetDecimal(1);
        }

        dr.Close();
        conn.Close();

        return result;
    }

    public static void AddPlayedGame(Games game)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd = new NpgsqlCommand();

        switch (game)
        {
            case Games.COLORWHEEL:
                cmd = new NpgsqlCommand("UPDATE gamesplayed SET colorwheelcounter = colorwheelcounter + 1 WHERE username = :username", conn);
                cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
                break;
            case Games.TRAINGAME:
                cmd = new NpgsqlCommand("UPDATE gamesplayed SET traingamecounter = traingamecounter + 1 WHERE username = :username", conn);
                cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
                break;
        }
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public static int GetPlayedGames(Games game)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd = new NpgsqlCommand();
        int playedGames = 0;

        switch (game)
        {
            case Games.COLORWHEEL:
                cmd = new NpgsqlCommand("SELECT colorwheelcounter FROM gamesplayed WHERE username = :username ", conn);
                cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
                break;
            case Games.TRAINGAME:
                cmd = new NpgsqlCommand("SELECT colorwheelcounter FROM gamesplayed WHERE username = :username ", conn);
                cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
                break;
        }

        Int32 count = (Int32)cmd.ExecuteScalar();
        conn.Close();

        return playedGames;
    }

    public static List<string> GetAquiredBadges(Games game)
    {
        ConnectToDatabase("aj8015");
        NpgsqlCommand cmd = new NpgsqlCommand();
        List<string> aquiredBadges = new List<string>();

        switch (game)
        {
            case Games.COLORWHEEL:
                cmd = new NpgsqlCommand("SELECT badge FROM aquiredbadges WHERE username = :username AND badge LIKE 'colorWheel%';", conn);
                cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
                break;
            case Games.TRAINGAME:
                cmd = new NpgsqlCommand("SELECT badge FROM aquiredbadges WHERE username = :username AND badge LIKE 'trainGame%';", conn);
                cmd.Parameters.Add(new NpgsqlParameter("username", PlayerPrefs.GetString("username")));
                break;
        }

        NpgsqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            aquiredBadges.Add(dr[0].ToString());
        }

        dr.Close();
        conn.Close();

        return aquiredBadges;
    }
}
