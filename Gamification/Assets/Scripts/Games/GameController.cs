using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	private Games currentGame;
	[SerializeField] private PointController pointController;

	public Games CurrentGame { get { return currentGame; } }

	public void SetGame(Games game)
	{
		currentGame = game;
	}
	public void OnContinue()
	{
		AddToLeaderboard();
		SceneManager.LoadScene(1);
	}

	public void OnReset()
	{
		AddToLeaderboard();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnQuitGame()
	{
		SceneManager.LoadScene(1);
	}

	private void AddToLeaderboard()
	{
		switch (currentGame)
		{
			case Games.COLORWHEEL:
				Repository.AddToColorwheelLeaderboard(pointController.CurrentPoints);
				break;
			case Games.TRAINGAME:

				break;
		}
	}
}
