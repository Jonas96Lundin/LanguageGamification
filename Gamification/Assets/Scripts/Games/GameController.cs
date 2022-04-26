using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	private Games currentGame;
	[SerializeField] private PointController pointController;

	[SerializeField] TMP_Text currentBestScore;
	[SerializeField] TMP_Text currentBestTime;

	[SerializeField] TMP_Text newScore;
	[SerializeField] TMP_Text newTime;

	//[SerializeField] TMP_Text leaderboardTitle;
	[SerializeField] TMP_Text leaderboardNames;
	[SerializeField] TMP_Text leaderboardScores;
	[SerializeField] TMP_Text leaderboardTimes;


	[SerializeField] List<Image> badges;
	Dictionary<string, bool> aquiredBadges;
	private List<Image> newBadges = new List<Image>();

	[SerializeField] GameObject UICamera;
	[SerializeField] GameObject particleBadgeCelebration;
	[SerializeField] GameObject particleWanderingSpirits;

	private int hours;
	private int minutes;
	private int seconds;
	private float milliSeconds;

	static int maxLeaderboardPositions = 5;

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

		Repository.AddPlayedGame(currentGame);
		SetBadges();
		AddToLeaderboard();
		GetLeaderboard();
		Instantiate(particleWanderingSpirits, UICamera.transform);
	}

	public void SetBadges()
	{
		if (currentGame != Games.HOMESCREEN)
		{
			aquiredBadges = BadgeManager.GetAquiredBadges(currentGame, pointController.GameTime, pointController.CurrentPoints);
		}
		else
		{
			aquiredBadges = BadgeManager.GetAquiredBadges(currentGame, 0, 0);
		}

		foreach (Image badge in badges)
		{
			if (aquiredBadges.ContainsKey(badge.name))
			{
				if (aquiredBadges[badge.name])
				{
					newBadges.Add(badge);
				}
				else
				{
					badge.color = Color.white;
				}
			}
		}
		if (newBadges.Count > 0)
		{
			StartCoroutine(ShowNewBadges());
		}
	}

	private IEnumerator ShowNewBadges()
	{
		foreach (Image badge in newBadges)
		{
			yield return new WaitForSeconds(1f);
			badge.color = Color.white;
			Instantiate(particleBadgeCelebration, badge.transform.position, particleBadgeCelebration.transform.rotation, UICamera.transform);
		}
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

		leaderboardNames.text = "";
		leaderboardScores.text = "";
		leaderboardTimes.text = "";


		switch (currentGame)
		{
			case Games.COLORWHEEL:

				leaderboard = Repository.GetColorWheelLeaderboard();
				break;

			case Games.TRAINGAME:

				leaderboard = Repository.GetTraingameLeaderboard();
				break;
		}

		foreach (KeyValuePair<string, List<float>> pair in leaderboard)
		{
			leaderboardCounter++;
			leaderboardNames.text += "\n" + leaderboardCounter + ": " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pair.Key);
			leaderboardScores.text += "\n" + pair.Value[0] + "p";
			leaderboardTimes.text += "\n" + GetTime(pair.Value[1]);
			if (leaderboardCounter >= maxLeaderboardPositions)
			{
				break;
			}
		}
	}
}
