using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityNpgsql;

public class DatabaseTest : MonoBehaviour
{
    private NpgsqlConnection conn;
    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        ConnectToDatabase("aj8015");
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
        conn = new NpgsqlConnection("Server=pgserver.mah.se; User Id=aj8015; Password=mau; Database=" + dataBase);
        conn.Open();
    }
}
