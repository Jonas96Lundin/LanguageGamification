using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HomeController : MonoBehaviour
{
	[SerializeField] GameObject HomePanel;
	[SerializeField] GameObject ColorWheelPanel;
	[SerializeField] TMP_Text leaderboardTitle;
	[SerializeField] TMP_Text leaderboardNames;
	[SerializeField] TMP_Text leaderboardScores;
	public void OpenColorWheelPanel()
    {
        HomePanel.SetActive(false);
        ColorWheelPanel.SetActive(true);
        GetLeaderboard(Games.COLORWHEEL);
    }

    private void GetLeaderboard(Games game)
    {
        Dictionary<string, int> leaderboard = new Dictionary<string, int>();
        leaderboardNames.text = "";
        leaderboardScores.text = "";
        switch (game)
        {
            case Games.COLORWHEEL:
                leaderboard = Repository.GetColorWheelLeaderboard();
                leaderboardTitle.text = "Palette de couleurs Classement";
                break;
            case Games.TRAINGAME:
                //leaderboard = Repository.GetTraingameLeaderboard();
                //leaderboardTitle.text = "Palette de couleurs Classement";
                break;
        }

        ShowPointsLeaderboard(leaderboard);

    }
    private void ShowPointsLeaderboard(Dictionary<string, int> leaderboard)
    {
        int leaderboardCounter = 0;
        foreach (KeyValuePair<string, int> pair in leaderboard)
        {
            leaderboardCounter++;
            leaderboardNames.text += "\n" + leaderboardCounter + ": " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pair.Key);

            leaderboardScores.text += "\n" + pair.Value;
            if (leaderboardCounter >= 3)
            {
                break;
            }
        }
    }

    public void OnColorWheel()
	{
		SceneManager.LoadScene(2);
	}

	public void LogOut()
	{
		SceneManager.LoadScene(0);
		PlayerPrefs.SetString("username", "");
	}

    public void BackToHomePanel(string panelToClose)
    {
        GameObject.Find(panelToClose).SetActive(false);
        HomePanel.SetActive(true);
    }
}
