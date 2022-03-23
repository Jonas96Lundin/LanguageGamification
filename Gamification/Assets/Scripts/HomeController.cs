using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HomeController : MonoBehaviour
{
	[SerializeField] GameObject HomePanel;
	[SerializeField] GameObject ColorWheelPanel;
    [SerializeField] GameObject TrainGamePanel;
    [SerializeField] TMP_Text leaderboardTitle;
	[SerializeField] TMP_Text leaderboardNames;
	[SerializeField] TMP_Text leaderboardScores;
    [SerializeField] TMP_Text leaderboardTimes;
    public void OpenColorWheelPanel()
    {
        HomePanel.SetActive(false);
        ColorWheelPanel.SetActive(true);
        GetLeaderboard(Games.COLORWHEEL);
    }
    public void OpenTrainGamePanel()
    {
        HomePanel.SetActive(false);
        TrainGamePanel.SetActive(true);
        GetLeaderboard(Games.TRAINGAME);
    }

    private void GetLeaderboard(Games game)
    {
        //Dictionary<string, int> leaderboard = new Dictionary<string, int>();
        leaderboardNames.text = "";
        leaderboardScores.text = "";
        leaderboardTimes.text = "";
        switch (game)
        {
            case Games.COLORWHEEL:
                //leaderboard = Repository.GetColorWheelLeaderboard();
                leaderboardTitle.text = "Palette de couleurs Classement";
                ShowPointsLeaderboard(Repository.GetColorWheelLeaderboard());
                break;
            case Games.TRAINGAME:
                //leaderboard = Repository.GetTraingameLeaderboard();
                //leaderboardTitle.text = "Palette de couleurs Classement";
                leaderboardTitle.text = "Jeu de trains";
                ShowPointsAndTimeLeaderboard(Repository.GetTraingameLeaderboard());
                break;
        }



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
    private void ShowPointsAndTimeLeaderboard(Dictionary<string, List<float>> leaderboard)
    {
        int leaderboardCounter = 0;
        foreach (KeyValuePair<string, List<float>> pair in leaderboard)
        {
            leaderboardCounter++;
            leaderboardNames.text += "\n" + leaderboardCounter + ": " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pair.Key);

            leaderboardScores.text += "\n" + pair.Value[0];

            leaderboardTimes.text += "\n" + pair.Value[1];
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
    public void OnTrainGame()
    {
        SceneManager.LoadScene(3);
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
