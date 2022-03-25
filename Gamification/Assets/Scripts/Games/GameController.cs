using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	private Games currentGame;
	[SerializeField] private PointController pointController;

	[SerializeField] TMP_Text currentBestScore;
	[SerializeField] TMP_Text currentBestTime;

	[SerializeField] TMP_Text newScore;
	[SerializeField] TMP_Text newTime;

	[SerializeField] TMP_Text leaderboardTitle;
	[SerializeField] TMP_Text leaderboardNames;
	[SerializeField] TMP_Text leaderboardScores;
	[SerializeField] TMP_Text leaderboardTimes;

	private int hours;
	private int minutes;
	private int seconds;
	private float milliSeconds;

	public Games CurrentGame { get { return currentGame; } }

	public void SetGame(Games game)
	{
		currentGame = game;
	}
	public void OnContinue()
	{
		SceneManager.LoadScene(1);
	}

	public void OnReset()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnQuitGame()
	{
		SceneManager.LoadScene(1);
	}
	public void EndGame()
	{
		float[] result = Repository.GetBestResult(currentGame);
		currentBestScore.text = result[0].ToString();
		if (result[1] > 0)
		{
			currentBestTime.text = GetTime(result[1]);
		}

		newScore.text = pointController.CurrentPoints.ToString();
		newTime.text = GetTime(pointController.GameTime);

		//Repository.AddPlayedGame(currentGame);
		AddToLeaderboard();
		GetLeaderboard();
	}

	private string GetTime(float totalTime)
	{
		CalculateTime(totalTime);

		if (hours > 0)
		{
			return hours.ToString() + "h " + minutes.ToString() + "m " + seconds.ToString() + "s";
		}
		else if (minutes > 0)
		{
			return minutes.ToString() + "m " + seconds.ToString() + "s";
		}
		else
		{
			return seconds.ToString() + "." + milliSeconds.ToString("F0") + "s";
		}
	}

	private void CalculateTime(float totalTime)
	{
		hours = TimeSpan.FromSeconds(totalTime).Hours;
		minutes = TimeSpan.FromSeconds(totalTime).Minutes;
		seconds = TimeSpan.FromSeconds(totalTime).Seconds;
		milliSeconds = (totalTime - seconds) * 10;
		if (milliSeconds > 9)
			milliSeconds = 0;
	}

	private void AddToLeaderboard()
	{
		switch (currentGame)
		{
			case Games.COLORWHEEL:
				Repository.AddToColorwheelLeaderboard(pointController.CurrentPoints, pointController.GameTime);
				break;
			case Games.TRAINGAME:
				Repository.AddToTraingameLeaderboard(pointController.CurrentPoints, pointController.GameTime);
				break;
		}
	}

	private void GetLeaderboard()
	{
		Dictionary<string, List<float>> leaderboard = new Dictionary<string, List<float>>();
		int leaderboardCounter = 0;
		switch (currentGame)
		{
			case Games.COLORWHEEL:
				
				leaderboardTitle.text = "Palette de couleurs Classement";
				leaderboardNames.text = "";
				leaderboardScores.text = "";
				leaderboardTimes.text = "";

				leaderboard = Repository.GetColorWheelLeaderboard();

				foreach (KeyValuePair<string, List<float>> pair in leaderboard)
				{
					leaderboardCounter++;
					leaderboardNames.text += "\n" + leaderboardCounter + ": " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pair.Key);
					leaderboardScores.text += "\n" + pair.Value[0] + "p";
					leaderboardTimes.text += "\n" + GetTime(pair.Value[1]);
					if (leaderboardCounter >= 3)
					{
						break;
					}
				}

				break;
			case Games.TRAINGAME:

				leaderboardTitle.text = "Jeu de trains";
				leaderboardNames.text = "";
				leaderboardScores.text = "";
				leaderboardTimes.text = "";

				leaderboard = Repository.GetTraingameLeaderboard();

				foreach (KeyValuePair<string, List<float>> pair in leaderboard)
				{
					leaderboardCounter++;
					leaderboardNames.text += "\n" + leaderboardCounter + ": " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pair.Key);
					leaderboardScores.text += "\n" + pair.Value[0] + "p";
					leaderboardTimes.text += "\n" + GetTime(pair.Value[1]);
					if (leaderboardCounter >= 3)
					{
						break;
					}
				}

				break;
		}


		
	}
}
