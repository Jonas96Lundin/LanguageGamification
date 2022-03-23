using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TMP_Text>().text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PlayerPrefs.GetString("username"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
